using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender = "BigDaddy";
	public float dmg;
	private bool damageDealt = false;
	// Use this for initialization


	void OnTriggerEnter (Collider other){
		if (networkView.isMine && other.tag == "Player"){
			PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();
			if (!(sender.Length < 3) && !(EnemyStats.ID.Length < 3) && !EnemyStats.ID.Equals (this.sender)) {
				Debug.Log(sender + " hit Player " + EnemyStats.ID);	
				if(!damageDealt){
					EnemyStats.takeDamage (dmg);
					networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
				}
				networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			}
		} else if (networkView.isMine && other.tag == "BodyPart") {
			TrashStats tStats = other.GetComponent<TrashStats> ();
			if(tStats == null){
				TurretStats tuStats = other.GetComponent<TurretStats> ();
				if (!(sender.Length < 3) && tuStats.ownerID != null && !tuStats.ownerID.Equals(this.sender)) {
					Debug.Log(sender + " hit BodyPArt Turret " + tuStats.ownerID);	
					if(!damageDealt){
						tuStats.damageTaken (dmg);
						networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
					}
					networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
					return;
				}
				return;
			} else if (!(sender.Length < 3) && tStats.ownerID != null && !tStats.ownerID.Equals(this.sender)) {
				Debug.Log(sender + " hit BodyPArt " + tStats.ownerID);	
				if(!damageDealt){
					tStats.takeDamage (dmg);
					networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
				}
				networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			}
		} else if (networkView.isMine && other.tag == "Trash") { // for offline testing
			TrashStats tStats = other.GetComponent<TrashStats> ();
			Debug.Log(sender + " hit Trash " + tStats.ownerID);	
			if(!damageDealt){
				tStats.takeDamage (dmg);
				networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
			}
			networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			
		}else if (networkView.isMine && other.tag == "Enemy") { // for offline testing
			EnemyStats eStats = other.GetComponent<EnemyStats> ();
			Debug.Log(sender + " hit Enemy");	
			if(!damageDealt){
				eStats.takeDamage (dmg);
			networkView.RPC ("rpcExpendedShot", RPCMode.All, 0);
			}
			networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			
		}else if (networkView.isMine && other.tag == "Untagged") { // for terrain
			Debug.Log(sender + " hit Terrain");	
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
