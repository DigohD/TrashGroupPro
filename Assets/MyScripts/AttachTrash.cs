using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter( Collider other)
	{
		print ("Trying to connect1");
		if (other.tag == "Trash") 
		{
			//Set the piece of thrash as a child to the gameobject
			other.transform.parent = player.transform;

			//Set up the joint
			FixedJoint joint;
			joint = player.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";


			//other.rigidbody.velocity = player.rigidbody.velocity;
			//print ("Trying to connect2");
			//player.GetComponent<FixedJoint>().connectedBody = other.rigidbody;


		}

	}
	                                  
}
