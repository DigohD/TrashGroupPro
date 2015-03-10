using UnityEngine;
using System.Collections;

public class GrenadeGrav : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (!networkView.isMine)
			return;

	}
}
