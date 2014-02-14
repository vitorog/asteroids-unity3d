using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public float initial_delay_ = 1.0f;
    public float min_delay_ = 0.2f;
    public float delay_decrease_rate_ = 0.05f;

    private float delay_ = 1.0f;
    private int index_ = 0;    
    private float play_timer_ = 0.0f;
    private bool playing_ = false;

    public void Stop()
    {
        playing_ = false;
    }

    public void Play()
    {
        PlayNote();
        playing_ = true;
    }

    public void Reset()
    {
        delay_ = initial_delay_;
        index_ = 0;
        play_timer_ = 0.0f;
    }

	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
        if (playing_)
        {
            play_timer_ += Time.deltaTime;
            if (play_timer_ > delay_)
            {
                PlayNote();
                play_timer_ = 0.0f;
                if (delay_ > min_delay_)
                {
                    delay_ -= delay_decrease_rate_;
                }
            }
        }
	}

 

    void PlayNote()
    {
        gameObject.GetComponents<AudioSource>()[index_].Play();
        index_ = Mathf.Abs(1 - index_);
    }
}
