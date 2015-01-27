using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	public PlayerStats stats;
	void FixedUpdate(){

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal , moveVertical, 0.0f); 
		rigidbody.velocity = movement*stats.speed;
	}

}
