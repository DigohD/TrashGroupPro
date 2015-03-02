using UnityEngine;
using System.Collections;

public class ObjectTrack : MonoBehaviour {

	// The game object to follow
	public GameObject tracked;
	// The follow "speed", less acceleration leads to elastic follow movement
	float acceleration = 0.05f;
	// The distance from the camera to the object
	public float baseDistance;
	// The amount the camera zooms out due to speed of the object
	float zoomAmount = 30f;

	public GameObject roof;

	public GameObject floor;

	public float offset;
	
	private Vector3 velocity = new Vector3(0, 0, 0);
	
	void Start()
	{
		//Initialize the camera centered on the tracked object
		gameObject.transform.position = new Vector3(
			tracked.transform.position.x,
			tracked.transform.position.y,
			tracked.transform.position.z - baseDistance);
	}
	
	private Vector3 myPos;
	private Vector3 targetPos;
	private float extraDistance;
	
	void FixedUpdate()
	{
		if(tracked != null)
		{
			// Position of the camera
			myPos = gameObject.transform.position;
			
			// Position that the camera moves towards
			targetPos = new Vector3(
				tracked.transform.position.x,
				tracked.transform.position.y,
				tracked.transform.position.z - baseDistance);
			
			// The higher the distance between myPos and targetPos, the higher the speed
			velocity = new Vector3(targetPos.x - myPos.x, targetPos.y - myPos.y, 0) * acceleration;

			// Zoom out amount, depending on speed
			extraDistance = Mathf.Min(velocity.magnitude * zoomAmount, zoomAmount/7);
			
			// Set the new camera position by adding velocity
			gameObject.transform.position = (gameObject.transform.position + velocity);

			if((roof.transform.position.y - offset) <= gameObject.transform.position.y)
			{
				gameObject.transform.position = new Vector3(
					gameObject.transform.position.x,
					roof.transform.position.y - offset,
					tracked.transform.position.z - baseDistance - extraDistance);
			} 
			else if((floor.transform.position.y + offset) >= gameObject.transform.position.y)
			{
			// Set depth of Camera, adding extra distance
			gameObject.transform.position = new Vector3(
				gameObject.transform.position.x,
				floor.transform.position.y + offset,
				tracked.transform.position.z - baseDistance - extraDistance);
			} 
			else
			{
				// Set depth of Camera, adding extra distance
				gameObject.transform.position = new Vector3(
					gameObject.transform.position.x,
					gameObject.transform.position.y,
					tracked.transform.position.z - baseDistance - extraDistance);
			}
			}
	}
}
