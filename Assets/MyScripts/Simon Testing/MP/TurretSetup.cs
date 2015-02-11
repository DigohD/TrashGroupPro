using UnityEngine;
using System.Collections;

public class TurretSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(gameObject.networkView.isMine){
			enabled = true;
		}
		else{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
