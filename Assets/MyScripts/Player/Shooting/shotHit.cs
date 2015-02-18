using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender;
	public float dmg;
	// Use this for initialization


	void OnTriggerEnter ( Collider other ) {
		dmg = 30;
		if (other.tag == "Player") {
			PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();
			if(sender != null && !EnemyStats.ID.Equals(this.sender)){
				Debug.Log(this.sender + " hit: " + EnemyStats.ID);
				EnemyStats.takeDamage(dmg);
				//Destroy(other.gameObject);
				Destroy(this.gameObject);
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
