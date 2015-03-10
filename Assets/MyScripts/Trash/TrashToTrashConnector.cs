using UnityEngine;
using System.Collections;

public class TrashToTrashConnector : MonoBehaviour {

	//public PlayerStats stats;
	void OnTriggerEnter (Collider other)
	{
		// If it's not the local player collecting the trash, ignore the collision
		if(!networkView.isMine)
			return;

		// Check the necessary conditions for trash-to-trash connectivity
		if(other.tag == "Trash" && this.tag == "BodyPart" && transform.parent.GetComponent<PlayerStats>().magnetOn  
		   && networkView.isMine) 
		{
			TrashStats tStats = other.GetComponent<TrashStats>(); 
			// if the trash is not already attached to a player, pick it up
			if(!tStats.isTaken){
				// Create and allocate a new network view ID for the trash. So that it is simulated on local client
				NetworkViewID id = Network.AllocateViewID();
				NetworkViewID oldID = other.networkView.viewID;
				other.networkView.viewID = id;
				// Make sure other clients get this change as well
				networkView.RPC("synchTTID", RPCMode.Others, oldID, id);

				// Set this object as the picked up trash's parent
				other.transform.parent = this.transform.parent;
				// Set the picked up thrash as a child of this trash
				this.gameObject.GetComponent<ChildList>().addChild(other.networkView.viewID);

				// Add trash attributes to player
				PlayerStats pStats = transform.parent.GetComponent<PlayerStats>();
				pStats.addAttributes(tStats.speed, tStats.rSpeed);

				GameObject[] go = GameObject.FindGameObjectsWithTag("mainControl");
				UIHandler uih = go[0].GetComponent<UIHandler>();
				uih.powerUp(tStats.speed, tStats.gameObject.GetComponent<Rigidbody>().mass, false);

				// Update picked up trash's ownership
				tStats.setTaken(pStats.ID, networkView.viewID);


				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint> ();
				joint.connectedBody = other.rigidbody;
				tStats.setToBodyPart();
			}
		}
		// Check the necessary conditions for trash-to-turret connectivity
		else if(other.tag == "PassiveTurret" && this.tag == "BodyPart" && transform.parent.GetComponent<PlayerStats>().magnetOn  
		        && networkView.isMine) 
		{
			TurretStats tStats = other.GetComponent<TurretStats>(); 
			// if the turret is not already attached to a player, pick it up
			if(!tStats.isTaken){
				// Create and allocate a new network view ID for the turret. So that it is simulated on my client
				NetworkViewID id = Network.AllocateViewID();
				NetworkViewID oldID = other.networkView.viewID;
				other.networkView.viewID = id;
				// Make sure other clients get this change as well
				networkView.RPC("synchTTID", RPCMode.Others, oldID, id);

				// Set this object as the picked up turret's parent
				other.transform.parent = this.transform.parent;
				// Set the picked up turret as a child of this trash
				this.gameObject.GetComponent<ChildList>().addChild(other.networkView.viewID);
				
				// Add trash attributes to player
				PlayerStats pStats = transform.parent.GetComponent<PlayerStats>();
				pStats.addAttributes(tStats.speed, tStats.rSpeed);


				GameObject[] go = GameObject.FindGameObjectsWithTag("mainControl");
				UIHandler uih = go[0].GetComponent<UIHandler>();
				uih.powerUp(tStats.speed, tStats.gameObject.GetComponent<Rigidbody>().mass, true);


				// Update picked up turret's ownership
				tStats.setTaken(pStats.ID, networkView.viewID);
				
				
				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint> ();
				joint.connectedBody = other.rigidbody;
				other.tag = "BodyPart";
				tStats.setToBodyPart();
				// Activate the turret
				other.GetComponentInChildren<WildTurretControlScript>().activate();
			}
		}
	}

	// Used to synchronize network allocation, for this turret, over the network
	[RPC]
	void synchTTID(NetworkViewID oldID, NetworkViewID id){
		NetworkView view = NetworkView.Find(oldID);
		view.viewID = id;
	}
}