using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" || other.tag == "BodyPart") {

			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(this.gameObject);
			Destroy (other.gameObject);


		
		}

	}
}
