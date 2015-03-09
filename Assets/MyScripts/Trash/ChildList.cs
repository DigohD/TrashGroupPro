using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChildList : MonoBehaviour {

	// List of children as networkViewIDs
	public List<NetworkViewID> childList;

	void Start(){
		childList = new List<NetworkViewID>();
	}

	// Add child of this object. Make sure all connected clients get an update in their lists.
	public void addChild(NetworkViewID id){
		networkView.RPC("rpcAddChildTrash", RPCMode.All, id);
	}

	// Remove child of this object. Make sure all connected clients get an update in their lists.
	public void removeChild(NetworkViewID id){
		networkView.RPC("rpcRemoveChildTrash", RPCMode.All, id);
	}

	public List<NetworkViewID> get(){
		return childList;
	}

	// Used to synchrnize added children over the network
	[RPC]
	void rpcAddChildTrash(NetworkViewID id){
		childList.Add(id);
	}

	// Used to synchrnize removed children over the network
	[RPC]
	void rpcRemoveChildTrash(NetworkViewID id){
		childList.Remove(id);
	}
}
