using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {
	//This script is no longer being used
	public PlayerStats stats;
	public Camera cam;
	public float rotation;

	private float moveHorizontal;
	private float moveVertical;
	private Vector3 movement;
	private Vector3 mouse;
	private Vector3 screenPoint;
	private Vector2 offset;
	private float angle;
	private float boost;


	void FixedUpdate() {

		if(!networkView.isMine)
			return;

		if (Input.GetKey ("up"))
			rigidbody.AddForce (Vector3.up*stats.speed, ForceMode.Force);//moveVertical = 1;
				else if (Input.GetKey ("down"))
			rigidbody.AddForce (-Vector3.up*stats.speed);
				//else {
				//		moveVertical = 0;
				//}
		if (Input.GetKey ("right"))
			rigidbody.AddForce (Vector3.right*stats.speed);
				else if (Input.GetKey ("left"))
			rigidbody.AddForce (-Vector3.right*stats.speed);
				//else {
			//			moveHorizontal = 0;
			//	}

		if (Input.GetKey ("e"))
			rigidbody.AddTorque (0, 0, rotation * Time.deltaTime);
		else if (Input.GetKey ("q"))
			rigidbody.AddTorque (0, 0, -rotation * Time.deltaTime);


		if (Input.GetKey ("space"))
						boost = 2;
				else
						boost = 1;
		//movement = new Vector3(moveHorizontal , moveVertical, 0.0f); 
		//rigidbody.velocity = movement*stats.speed*boost;
		//transform.Rotate(new Vector3(0,0,rotate*3)); //Uncomment to allow rotation by q&e





		//new angle calc:

	//	Vector2 target = mouse - transform.localRotation;
		//float angle2 = Mathf.Atan2( target.y, target.x )* Mathf.Rad2Deg;


		//yet another....

		//transform.rotation = Quaternion.Euler(0, 0, angle);//rotation by transformation
		//more "physics"
		//Vector3 torqueVector = Vector3.Cross(screenPoint, Vector3.forward);
		//torqueVector = Vector3.Project(torqueVector, transform.forward);
		//print (angle2);
		//if (angle2 > 180)
		//				angle2 -= 360;
		//rigidbody.AddTorque(transform.up * angle2*4);
	
	}

	/*void FixedUpdate(){

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal , moveVertical, 0.0f); 
		rigidbody.velocity = movement*stats.speed;
	}*/

}
