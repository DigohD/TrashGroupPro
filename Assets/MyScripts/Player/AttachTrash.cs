using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {
	

	public PlayerStats stats;	
	public GameObject trash, trashBattery;


	void OnTriggerEnter( Collider other)
	{
		if (other.tag == "Trash" && stats.magnetOn) 
		{
			if(Network.isClient){
				Transform t = other.transform;
				GameObject newTrash = null;
				if(other.name.Equals("Trash")){
					newTrash = (GameObject) Network.Instantiate(trash, t.position, t.rotation, 0);
				}else if(other.name.Contains("Battery")){
					newTrash = (GameObject) Network.Instantiate(trashBattery, t.position, t.rotation, 0);
				}
				Network.Destroy(other.networkView.viewID);

				if(newTrash == null){
					Debug.LogError("Trash collision return null");
					return;
				}

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
