﻿using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {

	[RPC]
	void PlayerSetupFunc(NetworkPlayer player){
		if(player == Network.player){
			enabled = true;
		}
		else{
			enabled = false;
		}
		
		ObjectTrack ot;
		ot = (ObjectTrack) Camera.main.GetComponent("ObjectTrack");
		ot.tracked = gameObject;
	}
}
