using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender = "BigDaddy";
	public float dmg;
	// Use this for initialization


	void OnTriggerEnter ( Collider other ) {

		if (other.tag == "Player") {
						PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();
			if (!(sender.Length < 3) && !(EnemyStats.ID.Length < 3) && !EnemyStats.ID.Equals (this.sender)) {
								Debug.Log (this.sender + " hit: " + EnemyStats.ID);
								EnemyStats.takeDamage (dmg);
								Network.Destroy (this.gameObject);
						}
				} else if (other.tag == "BodyPart") {
				TrashStats tStats = other.GetComponent<TrashStats> ();
				if (!(sender.Length < 3) && tStats.ownerID != null && !tStats.ownerID.Equals (this.sender)) {
					tStats.takeDamage (dmg);
					Network.Destroy (this.gameObject);
				}
				}
	}

	public void setSender(string newSender){
		networkView.RPC ("rpcSetSender", RPCMode.All, newSender);
	}

	[RPC]
	void rpcSetSender(string newSender){
		sender = newSender;
	}


}
