using UnityEngine;
using System.Collections;

public class TrashToTrashConnector : MonoBehaviour {



	//public PlayerStats stats;
	void OnTriggerEnter (Collider other)
	{

		if(other.tag == "Trash" && this.tag == "BodyPart" && transform.parent.GetComponent<PlayerStats>().magnetOn  
		   && transform.parent.GetComponent<NetworkView>().isMine) 
		{

			TrashStats tStats = other.GetComponent<TrashStats>(); 
		if(!tStats.isTaken){
			NetworkViewID id = Network.AllocateViewID();
			NetworkViewID oldID = other.networkView.viewID;
			other.networkView.viewID = id;

			networkView.RPC("synchTTID", RPCMode.Others, oldID, id);

			//Set the piece of thrash as a child to the player gameobject
			other.transform.parent = this.transform.parent;

			//Add trash attributes to player
			PlayerStats pStats = transform.parent.GetComponent<PlayerStats>();

			pStats.addAttributes(tStats.speed);
			tStats.setTaken(pStats.ID);


			//Set up the joint
			FixedJoint joint;
			joint = this.gameObject.AddComponent<FixedJoint> ();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";
		}
		}
		if(other.tag == "PassiveTurret" && this.tag == "BodyPart" && transform.parent.GetComponent<PlayerStats>().magnetOn  
		   && transform.parent.GetComponent<NetworkView>().isMine) 
		{
			
			TurretStats tStats = other.GetComponent<TurretStats>(); 
			if(!tStats.isTaken){
				NetworkViewID id = Network.AllocateViewID();
				NetworkViewID oldID = other.networkView.viewID;
				other.networkView.viewID = id;
				
				networkView.RPC("synchTTID", RPCMode.Others, oldID, id);
				
				//Set the piece of thrash as a child to the player gameobject
				other.transform.parent = this.transform.parent;
				
				//Add trash attributes to player
				PlayerStats pStats = transform.parent.GetComponent<PlayerStats>();
				
				pStats.addAttributes(tStats.speed);
				tStats.setTaken(pStats.ID);
				
				
				//Set up the joint
				FixedJoint joint;
				joint = this.gameObject.AddComponent<FixedJoint> ();
				joint.connectedBody = other.rigidbody;
				other.tag = "BodyPart";
				other.GetComponentInChildren<WildTurretControlScript>().active = true;
			}
		}
	}

	[RPC]
	void synchTTID(NetworkViewID oldID, NetworkViewID id){
		NetworkView view = NetworkView.Find(oldID);
		view.viewID = id;
		Debug.Log("AttachTrash SynchID CAlled!");
	}
}