using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour
{
		public float thrust_force_;
		public float rotate_speed_;
		public float max_velocity_;
		public Rigidbody2D rigid_body_;
		private Material line_material_;
		private List<Vector3> vertices_;
		// Use this for initialization
		void Start ()
		{
				CreateLineMaterial ();
				Vector3 p1 = new Vector3 (-0.5f, -0.5f, 0.0f);
				Vector3 p2 = new Vector3 (0.0f, 0.5f, 0.0f);
				Vector3 p3 = new Vector3 (0.5f, -0.5f, 0.0f);
				Vector3 p4 = new Vector3 (0.25f, -0.25f, 0.0f);
				Vector3 p5 = new Vector3 (-0.25f, -0.25f, 0.0f);
				vertices_ = new List<Vector3> ();
				vertices_.Add (p1);
				vertices_.Add (p2);
				vertices_.Add (p3);
				vertices_.Add (p4);
				vertices_.Add (p5);
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
    
		void CreateLineMaterial ()
		{
				if (!line_material_) {
						line_material_ = new Material ("Shader \"Lines/Colored Blended\" {" +
								"SubShader { Pass { " +
								"    Blend SrcAlpha OneMinusSrcAlpha " +
								"    ZWrite Off Cull Off Fog { Mode Off } " +
								"    BindChannels {" +
								"      Bind \"vertex\", vertex Bind \"color\", color }" +
								"} } }");
						line_material_.hideFlags = HideFlags.HideAndDontSave;
						line_material_.shader.hideFlags = HideFlags.HideAndDontSave;
				}
		}
	
		void OnGUI ()
		{
				RenderShip ();
		}
		
		void RenderShip ()
		{
				line_material_.SetPass (0);
				GL.PushMatrix ();		
				Matrix4x4 trs_matrix = Matrix4x4.TRS (transform.position, transform.rotation, Vector3.one);
				GL.MultMatrix (trs_matrix);		
				DrawLines ();
				DrawSquares (0.02f);
				GL.PopMatrix ();
		}
    
		void DrawLines ()
		{
				GL.Color (new Color (1.0f, 1.0f, 1.0f, 0.5f));
				GL.Begin (GL.LINES);
				for (int i = 0; i < vertices_.Count; i++) {
						GL.Vertex (vertices_ [i]);
						if (i < vertices_.Count - 1) {
								GL.Vertex (vertices_ [i + 1]);
						}				
				}
				GL.Vertex (vertices_ [0]);
				GL.End ();
		}
		
		void DrawSquares (float size)
		{
				float half_size = size / 2.0f;
				GL.Color (new Color (1.0f, 1.0f, 1.0f, 1.0f));
				GL.Begin (GL.QUADS);
				for (int i = 0; i < vertices_.Count; i++) {
						Vector3 vertice = vertices_ [i];
						GL.Vertex3 (vertice.x - half_size, vertice.y - half_size, 0.0f);
						GL.Vertex3 (vertice.x + half_size, vertice.y - half_size, 0.0f);
						GL.Vertex3 (vertice.x + half_size, vertice.y + half_size, 0.0f);
						GL.Vertex3 (vertice.x - half_size, vertice.y + half_size, 0.0f);
				}				
				GL.End ();
		}
    
}
