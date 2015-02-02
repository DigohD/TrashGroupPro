using UnityEngine;
using System.Collections;

public class TrashToTrashConnector : MonoBehaviour {

	//public PlayerStats stats;
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Trash" && this.tag == "BodyPart") 
		{
			//Set the piece of thrash as a child to the player gameobject
			other.transform.parent = this.transform.parent;

			//Add trash attributes to player
			PlayerStats pStats = transform.parent.GetComponent<PlayerStats>();
			TrashStats tStats = other.GetComponent<TrashStats>(); 
			pStats.addAttributes(tStats.speed);

			//Set up the joint
			FixedJoint joint;
			joint = this.gameObject.AddComponent<FixedJoint> ();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";
		}
	}
}