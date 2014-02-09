using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum GAME_STATE
{
    MAIN_MENU,
    PLAYING,
    GAME_OVER,
    HIGH_SCORES
}
;

public class GameController : MonoBehaviour
{
    public PlayerShip player_ship_;
    public GUIText score_text_;
    public GameObject hud_player_life_;
    public float hud_life_distance_;

    private GAME_STATE curr_state_;
    private int current_score_;
    private int num_lives_;
    private bool num_lives_changed_;
    private List<GameObject> hud_player_lives_;

    // Use this for initialization
    void Start()
    {
        num_lives_ = 3;
        hud_player_lives_ = new List<GameObject>();
        for (int i = 0; i < num_lives_; i++)
        {
            AddLife();
        }
        curr_state_ = GAME_STATE.PLAYING;
        GameObject.Instantiate(player_ship_);        
    }

    // Update is called once per frame
    void Update()
    {
        switch (curr_state_)
        {
            case GAME_STATE.MAIN_MENU:
                break;
            case GAME_STATE.PLAYING:
                break;
            case GAME_STATE.HIGH_SCORES:
                break;
            case GAME_STATE.GAME_OVER:
                break;
        }
    }

    void AddScore(int score)
    {
        current_score_ += score;
        score_text_.text = current_score_.ToString();
        if (current_score_ % 10.000 == 0)
        {
            AddLife();
        }
    }

    void AddLife()
    {
        float shift = hud_player_lives_.Count * hud_life_distance_;
        GameObject hud_life = (GameObject)GameObject.Instantiate(hud_player_life_);        
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(0.08f + shift, 0.9f, 0.0f));
        position.z = 0.0f;
        hud_life.transform.position = position;
        hud_player_lives_.Add(hud_life);
    }

    void OnPlayerDeath()
    {
        Destroy(hud_player_lives_[num_lives_ - 1]);
        hud_player_lives_.RemoveAt(num_lives_ - 1);
        num_lives_ -= 1;
    }
}
