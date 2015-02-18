using UnityEngine;
using System.Collections;

public class shotOutOfBounds : MonoBehaviour {
	void OnTriggerExit( Collider other){
		if (other.networkView.isMine && other.tag == "Gunshot"){
			Destroy(other.gameObject);
		}
	}
}
