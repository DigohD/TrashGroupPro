using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {


	public float rotation = 100;//might add to playerstats laters
	public PlayerStats stats;
	public float maxSpeed;

	public void MoveUp(){
		if(networkView.isMine)
			if (rigidbody.velocity.magnitude < maxSpeed)
				rigidbody.AddForce (Vector3.up*stats.speed, ForceMode.Force);
	}
	public void MoveDown(){
		if(networkView.isMine)
			if (rigidbody.velocity.magnitude < maxSpeed)
				rigidbody.AddForce (-Vector3.up*stats.speed);
	}
	public void MoveRight(){
		if(networkView.isMine)
			if(rigidbody.velocity.magnitude < maxSpeed)
				rigidbody.AddForce (Vector3.right*stats.speed);
	}
	public void MoveLeft(){
		if(networkView.isMine)
			if (rigidbody.velocity.magnitude < maxSpeed)
				rigidbody.AddForce (-Vector3.right*stats.speed);
	}
	public void RotateRight(){
		if(networkView.isMine)
			rigidbody.AddTorque (0, 0, -rotation * Time.deltaTime * stats.rSpeed);
	}
	public void RotateLeft(){
		if(networkView.isMine)
			rigidbody.AddTorque (0, 0, rotation * Time.deltaTime * stats.rSpeed);
	}

}
