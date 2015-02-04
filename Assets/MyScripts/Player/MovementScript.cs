using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {


	public float rotation = 100;//might add to playerstats laters
	public PlayerStats stats;
	public float maxSpeed;

	public void MoveUp(){
		if (rigidbody.velocity.magnitude < maxSpeed)
			rigidbody.AddForce (Vector3.up*stats.speed, ForceMode.Force);
	}
	public void MoveDown(){
		if (rigidbody.velocity.magnitude < maxSpeed)
			rigidbody.AddForce (-Vector3.up*stats.speed);
	}
	public void MoveRight(){
		if (rigidbody.velocity.magnitude < maxSpeed)
			rigidbody.AddForce (Vector3.right*stats.speed);
	}
	public void MoveLeft(){
		if (rigidbody.velocity.magnitude < maxSpeed)
			rigidbody.AddForce (-Vector3.right*stats.speed);
	}
	public void RotateRight(){
		rigidbody.AddTorque (0, 0, -rotation * Time.deltaTime);
	}
	public void RotateLeft(){
		rigidbody.AddTorque (0, 0, rotation * Time.deltaTime);
	}

}
