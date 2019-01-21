using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour
{
    public float thrust_force_;
    public float rotate_speed_;
    public float max_velocity_;
    public float projectile_speed_;
    public float hyperspace_duration_;
    public float hyperspace_explode_chance_;
    public int max_projectiles_number_;
    public Rigidbody2D rigid_body_;
    public Projectile projectile_;
    public ParticleSystem thrust_particles_prefab_;
    public AudioClip projectile_sound_;
    public GameObject hyperspace_anim_prefab_;
    public ParticleSystem ship_explosion_prefab_;

    private bool alive_;
    private bool on_hyperspace_;
    private float hyperspace_timer_;
    private ParticleSystem thrust_particles_instance_;


    // Use this for initialization
    void Start()
    {        
        alive_ = true;
        thrust_particles_instance_ = (ParticleSystem)Instantiate(thrust_particles_prefab_);
        thrust_particles_instance_.enableEmission = false;
        thrust_particles_instance_.transform.parent = transform;

        Camera.main.GetComponent<PlayerShipRenderer>().AddPlayerShip(this);            
    }

    public bool IsPlayerAlive()
    {       
        return alive_;
    }

    public bool IsOnHyperspace()
    {
        return on_hyperspace_;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive_)
        {
            if (!on_hyperspace_)
            {
                transform.Rotate(Vector3.forward * ((-1) * Input.GetAxis("Horizontal") * rotate_speed_ * Time.deltaTime));
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    Hyperspace();
                }
            }
            else
            {
                hyperspace_timer_ += Time.deltaTime;
                if (hyperspace_timer_ > hyperspace_duration_)
                {
                    on_hyperspace_ = false;                    
                    gameObject.GetComponent<BoxCollider2D>().enabled = true;                    

                    float chance = Random.Range(0.0f, 1.0f);
                    if ((chance * 100) <= hyperspace_explode_chance_)
                    {
                        DestroyShip();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (alive_)
        {
            if (on_hyperspace_)
            {
                return;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                rigid_body_.AddForce(transform.up * thrust_force_ * Time.deltaTime);
                rigid_body_.drag = 0.0f;
                thrust_particles_instance_.transform.position = transform.position;
                thrust_particles_instance_.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
                thrust_particles_instance_.enableEmission = true;                
                if (!gameObject.GetComponents<AudioSource>()[1].isPlaying)
                {
                    gameObject.GetComponents<AudioSource>()[1].Play();
                }
            }
            else
            {
                rigid_body_.drag = 0.4f;
                thrust_particles_instance_.enableEmission = false;
                if (gameObject.GetComponents<AudioSource>()[1].isPlaying)
                {
                    gameObject.GetComponents<AudioSource>()[1].Stop();
                }
            }
            float curr_velocity_mag = rigid_body_.velocity.magnitude;
            if (curr_velocity_mag > max_velocity_)
            {
                Vector2 curr_velocity = rigid_body_.velocity;
                rigid_body_.velocity = curr_velocity.normalized * max_velocity_;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {        
        if (c.gameObject.tag == "Asteroid" || c.gameObject.tag == "EnemyProjectile" || c.gameObject.tag == "EnemyShip")
        {
            DestroyShip();
        }
    }

    void DestroyShip()
    {
        alive_ = false;
        ParticleSystem ship_explosion_instance = (ParticleSystem)Instantiate(ship_explosion_prefab_, transform.position, transform.rotation);
        ship_explosion_instance.gameObject.GetComponent<AudioSource>().Play();             
    }

    void Hyperspace()
    {        
        GameObject hyperspace_anim = (GameObject)Instantiate(hyperspace_anim_prefab_, transform.position, transform.rotation);
        hyperspace_anim.GetComponent<HyperspaceAnim>().Init(0);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        float x = Random.Range(0.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        pos.z = 0.0f;
        transform.position = pos;
        on_hyperspace_ = true;        
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        hyperspace_timer_ = 0.0f;

        hyperspace_anim = (GameObject)Instantiate(hyperspace_anim_prefab_, transform.position, transform.rotation);
        hyperspace_anim.GetComponent<HyperspaceAnim>().Init(1);

    }

    void Shoot()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerProjectile").Length < max_projectiles_number_)
        {
            Projectile p = (Projectile)GameObject.Instantiate(projectile_, transform.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().velocity = transform.up * projectile_speed_ * Time.deltaTime;

            gameObject.GetComponents<AudioSource>()[0].Play();
            
        }
    }

}
