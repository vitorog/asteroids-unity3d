using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour
{
		public float thrust_force_;
		public float rotate_speed_;
		public float max_velocity_;
		public Rigidbody2D rigid_body_;		
		// Use this for initialization
		void Start ()
		{				
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.Rotate (Vector3.forward * ((-1) * Input.GetAxis ("Horizontal") * rotate_speed_ * Time.deltaTime));
        }
	
		void FixedUpdate ()
		{					
				if (Input.GetAxis ("Vertical") > 0) { 
						rigid_body_.AddForce (transform.up * thrust_force_ * Time.deltaTime); 
				}
				float curr_velocity_mag = rigid_body_.velocity.magnitude;
				if (curr_velocity_mag > max_velocity_) {
						Vector2 curr_velocity = rigid_body_.velocity;
						rigid_body_.velocity = curr_velocity.normalized * max_velocity_;
				}	
		}
    
}
