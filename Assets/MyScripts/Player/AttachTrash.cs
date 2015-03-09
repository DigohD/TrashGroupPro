using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {
	

	public PlayerStats stats;	


	void OnTriggerEnter( Collider other)
	{
		// If for some reason we collided with nothing, return.
		if(other == null)
			return;

		/* if:
		 * - Colliding with trash
		 * - And the magnet is on
		 * - And this is my player object
		 */
		if (other.tag == "Trash" && stats.magnetOn && networkView.isMine) 
		{
			// fetch the trash stats of the collided object
			TrashStats tStats = other.GetComponent<TrashStats>(); //Add attributes to player

			// if the trash is not already attached to a player, pick it up
			if(!tStats.isTaken){
				// Create and allocate a new network view ID for the trash. So that it is simulated on my client
				NetworkViewID id = Network.AllocateViewID();
				NetworkViewID oldID = other.networkView.viewID;
				other.networkView.viewID = id;
				// Make sure other clients get this change as well
				networkView.RPC("synchID", RPCMode.Others, oldID, id);

				// Set the player object as the trash parent
				other.transform.parent = this.transform;
				// Set the piece of thrash as a child to the player gameobject
				this.gameObject.GetComponent<ChildList>().addChild(other.networkView.viewID);

				// Add trash stats to player stats
				stats.addAttributes(tStats.speed, tStats.rSpeed);
				// Mark the trash as picked up
				tStats.setTaken(stats.ID, networkView.viewID);

				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint>();
				joint.connectedBody = other.rigidbody;
				tStats.setToBodyPart();
			}
		}
		/* else if:
		 * - Colliding with a trash turret
		 * - And the magnet is on
		 * - And this is my player object
		 */
		else if (other.tag == "PassiveTurret" && stats.magnetOn && networkView.isMine) 
		{
			// fetch the turret stats of the collided object
			TurretStats tStats = other.GetComponent<TurretStats>(); //Add attributes to player

			// if the turret is not already attached to a player, pick it up
			if(!tStats.isTaken){
				// Create and allocate a new network view ID for the trash. So that it is simulated on my client
				NetworkViewID id = Network.AllocateViewID();
				NetworkViewID oldID = other.networkView.viewID;
				other.networkView.viewID = id;
				// Make sure other clients get this change as well
				networkView.RPC("synchID", RPCMode.Others, oldID, id);
				
				// Set the player object as the trash parent
				other.transform.parent = this.transform;
				//Set the piece of thrash as a child to the player gameobject
				this.gameObject.GetComponent<ChildList>().addChild(other.networkView.viewID);

				// Add trash stats to player stats
				stats.addAttributes(tStats.speed, tStats.rSpeed);
				// Mark the trash as picked up
				tStats.setTaken(stats.ID, networkView.viewID);

				// Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint>();
				joint.connectedBody = other.rigidbody;
				tStats.setToBodyPart();
				// Activate the picked up turret
				other.GetComponentInChildren<WildTurretControlScript>().activate();
			}
		}
	}

	// Synchronizes the new network allocation, so that trash picked up by another client
	// is no longer simulated on this machine.
	[RPC]
	void synchID(NetworkViewID oldID, NetworkViewID id){
		NetworkView view = NetworkView.Find(oldID);
		view.viewID = id;
	}	                                  
}
