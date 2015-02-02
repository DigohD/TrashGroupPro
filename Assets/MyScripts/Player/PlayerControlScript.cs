using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	public PlayerStats stats;
	public Camera cam;

	private float moveHorizontal;
	private float moveVertical;
	private Vector3 movement;
	private Vector3 mouse;
	private Vector3 screenPoint;
	private Vector2 offset;
	private float angle;


	void Update() {
		if (Input.GetKey ("w"))
						moveVertical = 1;
				else if (Input.GetKey ("s"))
						moveVertical = -1;
				else {
						moveVertical = 0;
				}
		if (Input.GetKey ("d"))
						moveHorizontal = 1;
				else if (Input.GetKey ("a"))
						moveHorizontal = -1;
				else {
						moveHorizontal = 0;
				}
		//uncomment to allow roation by q&e
		/*if (Input.GetKey ("e"))
			rotate = 1;
		else if (Input.GetKey ("q"))
			rotate = -1;
		else {
			rotate = 0;
		}*/

		movement = new Vector3(moveHorizontal , moveVertical, 0.0f); 
		rigidbody.velocity = movement*stats.speed;
		//transform.Rotate(new Vector3(0,0,rotate*3)); //Uncomment to allow rotation by q&e

		mouse = Input.mousePosition;
		screenPoint = cam.WorldToScreenPoint(transform.localPosition);
		offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	
	}

	/*void FixedUpdate(){

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal , moveVertical, 0.0f); 
		rigidbody.velocity = movement*stats.speed;
	}*/

}
