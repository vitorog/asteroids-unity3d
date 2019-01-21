using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum HIGH_SCORES_STATE
{
    INITIALS_INPUT,
    SCORES_DISPLAY
}

public class HighScoresController : MonoBehaviour {

    private int current_score_;
    private List<int> high_scores_;
    private List<string> high_scores_initials_;
    private List<GameObject> gui_letters_;
    private List<char> letters_;    
    private int letter_index_ = 0;
    private int num_letters_ = 3;
    private int max_num_scores_ = 10;
    private HIGH_SCORES_STATE current_state_;
    private bool done_;
    private GameObject info_text_;

    public bool IsDone()
    {
        return done_;
    }	

    public void Init(List<int> high_scores, List<string> high_scores_initials)
    {
        high_scores_ = high_scores;
        high_scores_initials_ = high_scores_initials;
        
        info_text_ = new GameObject();
        info_text_.AddComponent<GUIText>();
        info_text_.GetComponent<GUIText>().alignment = TextAlignment.Center;
        info_text_.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
        info_text_.transform.parent = transform;
        info_text_.transform.position = new Vector3(0.5f, 0.75f, 0.0f);
        
        current_score_ = GameObject.Find("GameController").GetComponent<GameController>().GetScore();        
        if (high_scores_.Count == max_num_scores_ && current_score_ < high_scores_[high_scores_.Count - 1])
        {        
            current_state_ = HIGH_SCORES_STATE.SCORES_DISPLAY;            
        }
        else
        {
            gui_letters_ = new List<GameObject>();
            letters_ = new List<char>();
            for (int i = 0; i < 3; i++)
            {
                GameObject letter = new GameObject();
                letter.transform.parent = transform;
                letter.AddComponent<GUIText>();
                letter.GetComponent<GUIText>().text = "A";
                letter.transform.position = new Vector3(0.5f + (i * 0.015f), 0.5f, 0.0f);
                letter.GetComponent<GUIText>().alignment = TextAlignment.Center;
                letter.GetComponent<GUIText>().anchor = TextAnchor.UpperCenter;
                gui_letters_.Add(letter);
                letters_.Add('A');
            }
            info_text_.GetComponent<GUIText>().text = "YOU BEAT A HIGH SCORE! ENTER YOUR INITIALS! (PRESS SPACE TO CONFIRM)";
            for (int i = 0; i < num_letters_; i++)
            {
                gui_letters_[i].GetComponent<GUIText>().enabled = true;
            }
            InvokeRepeating("BlinkLetter", 0.25f, 0.25f);
            current_state_ = HIGH_SCORES_STATE.INITIALS_INPUT;            
        }
        done_ = false;
    }

    public void GetUpdatedHighScores(out List<int> high_scores, out List<string> high_scores_initals)
    {
        high_scores = high_scores_;
        high_scores_initals = high_scores_initials_;
    }

    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () 
    {
        switch (current_state_)
        {
            case HIGH_SCORES_STATE.INITIALS_INPUT:                
                OnInitialsInput();
                break;
            case HIGH_SCORES_STATE.SCORES_DISPLAY:
                OnScoresDisplay();
                break;
        }
	}   

    public void OnInitialsInput()
    {        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < num_letters_; i++)
            {
                gui_letters_[i].GetComponent<GUIText>().enabled = true;
            }
            letter_index_++;
            if (letter_index_ >= num_letters_)
            {
                letter_index_ = num_letters_ - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = 0; i < num_letters_; i++)
            {
                gui_letters_[i].GetComponent<GUIText>().enabled = true;
            }
            letter_index_--;
            if (letter_index_ < 0)
            {
                letter_index_ = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {            
            letters_[letter_index_]++;
            if (letters_[letter_index_] > 90)
            {
                letters_[letter_index_] = (char)65;
            }
            gui_letters_[letter_index_].GetComponent<GUIText>().text = letters_[letter_index_].ToString();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {            
            letters_[letter_index_]--;
            if (letters_[letter_index_] < 65)
            {
                letters_[letter_index_] = (char)90;
            }
            gui_letters_[letter_index_].GetComponent<GUIText>().text = letters_[letter_index_].ToString();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string initials = new string(letters_.ToArray());
            if (high_scores_.Count == max_num_scores_)
            {
                high_scores_.RemoveAt(max_num_scores_ - 1);
                high_scores_initials_.RemoveAt(max_num_scores_ - 1);
            }
            high_scores_initials_.Add(initials);
            high_scores_.Add(current_score_);
            
            //Lazy sort, but  since the max number of scores is only 10, no problem... :p
            for (int i = 0; i < high_scores_.Count; i++)
            {
                for (int j = i+1; j < high_scores_.Count; j++)
                {                    
                    if (high_scores_[j] > high_scores_[i])
                    {                        
                        int aux1 = high_scores_[i];
                        string aux2 = high_scores_initials_[i];

                        high_scores_[i] = high_scores_[j];
                        high_scores_initials_[i] = high_scores_initials_[j];

                        high_scores_[j] = aux1;
                        high_scores_initials_[j] = aux2;
                    }
                }
            }
            for (int i = 0; i < num_letters_; i++)
            {
                Destroy(gui_letters_[i]);
            }
            CancelInvoke("BlinkLetter");
            current_state_ = HIGH_SCORES_STATE.SCORES_DISPLAY;
        }
    }

    public void OnScoresDisplay()
    {        
        info_text_.transform.position = new Vector3(0.5f, 0.5f, 0.0f);
        float max_value = 100000.0f;
        string text = "HIGH SCORES\n\n";
        for (int i = 0; i < high_scores_.Count; i++)
        {
            int index = i+1;
            text += index.ToString() + "\t\t\t\t\t\t\t\t" + high_scores_initials_[i] + "\t\t\t\t\t\t\t\t";
            int pads = Mathf.FloorToInt(Mathf.Log10(max_value / high_scores_[i])) + 1;
            //Bad alignment solution... maybe learn how to use gui styles later...
            for (int j = 0; j < pads; j++)
            {
                text += " ";
            }           
            text+=high_scores_[i].ToString();
            text += "\n";
        }
        info_text_.GetComponent<GUIText>().text = text;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            done_ = true;
        }
    }

    void BlinkLetter()
    {
        gui_letters_[letter_index_].gameObject.GetComponent<GUIText>().enabled = !gui_letters_[letter_index_].gameObject.GetComponent<GUIText>().enabled;
    }
}
