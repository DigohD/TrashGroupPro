using UnityEngine;
using System.Collections;

public class TrashToTrashConnector : MonoBehaviour {

	public GameObject player;
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Trash") 
		{
			//Set the piece of thrash as a child to the gameobject
			other.transform.parent = player.transform;
			
			//Set up the joint
			FixedJoint joint;
			joint = player.gameObject.AddComponent<FixedJoint> ();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";
		}
	}
}