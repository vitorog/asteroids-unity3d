using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum GAME_STATE
{
    MAIN_MENU,
    LEVEL_STARTING,
    PLAYING,
    LEVEL_FINISHED,
    PLAYER_DESTROYED,
    GAME_OVER,
    HIGH_SCORES
}
;

public class GameController : MonoBehaviour
{
    public GameObject player_ship_prefab_;
    public GameObject hud_player_life_prefab_;
    public Asteroid asteroid_prefab_;
    public EnemyShip ufo_prefab_;
    public GameObject music_player_prefab_;
    public GameObject high_scores_controller_prefab_;
  
    public float score_to_life_rate_;
    public float seconds_to_respawn_;
    public float hud_life_distance_;
    public float blinking_delay_;
    public float max_asteroid_torque_;
    public float min_asteroid_torque_;

    private float ufo_projectile_speed_;
    private float ufo_shooting_delay_;
    private float ufo_delay_;
    private float ufo_chance_;
    private float max_ufo_speed_;
    private float min_ufo_speed_;
    private float max_asteroid_speed_;
    private float min_asteroid_speed_;
    private int num_generated_ufos_ = 0;
    private int current_level_;
    private int max_score_;
    private int max_asteroids_;
    private GUIText current_score_text_;
    private GUIText info_text_;
    private GUIText max_score_text_;
    private GAME_STATE current_state_;
    private int current_score_;
    private int num_lives_;
    private bool num_lives_changed_;
    private List<GameObject> hud_player_lives_;
    private GameObject player_ship_intance_;
    private float respawn_timer_;
    private GameObject music_player_instance_;
    private GameObject high_scores_controller_instance_;
    private List<int> high_scores_;
    private List<string> high_scores_initials_;
    private int max_num_high_scores_;    

    //PUBLIC FUNCTIONS
    public void AddScore(int score)
    {
        current_score_ += score;
        current_score_text_.text = current_score_.ToString();
        if (current_score_ > score_to_life_rate_)
        {
            num_lives_++;
            AddLifeHUD();
            score_to_life_rate_ += score_to_life_rate_;
        }
    }

    public int GetScore()
    {
        return current_score_;
    }

    //PRIVATE FUNCTIONS
    // Use this for initialization
    void Start()
    {
        current_level_ = 1;
        max_score_ = 0;
        max_asteroids_ = 5;

        GameObject score_game_object = new GameObject("ScoreText");
        current_score_text_ = score_game_object.AddComponent<GUIText>();
        current_score_text_.gameObject.transform.position = new Vector3(0.1f, 0.95f, 0.0f);
        current_score_text_.text = "0";

        GameObject info_text_object = new GameObject("InfoText");
        info_text_ = info_text_object.AddComponent<GUIText>();
        info_text_.gameObject.transform.position = new Vector3(0.5f, 0.9f, 0.0f);
        info_text_.guiText.alignment = TextAlignment.Center;
        info_text_.guiText.anchor = TextAnchor.MiddleCenter;

        GameObject max_score_object = new GameObject("MaxScore");
        max_score_text_ = max_score_object.AddComponent<GUIText>();
        max_score_text_.gameObject.transform.position = new Vector3(0.5f, 0.95f, 0.0f);
        max_score_text_.guiText.alignment = TextAlignment.Center;
        max_score_text_.guiText.anchor = TextAnchor.MiddleCenter;
        max_score_text_.text = "0";

        current_state_ = GAME_STATE.MAIN_MENU;
        hud_player_lives_ = new List<GameObject>();

        music_player_instance_ = (GameObject)Instantiate(music_player_prefab_);        

        high_scores_initials_ = new List<string>();
        high_scores_ = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (current_state_)
        {
            case GAME_STATE.MAIN_MENU:
                OnMainMenu();
                break;
            case GAME_STATE.LEVEL_STARTING:
                OnLevelStarting();
                break;
            case GAME_STATE.PLAYING:
                OnPlaying();
                break;
            case GAME_STATE.LEVEL_FINISHED:
                OnLevelFinished();
                break;
            case GAME_STATE.PLAYER_DESTROYED:
                OnPlayerDestroyed();
                break;
            case GAME_STATE.HIGH_SCORES:                
                OnHighScores();
                break;
            case GAME_STATE.GAME_OVER:
                OnGameOver();
                break;
        }
    }

