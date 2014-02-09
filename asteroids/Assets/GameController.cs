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
    public GameObject player_ship_prefab_;    
    public GameObject hud_player_life_prefab_;
    public Asteroid asteroid_prefab_;
    public GUIText score_text_;
    public float hud_life_distance_;
    public float max_asteroid_speed_;
    public float min_asteroid_speed_;
    public float max_asteroid_torque_;
    public float min_asteroid_torque_;

    private GAME_STATE curr_state_;
    private int current_score_;
    private int num_lives_;
    private bool num_lives_changed_;
    private List<GameObject> hud_player_lives_;
    private GameObject instatiated_player_ship_;

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
        GenerateAsteroid();
        GenerateAsteroid();
        GenerateAsteroid();
        GenerateAsteroid();
        GenerateAsteroid();
        SpawnPlayer();
    }

    public void AddScore(int score)
    {
        current_score_ += score;
        score_text_.text = current_score_.ToString();
        if (current_score_ % 10000 == 0)
        {
            AddLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (curr_state_)
        {
            case GAME_STATE.MAIN_MENU:
                break;
            case GAME_STATE.PLAYING:                
                if (instatiated_player_ship_ != null && !instatiated_player_ship_.GetComponent<PlayerShip>().IsPlayerAlive())
                {                    
                    OnPlayerDeath();
                }
                break;
            case GAME_STATE.HIGH_SCORES:
                break;
            case GAME_STATE.GAME_OVER:
                break;
        }
    }    

    void AddLife()
    {
        float shift = hud_player_lives_.Count * hud_life_distance_;
        GameObject hud_life = (GameObject)GameObject.Instantiate(hud_player_life_prefab_);
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(0.08f + shift, 0.9f, 0.0f));
        position.z = 0.0f;
        hud_life.transform.position = position;
        hud_player_lives_.Add(hud_life);
    }

    void OnPlayerDeath()
    {
        Destroy(instatiated_player_ship_);
        if (hud_player_lives_.Count > 0)
        {
            Destroy(hud_player_lives_[num_lives_ - 1]);
            hud_player_lives_.RemoveAt(num_lives_ - 1);
            num_lives_ -= 1;
            if (num_lives_ > 0)
            {
                SpawnPlayer();
            }
            else
            {
                curr_state_ = GAME_STATE.GAME_OVER;
            }
        }
    }

    void GenerateAsteroid()
    {
        int corner = Random.Range(1, 5); //1 = Left, 2 = Top, 3 = Right, 4 = Bottom        
        float x = 0.0f;
        float y = 0.0f;
        float x_dir = 0.0f;
        float y_dir = 0.0f;
        switch (corner)
        {
            case 1:
                y = Random.Range(0.0f, 1.0f);
                x_dir = 1.0f;
                y_dir = Random.Range(0.0f, 1.0f);
                break;
            case 2:
                x = Random.Range(0.0f, 1.0f);
                y = 1.0f;

                x_dir = Random.Range(0.0f, 1.0f);
                y_dir = 0.0f;
                break;
            case 3:
                x = 1.0f;
                y = Random.Range(0.0f, 1.0f);

                x_dir = 0.0f;
                y_dir = Random.Range(0.0f, 1.0f);
                break;
            case 4:
                x = Random.Range(0.0f, 1.0f);

                x_dir = Random.Range(0.0f, 1.0f);
                y_dir = 0.0f;
                break;

        }
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        position.z = 0.0f;

        Vector3 direction = position - Camera.main.ViewportToWorldPoint(new Vector3(x_dir, y_dir, 0.0f));
        direction.Normalize();

        Asteroid asteroid = (Asteroid)GameObject.Instantiate(asteroid_prefab_, position, Quaternion.identity);
        float asteroid_speed = Random.Range(min_asteroid_speed_, max_asteroid_speed_);
        asteroid.rigidbody2D.velocity = direction * asteroid_speed * Time.deltaTime;

        float torque = Random.Range(min_asteroid_torque_, max_asteroid_torque_);
        asteroid.rigidbody2D.AddTorque(torque);
    }


    void SpawnPlayer()
    {
        instatiated_player_ship_ = (GameObject)Instantiate(player_ship_prefab_, Vector3.zero, Quaternion.identity);
    }
}
