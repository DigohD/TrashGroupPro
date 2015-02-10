using UnityEngine;
using System.Collections;

public class shotOutOfBounds : MonoBehaviour {

	void OnTriggerExit( Collider other) {
		print ("jndffndsf");
		if (other.tag == "Gunshot")
						Destroy (other.gameObject);


	}
}
