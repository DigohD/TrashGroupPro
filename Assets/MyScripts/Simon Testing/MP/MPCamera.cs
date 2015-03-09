using UnityEngine;
using System.Collections;

public class MPCamera : MonoBehaviour {

	// Use this for initialization, make sure I control the camera, not the other clients
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