    void AddLifeHUD()
    {
        float shift = hud_player_lives_.Count * hud_life_distance_;
        GameObject hud_life = (GameObject)GameObject.Instantiate(hud_player_life_prefab_);
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(0.08f + shift, 0.9f, 0.0f));
        position.z = 0.0f;
        hud_life.transform.position = position;
        hud_player_lives_.Add(hud_life);
    }

    void GenerateCornerPositionAndDirection(out Vector3 position, out Vector3 direction)
    {
        int corner = Random.Range(1, 5); //1 = Left, 2 = Top, 3 = Right, 4 = Bottom        
        float x = 0.0f;
        float y = 0.0f;
        float x_dir = 0.0f;
        float y_dir = 0.0f;
        float min_dist_threshold = 0.1f; // This value is to prevent asteroids from having a straight line trajectory in the screen corners; 
        //they should go near the center of the screen        
        switch (corner)
        {
            case 1:
                y = Random.Range(0.0f, 1.0f);
                x_dir = 1.0f;
                y_dir = Random.Range(0.0f, 1.0f);
                while (Mathf.Abs(y - y_dir) < min_dist_threshold)
                {
                    y_dir = Random.Range(0.0f, 1.0f);
                }
                break;
            case 2:
                x = Random.Range(0.0f, 1.0f);
                y = 1.0f;

                x_dir = Random.Range(0.0f, 1.0f);
                while (Mathf.Abs(x - x_dir) < min_dist_threshold)
                {
                    x_dir = Random.Range(0.0f, 1.0f);
                }

                y_dir = 0.0f;
                break;
            case 3:
                x = 1.0f;
                y = Random.Range(0.0f, 1.0f);

                x_dir = 0.0f;
                y_dir = Random.Range(0.0f, 1.0f);
                while (Mathf.Abs(y - y_dir) < min_dist_threshold)
                {
                    y_dir = Random.Range(0.0f, 1.0f);
                }
                break;
            case 4:
                x = Random.Range(0.0f, 1.0f);

                x_dir = Random.Range(0.0f, 1.0f);
                while (Mathf.Abs(x - x_dir) < min_dist_threshold)
                {
                    x_dir = Random.Range(0.0f, 1.0f);
                }
                y_dir = 1.0f;
                break;

        }
        position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        position.z = 0.0f;

        direction = Camera.main.ViewportToWorldPoint(new Vector3(x_dir, y_dir, 0.0f)) - position;
        direction.z = 0.0f;
        direction.Normalize();
    }

    void GenerateAsteroid()
    {
        Vector3 position;
        Vector3 direction;
        GenerateCornerPositionAndDirection(out position, out direction);

        Asteroid asteroid = (Asteroid)GameObject.Instantiate(asteroid_prefab_, position, Quaternion.identity);
        float asteroid_speed = Random.Range(1, 11);
        asteroid.rigidbody2D.velocity = direction * asteroid_speed * Time.deltaTime;

        if (asteroid.rigidbody2D.velocity.magnitude > max_asteroid_speed_)
        {
            asteroid.rigidbody2D.velocity = asteroid.rigidbody2D.velocity.normalized * max_asteroid_speed_;
        }
        if (asteroid.rigidbody2D.velocity.magnitude < min_asteroid_speed_)
        {
            asteroid.rigidbody2D.velocity = asteroid.rigidbody2D.velocity.normalized * ((max_asteroid_speed_ + min_asteroid_speed_) / 2.0f);
        }

        float torque = Random.Range(min_asteroid_torque_, max_asteroid_torque_);
        asteroid.rigidbody2D.AddTorque(torque);
    }

    void GenerateUFO()
    {
        float chance = Random.Range(0.0f, 1.0f) * 100;
        if (chance <= ufo_chance_)
        {
            Vector3 position;
            Vector3 direction;
            GenerateCornerPositionAndDirection(out position, out direction);
            EnemyShip ufo = (EnemyShip)GameObject.Instantiate(ufo_prefab_, position, Quaternion.identity);
            if ((num_generated_ufos_+1) % 3 == 0)
            {              
                ufo.GetComponent<EnemyShip>().SetSmall();
            }
            ufo.GetComponent<EnemyShip>().projectile_speed_ = ufo_projectile_speed_;
            ufo.GetComponent<EnemyShip>().shooting_delay_ = ufo_shooting_delay_;            
            float ufo_speed = Random.Range(min_ufo_speed_, max_ufo_speed_);
            ufo.rigidbody2D.velocity = direction * (ufo_speed*50) * Time.deltaTime;
            num_generated_ufos_++;
        }
    }


    void SpawnPlayer()
    {
        player_ship_intance_ = (GameObject)Instantiate(player_ship_prefab_, Vector3.zero, Quaternion.identity);
    }

    void BlinkInfoText()
    {
        info_text_.enabled = !info_text_.enabled;
    }

    void Clear()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i]);
        }

        GameObject[] ufos = GameObject.FindGameObjectsWithTag("EnemyShip");
        for (int i = 0; i < ufos.Length; i++)
        {
            Destroy(ufos[i]);
        }
    }

    void InitLevel()
    {
        CancelInvoke("GenerateUFO");
        switch (current_level_)
        {
            case 1:
                ufo_chance_ = 40.0f;
                ufo_delay_ = 10.0f;
                max_asteroids_ = 5;
                max_asteroid_speed_ = 5;
                min_asteroid_speed_ = 1;
                max_ufo_speed_ = 5;
                min_ufo_speed_ = 1;
                ufo_projectile_speed_ = 400;
                ufo_shooting_delay_ = 2;
                break;
            case 2:
                ufo_chance_ = 50.0f;
                ufo_delay_ = 8.0f;
                max_asteroids_ = 6;

                max_asteroid_speed_ = 5;
                min_asteroid_speed_ = 2;
                max_ufo_speed_ = 5;
                min_ufo_speed_ = 2;

                ufo_projectile_speed_ = 500;
                ufo_shooting_delay_ = 1.5f;
                break;
            case 3:
                ufo_chance_ = 60.0f;
                ufo_delay_ = 6.0f;
                max_asteroids_ = 7;

                max_asteroid_speed_ = 6;
                min_asteroid_speed_ = 2;
                max_ufo_speed_ = 6;
                min_ufo_speed_ = 2;

                ufo_projectile_speed_ = 550;
                ufo_shooting_delay_ = 1.0f;
                break;            
            default:
                ufo_chance_+=5;
                max_asteroids_++;
                ufo_delay_ -= 0.5f;

                max_asteroid_speed_++;
                min_asteroid_speed_++;
                max_ufo_speed_++;
                min_ufo_speed_++;
                ufo_projectile_speed_ += 10;
                break;
                                
        }
        InvokeRepeating("GenerateUFO", ufo_delay_, ufo_delay_);
        for (int i = 0; i < max_asteroids_; i++)
        {
            GenerateAsteroid();
        }
    }

    //STATE FUNCTIONS
    void OnMainMenu()
    {
        info_text_.text = "PUSH START";
        if (!IsInvoking("BlinkInfoText"))
        {
            InvokeRepeating("BlinkInfoText", 0.0f, blinking_delay_);
        }
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            ufo_chance_ = 40.0f;
            ufo_delay_ = 10.0f;
            max_asteroids_ = 5;
            max_asteroid_speed_ = 5;
            min_asteroid_speed_ = 1;
            max_ufo_speed_ = 5;
            min_ufo_speed_ = 1;
            ufo_projectile_speed_ = 400;
            ufo_shooting_delay_ = 2;

            for (int i = 0; i < max_asteroids_; i++)
            {
                GenerateAsteroid();
            }
            InvokeRepeating("GenerateUFO", ufo_delay_,ufo_delay_);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            num_lives_ = 3;
            for (int i = 0; i < num_lives_; i++)
            {
                AddLifeHUD();
            }
            SpawnPlayer();
            CancelInvoke("BlinkInfoText");
            current_state_ = GAME_STATE.LEVEL_STARTING;
        }
    }

    void OnLevelStarting()
    {
        Clear();
        InitLevel();
        music_player_instance_.GetComponent<MusicPlayer>().Reset();
        music_player_instance_.GetComponent<MusicPlayer>().Play();

        current_state_ = GAME_STATE.PLAYING;
    }

    void OnPlaying()
    {
        info_text_.enabled = false;
        if (player_ship_intance_ != null && !player_ship_intance_.GetComponent<PlayerShip>().IsPlayerAlive())
        {
            current_state_ = GAME_STATE.PLAYER_DESTROYED;
        }
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 0 && GameObject.FindGameObjectsWithTag("EnemyShip").Length == 0)
        {
            music_player_instance_.GetComponent<MusicPlayer>().Stop();
            current_state_ = GAME_STATE.LEVEL_FINISHED;
        }
    }

    void OnLevelFinished()
    {
        current_level_++;
        current_state_ = GAME_STATE.LEVEL_STARTING;
    }

    void OnPlayerDestroyed()
    {
        if (player_ship_intance_ != null)
        {
            Destroy(player_ship_intance_);
            if (hud_player_lives_.Count > 0)
            {
                Destroy(hud_player_lives_[num_lives_ - 1]);
                hud_player_lives_.RemoveAt(num_lives_ - 1);
                num_lives_ -= 1;
                if (num_lives_ > 0)
                {
                    current_state_ = GAME_STATE.PLAYER_DESTROYED;
                }
                else
                {
                    current_state_ = GAME_STATE.GAME_OVER;
                }
            }
            respawn_timer_ = 0.0f;
        }

        music_player_instance_.GetComponent<MusicPlayer>().Stop();
        respawn_timer_ += Time.deltaTime;
        if (respawn_timer_ > seconds_to_respawn_)
        {
            SpawnPlayer();
            music_player_instance_.GetComponent<MusicPlayer>().Reset();
            music_player_instance_.GetComponent<MusicPlayer>().Play();
            current_state_ = GAME_STATE.PLAYING;
        }
    }

    void OnGameOver()
    {        
        info_text_.enabled = true;
        info_text_.text = "GAME OVER";
        if (Input.GetKeyDown(KeyCode.Space))
        {         
            current_state_ = GAME_STATE.HIGH_SCORES;
        }
    }

    void OnHighScores()
    {
        Clear();
        if (high_scores_controller_instance_ == null)
        {
            high_scores_controller_instance_ = (GameObject)Instantiate(high_scores_controller_prefab_);
            high_scores_controller_instance_.GetComponent<HighScoresController>().Init(high_scores_, high_scores_initials_);
        }        
        if (high_scores_controller_instance_.GetComponent<HighScoresController>().IsDone())
        {
            high_scores_controller_instance_.GetComponent<HighScoresController>().GetUpdatedHighScores(out high_scores_, out high_scores_initials_);
            Destroy(high_scores_controller_instance_);
            high_scores_controller_instance_ = null;
            
            current_score_text_.text = "0";
            if (current_score_ > max_score_)
            {
                max_score_ = current_score_;
                current_score_ = 0;
                max_score_text_.text = max_score_.ToString();
            }

            current_state_ = GAME_STATE.MAIN_MENU;
        }        
    }
}
