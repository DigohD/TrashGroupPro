using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender = "BigDaddy";
	public float dmg;
	private bool damageDealt = false;
	// Use this for initialization


	void OnTriggerEnter ( Collider other ) {
		if (networkView.isMine && other.tag == "Player") {
						PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();
						if (!(sender.Length < 3) && !(EnemyStats.ID.Length < 3) && !EnemyStats.ID.Equals (this.sender)) {
								Debug.Log (this.sender + " hit: " + EnemyStats.ID);
							if(!damageDealt)
								EnemyStats.takeDamage (dmg);
							networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
							networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
						}
				} else if (other.tag == "BodyPart") {
						TrashStats tStats = other.GetComponent<TrashStats> ();
						if (!(sender.Length < 3) && tStats.ownerID != null && !tStats.ownerID.Equals (this.sender)) {
							if(!damageDealt)
								tStats.takeDamage (dmg);
							networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
							networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
						}
				} else if (other.tag == "Trash") { // for offline testing
						TrashStats tStats = other.GetComponent<TrashStats> ();
						
						if(!damageDealt)
							tStats.takeDamage (dmg);
						networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
						networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
						
				}else if (other.tag == "Enemy") { // for offline testing
						EnemyStats eStats = other.GetComponent<EnemyStats> ();
						
						if(!damageDealt)
							eStats.takeDamage (dmg);
						networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
						networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
						
				}else if (other.tag == "Untagged") { // for terrain
						networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			
				}
	}
	
	public void setSender(string newSender){
		networkView.RPC ("rpcSetSender", RPCMode.All, newSender);
	}

	[RPC]
	void rpcSetSender(string newSender){
		sender = newSender;
	}

	[RPC]
	void rpcExpendedShot(int wasted){
		damageDealt = true;
	}

	[RPC]
	void rpcDestroyShot(int wasted){
		Destroy (this.gameObject);
	}


}
