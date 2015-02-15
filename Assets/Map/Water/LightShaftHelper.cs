using UnityEngine;
using System.Collections;

public class LightShaftHelper : MonoBehaviour {

	public Color diffuseColor;

	// Update is called once per frame
	void Update () {
		renderer.sharedMaterial.SetColor ("_Color", diffuseColor);
	}
}
