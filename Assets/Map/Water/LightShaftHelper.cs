using UnityEngine;
using System.Collections;

public class LightShaftHelper : MonoBehaviour {

	public Color diffuseColor;

	public Camera camera;

	private Transform transform;

	void Start()
	{
		transform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () {
		renderer.sharedMaterial.SetColor ("_Color", diffuseColor);
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(camera.transform.position.x,
		                                 transform.position.y,
		                                 transform.position.z);

	}
}