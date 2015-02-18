using UnityEngine;
using System.Collections;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
	public float health;
	//public float scale;
	public float speed;
	public float boost;
	public bool isTaken;

	public string ownerID;

	void start ()
	{
		isTaken = false;
		//health = 10;
		//speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (health == 0)//This will be implemented better in the future
						Destroy (this);
	}

	public void setTaken(string newOwnerID){
		isTaken = true;
		networkView.RPC("rpcTaken", RPCMode.Others, 0);
		ownerID = newOwnerID;
		networkView.RPC("rpcTaken", RPCMode.Others, 0, newOwnerID);
	}
	
	[RPC]
	void rpcTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}
}
