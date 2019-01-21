using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShip : MonoBehaviour
{
    public float velocity_;
    public float projectile_speed_;
    public float shooting_delay_;
    public Projectile projectile_;
    public AudioClip projectile_sound_;
    public ParticleSystem ship_explosion_prefab_;    
    public float big_ufo_min_angle_;
    public float big_ufo_max_angle_;
    public float small_ufo_min_angle_;
    public float small_ufo_max_angle_;
    public int small_ufo_score_;
    public int big_ufo_score_;    

 
    private float min_angle_;
    private float max_angle_;
    private int score_;

    private bool is_alive_;

    void Awake()
    {
        min_angle_ = big_ufo_min_angle_;
        max_angle_ = big_ufo_max_angle_;
        score_ = big_ufo_score_;
    }

    // Use this for initialization
    void Start()
    {
        is_alive_ = true;
        InvokeRepeating("Shoot", 1.0f, shooting_delay_);
        Camera.main.GetComponent<EnemyShipRenderer>().AddEnemyShip(this);
    }

    public void SetSmall()
    {        
        transform.localScale = transform.localScale / 1.5f;
        min_angle_ = small_ufo_min_angle_;
        max_angle_ = small_ufo_max_angle_;
        score_ = small_ufo_score_;
    }    

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Asteroid" || c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "Player")
        {
            ParticleSystem ship_explosion_instance = (ParticleSystem)Instantiate(ship_explosion_prefab_, transform.position, transform.rotation);
            ship_explosion_instance.gameObject.GetComponent<AudioSource>().Play();
            if (c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "Player")
            {
                GameObject.Find("GameController").GetComponent<GameController>().AddScore(score_);
            }
            is_alive_ = false;           
        }
    }

    void Shoot()
    {
        GameObject player_ship = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = new Vector3(0.0f, 1.0f, 0.0f);
        if (player_ship != null)
        {
            dir = player_ship.transform.position - transform.position;
            dir.Normalize();
        }
        Projectile p = (Projectile)GameObject.Instantiate(projectile_, transform.position, transform.rotation);
        p.tag = "EnemyProjectile";
        p.gameObject.layer = 12;

        float projectile_precision = Random.Range(min_angle_, max_angle_);                
        dir = Quaternion.Euler(0.0f, 0.0f, projectile_precision) * dir;
        p.GetComponent<Rigidbody2D>().velocity = dir * projectile_speed_ * Time.deltaTime;
        p.GetComponent<Projectile>().max_time_alive_ = 3.0f;

        gameObject.GetComponents<AudioSource>()[0].Play();        
    }

    //Moved destruction code to LateUpdate function because it seems there is a problem 
    //when handling collision between objects: if each object destroys itself, 
    //sometimes only one of the OnCollisionEnter is called (Unity bug?)
    //Not sure if it will work in all cases and could not find references...
    void LateUpdate()
    {
        Destroy(gameObject);
    }
    

}

