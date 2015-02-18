using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {
	

	public PlayerStats stats;	
	void OnTriggerEnter( Collider other)
	{

		if (other.tag == "Trash" && stats.magnetOn) 
		{
			if(Network.isClient){
				Transform t = other.transform;
				GameObject newTrash = (GameObject) Network.Instantiate(other, t.position, t.rotation, 0);
				GameObject.Destroy(other);

				//Set the piece of thrash as a child to the player gameobject
				newTrash.transform.parent = this.transform;
				
				TrashStats tStats = newTrash.GetComponent<TrashStats>(); //Add attributes to player
				stats.addAttributes(tStats.speed);
				
				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint>();
				joint.connectedBody = newTrash.rigidbody;
				newTrash.tag = "BodyPart";
			}else{
				//Set the piece of thrash as a child to the player gameobject
				other.transform.parent = this.transform;
				
				TrashStats tStats = other.GetComponent<TrashStats>(); //Add attributes to player
				stats.addAttributes(tStats.speed);
				
				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint>();
				joint.connectedBody = other.rigidbody;
				other.tag = "BodyPart";
			}
		}

	}
	                                  
}
