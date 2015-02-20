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

	void Start ()
	{
		if(type.Equals("Barrel")){
			barrelAudio = GetComponent<AudioSource> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setTaken(string newOwnerID){
		isTaken = true;
		networkView.RPC("rpcTaken", RPCMode.Others, 0);
		ownerID = newOwnerID;
		networkView.RPC("rpcTaken", RPCMode.Others, 0, newOwnerID);
	}

	public void takeDamage ( float incDmg ) {
		health -= incDmg;
		if(type.Equals("Barrel"))
			barrelAudio.Play ();

		if (health <= 0) {
		
			Network.Destroy (this.gameObject);
	
		}
		
	}
	[RPC]
	void rpcTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}
}
