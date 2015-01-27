using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {
	

	void OnTriggerEnter( Collider other)
	{

		if (other.tag == "Trash") 
		{
			//Set the piece of thrash as a child to the player gameobject
			other.transform.parent = this.transform;


			//Set up the joint
			FixedJoint joint;
			joint = this.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";


		}

	}
	                                  
}
