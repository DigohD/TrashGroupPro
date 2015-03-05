using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
	public float health;
	//public float scale;
	public float speed;
	public float rSpeed;
	public float boost;
	public bool isTaken;
	public string type;
	AudioSource barrelAudio;
	public string ownerID;
	public GameObject explosion;
	public float maxHealth;

	private NetworkViewID parent;
	private float timer;
	private bool destruct;

	void Start ()
	{
		if(type.Equals("Barrel")){
			barrelAudio = GetComponent<AudioSource> ();
		}
		maxHealth = health;
	}


	public void setTaken(string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		ownerID = newOwnerID;
		parent = newParent;
		networkView.RPC("rpcSetTTaken", RPCMode.Others, 0, newOwnerID, newParent);
	}

	public int takeDamage (float incDmg, int comboCount) {
				//if (health > 0) {
						health -= incDmg;
						int initComboCount = comboCount;
						gameObject.GetComponent<DamageBarrel> ().barrelHit ();
						
						if (type.Equals ("Barrel"))
								barrelAudio.Play ();

						if (health <= 0) {
								comboCount++;
								print ("health zero, time to delete");
								List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
								foreach (NetworkViewID childID in banana) {
										Debug.Log ("damaging: " + gameObject);
										GameObject child = NetworkView.Find(childID).gameObject;
										if (child != null) {
												if (child.GetComponent<TrashType> ().type.Equals("Trash"))
														comboCount = child.GetComponent<TrashStats> ().delayedDestruction (0.5f, comboCount);
												if (child.GetComponent<TrashType> ().type.Equals("Turret"))
														comboCount = child.GetComponent<TurretStats> ().delayedDestruction (0.5f, comboCount);
											//banana.Remove(child.networkView.viewID);
										}
								}
								
								if(isTaken && initComboCount == 0)
									NetworkView.Find(parent).gameObject.GetComponent<ChildList>().removeChild(networkView.viewID);
								Network.Destroy (this.gameObject);
								Network.Instantiate (explosion, transform.position, transform.rotation, 0);	
						}
				//}
				return comboCount;
		}

	public void Update(){
		if (destruct) {
			timer-=Time.deltaTime;
			if(timer <=0){
				Network.Destroy (this.gameObject);
				Network.Instantiate (explosion, transform.position, transform.rotation, 0);
				
			}
		}
		
	}
	
	
	public int delayedDestruction(float time, int combo){
		destruct = true;
		timer = time;
		combo ++;
		List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
		foreach (NetworkViewID childID in banana) {
			GameObject child = NetworkView.Find(childID).gameObject;
			if (child != null){
				if (child.GetComponent<TrashType> ().type.Equals("Trash"))
					combo = child.GetComponent<TrashStats> ().delayedDestruction (time + 0.5f, combo);
				if (child.GetComponent<TrashType> ().type.Equals("Turret"))
					combo = child.GetComponent<TurretStats> ().delayedDestruction (time + 0.5f, combo);
				//banana.Remove(child.networkView.viewID);						
			}
		}
		return combo;
	}

	public void setToBodyPart(){
		networkView.RPC ("rpcSetBodyPart", RPCMode.All, 0);
	}

	[RPC]
	void rpcSetTTaken(int wasted, string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
	}

	[RPC]
	void rpcSetBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
	
}
