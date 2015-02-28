using UnityEngine;
using System.Collections;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
	public float health;
	//public float scale;
	public float speed;
	public float boost;
	public bool isTaken;
	public string type;
	AudioSource barrelAudio;
	public string ownerID;
	public GameObject explosion;
	public float maxHealth;

	void Start ()
	{
		if(type.Equals("Barrel")){
			barrelAudio = GetComponent<AudioSource> ();
		}
		maxHealth = health;
	}


	public void setTaken(string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
		networkView.RPC("rpcSetTTaken", RPCMode.Others, 0, newOwnerID);
	}

	public void takeDamage ( float incDmg ) {
		health -= incDmg;
		gameObject.GetComponent<DamageBarrel> ().barrelHit ();
		if(type.Equals("Barrel"))
			barrelAudio.Play ();

		if (health <= 0) {
		
			Network.Destroy (this.gameObject);
			Instantiate(explosion, transform.position, transform.rotation);	
		}
		
	}

	public void setToBodyPart(){
		networkView.RPC ("rpcSetBodyPart", RPCMode.All, 0);
	}

	[RPC]
	void rpcSetTTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}

	[RPC]
	void rpcSetBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
}
