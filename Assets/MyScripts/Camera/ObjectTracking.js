#pragma strict

// The game object to follow
var tracked : GameObject;
// The follow "speed", less acceleration leads to elastic follow movement
var acceleration : float = 0.05f;
// The distance from the camera to the object
var baseDistance : float = 10f;
// The amount the camera zooms out due to speed of the object
var zoomAmount : float = 25f;

private var velocity = Vector2(0, 0);

function Start()
{
	//Initialize the camera centered on the tracked object
	gameObject.transform.position = Vector3(
			tracked.transform.position.x,
			tracked.transform.position.y,
			tracked.transform.position.z - baseDistance);
}

private var myPos : Vector3;
private var targetPos : Vector3;
private var extraDistance : float;

function FixedUpdate()
{
	if(tracked != null){
	// Position of the camera
	myPos = gameObject.transform.position;
	
	// Position that the camera moves towards
	targetPos = Vector3(
			tracked.transform.position.x,
			tracked.transform.position.y,
			tracked.transform.position.z - baseDistance);
	
	// The higher the distance between myPos and targetPos, the higher the speed
	velocity = Vector3(targetPos.x - myPos.x, targetPos.y - myPos.y, targetPos.z - myPos.y) * acceleration;
	
	// Zoom out amount, depending on speed
	extraDistance = velocity.magnitude * zoomAmount;
	
	// Set the new camera position by adding velocity
	gameObject.transform.position = gameObject.transform.position + velocity;
	
	// Set depth of Camera, adding extra distance
	gameObject.transform.position.z = tracked.transform.position.z - baseDistance - extraDistance;
	}
	
}