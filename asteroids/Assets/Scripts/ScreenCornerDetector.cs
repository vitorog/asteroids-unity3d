using UnityEngine;
using System.Collections;

public class ScreenCornerDetector : MonoBehaviour {
	public Camera current_cam_;
	public float dead_zone_;
	
	// Use this for initialization
	void Start () {
		dead_zone_ = 1.0f;
		current_cam_ = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 bottom_left = current_cam_.ViewportToWorldPoint(new Vector3(0,0,current_cam_.nearClipPlane));
		Vector3 top_right = current_cam_.ViewportToWorldPoint(new Vector3(1,1,current_cam_.nearClipPlane));
		bool left_screen = false;
		float new_x = transform.position.x;
		float new_y = transform.position.y;
		if(transform.position.x <= bottom_left.x - dead_zone_){
			new_x = top_right.x;
			left_screen = true;			
		}
		if(transform.position.x >= top_right.x + dead_zone_){
			new_x = bottom_left.x;			
			left_screen = true;
		}
		if(transform.position.y <= bottom_left.y - dead_zone_){
			new_y = top_right.y;
			left_screen = true;
		}
		if(transform.position.y >= top_right.y + dead_zone_){
			new_y = bottom_left.y;
			left_screen = true;
		}
		if(left_screen){
			transform.position = new Vector3(new_x,new_y,transform.position.z);
		}				
	}
}
