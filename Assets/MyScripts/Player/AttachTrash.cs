using UnityEngine;
using System.Collections;

public class AttachTrash : MonoBehaviour {
	

	public PlayerStats stats;	


	void OnTriggerEnter( Collider other)
	{
		if (other.tag == "Trash" && stats.magnetOn && transform.GetComponent<NetworkView>().isMine) 
		{
			TrashStats tStats = other.GetComponent<TrashStats>(); //Add attributes to player
			if(!tStats.isTaken){
			NetworkViewID id = Network.AllocateViewID();
			NetworkViewID oldID = other.networkView.viewID;
			other.networkView.viewID = id;

			networkView.RPC("synchID", RPCMode.Others, oldID, id);

			//Set the piece of thrash as a child to the player gameobject
			other.transform.parent = this.transform;
			
			
			stats.addAttributes(tStats.speed);
			tStats.isTaken = true;
			//Set up the joint
			FixedJoint joint;
			joint = this.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = other.rigidbody;
			other.tag = "BodyPart";
		}
		}
	}

	[RPC]
	void synchID(NetworkViewID oldID, NetworkViewID id){
		NetworkView view = NetworkView.Find(oldID);
		view.viewID = id;
		Debug.Log("AttachTrash SynchID CAlled!");
	}
	                                  
}
