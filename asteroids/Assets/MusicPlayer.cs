using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public float delay_ = 1.0f;
    public float min_delay_ = 0.2f;
    public float delay_decrease_rate_ = 0.05f;

    private int index_ = 0;    
    private float play_timer_ = 0.0f;
    
	// Use this for initialization
	void Start () {
        PlayNote();
	}
	
	// Update is called once per frame
	void Update () {
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

    void PlayNote()
    {
        gameObject.GetComponents<AudioSource>()[index_].Play();
        index_ = Mathf.Abs(1 - index_);
    }
}
