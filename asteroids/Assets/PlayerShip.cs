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
    public Rigidbody2D rigid_body_;
    public Projectile projectile_;

    private bool alive_;
    private bool on_hyperspace_;
    private float hyperspace_timer_;


    // Use this for initialization
    void Start()
    {        
        alive_ = true;        
    }

    public bool IsPlayerAlive()
    {       
        return alive_;
    }

    // Update is called once per frame
    void Update()
    {
        if (!on_hyperspace_)
        {
            transform.Rotate(Vector3.forward * ((-1) * Input.GetAxis("Horizontal") * rotate_speed_ * Time.deltaTime));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Projectile p = (Projectile)GameObject.Instantiate(projectile_, transform.position, transform.rotation);
                p.rigidbody2D.velocity = transform.up * projectile_speed_ * Time.deltaTime;
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
                gameObject.GetComponent<PlayerShipRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                float chance = Random.Range(0, 101);
                if (chance < hyperspace_explode_chance_)
                {
                    alive_ = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (on_hyperspace_)
        {
            return;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            rigid_body_.AddForce(transform.up * thrust_force_ * Time.deltaTime);
        }
        float curr_velocity_mag = rigid_body_.velocity.magnitude;
        if (curr_velocity_mag > max_velocity_)
        {
            Vector2 curr_velocity = rigid_body_.velocity;
            rigid_body_.velocity = curr_velocity.normalized * max_velocity_;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {        
        if (c.gameObject.tag == "Asteroid")
        {         
            alive_ = false;
        }
    }

    void Hyperspace()
    {        
        gameObject.rigidbody2D.velocity = Vector3.zero;
        float x = Random.Range(0.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        pos.z = 0.0f;
        transform.position = pos;
        on_hyperspace_ = true;
        gameObject.GetComponent<PlayerShipRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        hyperspace_timer_ = 0.0f;
    }

}
