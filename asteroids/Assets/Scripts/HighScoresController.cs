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
    private List<GameObject> high_scores_text_;
    private int letter_index_ = 0;
    private int num_letters_ = 3;
    private int max_num_scores_ = 10;
    private HIGH_SCORES_STATE current_state_ = HIGH_SCORES_STATE.INITIALS_INPUT;
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
        gui_letters_ = new List<GameObject>();
        high_scores_text_ = new List<GameObject>();
        letters_ = new List<char>();
        for (int i = 0; i < 3; i++)
        {
            GameObject letter = new GameObject();
            letter.transform.parent = transform;
            letter.AddComponent<GUIText>();
            letter.GetComponent<GUIText>().text = "A";
            letter.transform.position = new Vector3(0.5f + (i * 0.015f), 0.5f, 0.0f);
            letter.GetComponent<GUIText>().alignment = TextAlignment.Center;
            letter.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
            gui_letters_.Add(letter);
            letters_.Add('A');
        }
        for (int i = 0; i < max_num_scores_; i++)
        {
            GameObject high_score_text = new GameObject();
            high_score_text.transform.parent = transform;
            high_score_text.AddComponent<GUIText>();
            high_score_text.GetComponent<GUIText>().alignment = TextAlignment.Center;
            high_score_text.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;

            high_scores_text_.Add(high_score_text);           
        }

        if (high_scores_.Count < max_num_scores_)
        {
            for (int i = high_scores_.Count; i < max_num_scores_; i++)
            {
                high_scores_.Add(0);
                high_scores_initials_.Add("AAA");
            }
        }

        info_text_ = new GameObject();
        info_text_.AddComponent<GUIText>();
        info_text_.GetComponent<GUIText>().alignment = TextAlignment.Center;
        info_text_.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
        info_text_.transform.parent = transform;
        info_text_.transform.position = new Vector3(0.5f, 0.75f, 0.0f);

        InvokeRepeating("BlinkLetter", 0.25f, 0.25f);
        current_score_ = GameObject.Find("GameController").GetComponent<GameController>().GetScore();
        if (high_scores_.Count == max_num_scores_ && current_score_ < high_scores_[high_scores_.Count - 1])
        {
            current_state_ = HIGH_SCORES_STATE.SCORES_DISPLAY;            
            info_text_.guiText.text = "HIGH SCORES";
        }
        else
        {            
            info_text_.guiText.text = "YOU BEAT A HIGH SCORE! ENTER YOUR INITIALS! (PRESS SPACE TO CONFIRM)";
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
            gui_letters_[letter_index_].gameObject.guiText.enabled = true;
            letter_index_++;
            if (letter_index_ >= num_letters_)
            {
                letter_index_ = num_letters_ - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gui_letters_[letter_index_].gameObject.guiText.enabled = true;
            letter_index_--;
            if (letter_index_ < 0)
            {
                letter_index_ = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gui_letters_[letter_index_].gameObject.guiText.enabled = true;
            letters_[letter_index_]++;
            if (letters_[letter_index_] > 90)
            {
                letters_[letter_index_] = (char)65;
            }
            gui_letters_[letter_index_].guiText.text = letters_[letter_index_].ToString();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gui_letters_[letter_index_].gameObject.guiText.enabled = true;
            letters_[letter_index_]--;
            if (letters_[letter_index_] < 65)
            {
                letters_[letter_index_] = (char)90;
            }
            gui_letters_[letter_index_].guiText.text = letters_[letter_index_].ToString();
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
            for (int i = 0; i < max_num_scores_; i++)
            {
                for (int j = i+1; j < max_num_scores_; j++)
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
                gui_letters_[i].guiText.enabled = false;
            }
            CancelInvoke("BlinkLetter");
            current_state_ = HIGH_SCORES_STATE.SCORES_DISPLAY;
        }
    }

    public void OnScoresDisplay()
    {
        info_text_.guiText.text = "HIGH SCORES";
        for (int i = 0; i < high_scores_.Count; i++)
        {
            int index = i+1;
            string text = index.ToString() + " " + high_scores_initials_[i] + " " + high_scores_[i].ToString();
            high_scores_text_[i].guiText.text = text;
            high_scores_text_[i].transform.position = new Vector3(0.5f, 0.5f - (i * 0.03f), 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            done_ = true;
        }
    }

    void BlinkLetter()
    {
        gui_letters_[letter_index_].gameObject.guiText.enabled = !gui_letters_[letter_index_].gameObject.guiText.enabled;
    }
}
