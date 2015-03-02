using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChildList : MonoBehaviour {

	public List<GameObject> childList;

	void Start(){
		childList = new List<GameObject>();
	}

	public void addChild(GameObject go){
		networkView.RPC("rpcAddChildTrash", RPCMode.All, networkView.viewID);
	}

	public List<GameObject> get(){
		Debug.Log ("returning childlist: " + childList.Count + " name" + childList);
		return childList;
	}

	[RPC]
	void rpcAddChildTrash(NetworkViewID id){
		childList.Add (NetworkView.Find(id).gameObject);
	}
}
