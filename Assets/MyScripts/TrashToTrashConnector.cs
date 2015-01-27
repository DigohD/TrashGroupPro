using UnityEngine;
using System.Collections;

public class TrashToTrashConnector : MonoBehaviour {

	//public GameObject player;
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Trash" && this.tag == "BodyPart") 
		{
			//Set the piece of thrash as a child to the player gameobject
			other.transform.parent = this.transform.parent;
			
			//Set up the joint
			FixedJoint joint;
			joint = this.gameObject.AddComponent<FixedJoint> ();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";
		}
	}
}