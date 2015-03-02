using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChildList : MonoBehaviour {

	public List<NetworkViewID> childList;

	void Start(){
		childList = new List<NetworkViewID>();
	}

	public void addChild(NetworkViewID id){
		networkView.RPC("rpcAddChildTrash", RPCMode.All, id);
	}

	public List<NetworkViewID> get(){
		Debug.Log ("returning childlist: " + childList.Count + " name" + childList);
		return childList;
	}

	[RPC]
	void rpcAddChildTrash(NetworkViewID id){
		childList.Add (id);
	}
}
