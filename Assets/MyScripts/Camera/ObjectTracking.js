#pragma strict

var tracked : GameObject;
var acceleration : float = 0.05f;

private var velocity = Vector2(0, 0);

function Start()
{
	gameObject.transform.position = Vector3(
	tracked.transform.position.x,
	tracked.transform.position.y,
	tracked.transform.position.z - 10
	);
}

function Update()
{

	/*gameObject.transform.position = Vector3(
	tracked.transform.position.x,
	tracked.transform.position.y,
	tracked.transform.position.z - 10
	);*/
	var myPos : Vector3 = Vector3(
			gameObject.transform.position.x,
			gameObject.transform.position.y,
			gameObject.transform.position.z);
	var targetPos : Vector3 = Vector3(
			tracked.transform.position.x,
			tracked.transform.position.y,
			tracked.transform.position.z - 10);
	
	//var distance : float = (tracked.transform.position.x - gameObject.transform.position.x) +
	//		(tracked.transform.position.y - gameObject.transform.position.y);
	
	velocity = Vector3(targetPos.x - myPos.x, targetPos.y - myPos.y, targetPos.z - myPos.y) * acceleration;
	
	gameObject.transform.position = gameObject.transform.position + velocity;
	
}