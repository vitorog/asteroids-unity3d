using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour
{
    public float thrust_force_;
    public float rotate_speed_;
    public float max_velocity_;
    public float projectile_speed_;
    public Rigidbody2D rigid_body_;
    public Projectile projectile_;

    private bool is_alive_;


    // Use this for initialization
    void Start()
    {        
        is_alive_ = true;        
    }

    public bool IsPlayerAlive()
    {       
        return is_alive_;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * ((-1) * Input.GetAxis("Horizontal") * rotate_speed_ * Time.deltaTime));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Projectile p = (Projectile)GameObject.Instantiate(projectile_,transform.position,transform.rotation);
            p.rigidbody2D.velocity = transform.up * projectile_speed_ * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
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
            is_alive_ = false;
        }
    }

    

}
