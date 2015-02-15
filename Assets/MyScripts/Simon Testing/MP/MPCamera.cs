using UnityEngine;
using System.Collections;

public class MPCamera : MonoBehaviour {

	// Use this for initialization
	void Start(){
		Camera cam = GetComponent<Camera>();
		if(networkView.isMine){
			cam.enabled = true;
		}
		else{
			cam.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
