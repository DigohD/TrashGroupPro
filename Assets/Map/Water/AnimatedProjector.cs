using UnityEngine;
using System.Collections;

public class AnimatedProjector : MonoBehaviour {

	public float fps = 30;
	public Texture2D[] frames;
	// Height above the player
	public float baseHeight;

	// The game object to follow
	public GameObject player;

	private int frameIndex;
	private Projector projector;

	void Start()
	{
		projector = GetComponent<Projector>();
		NextFrame();
		InvokeRepeating ("NextFrame", 1 / fps, 1 / fps);
	}

	void NextFrame()
	{
		projector.material.SetTexture ("_ShadowTex", frames [frameIndex]);
		frameIndex = (frameIndex + 1) % frames.Length;
	}

	void FixedUpdate()
	{
		projector.transform.position = new Vector3 (player.transform.position.x, 
		                                            player.transform.position.y + baseHeight, 
		                                            player.transform.position.z + 50);

		// Set the farplane to so that the projector can reach the sea bed
		projector.farClipPlane = projector.transform.position.y + 20;
	}
}
