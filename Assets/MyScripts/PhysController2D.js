#pragma strict

//Author: Simon Eliasson

@script RequireComponent(Rigidbody)

var acceleration : double = 0.1;
var inertia : double = 1.5;
var rigidBody : Rigidbody;

private var velocity : Vector3 = Vector3(0, 0, 0);

private var upPressed : boolean = false;
private var downPressed : boolean = false;
private var leftPressed : boolean = false;
private var rightPressed : boolean = false;

function Update () {
	if(Input.GetKeyDown(KeyCode.A))
		leftPressed = true;
	if(Input.GetKeyDown(KeyCode.D))
		rightPressed = true;
	if(Input.GetKeyDown(KeyCode.W))
		upPressed = true;
	if(Input.GetKeyDown(KeyCode.S))
		downPressed = true;
		
	if(Input.GetKeyUp(KeyCode.A))
		leftPressed = false;
	if(Input.GetKeyUp(KeyCode.D))
		rightPressed = false;
	if(Input.GetKeyUp(KeyCode.W))
		upPressed = false;
	if(Input.GetKeyUp(KeyCode.S))
		downPressed = false;
	
	if(leftPressed)
		velocity.x = velocity.x - acceleration;
	if(rightPressed)
		velocity.x = velocity.x + acceleration;
	if(upPressed)	
		velocity.y = velocity.y + acceleration;
	if(downPressed)
		velocity.y = velocity.y - acceleration;
	
	velocity = velocity / inertia;
	
	// Move the controller
	rigidBody = GetComponent(Rigidbody);
	rigidBody.AddForce(velocity);
	
	velocity = Vector3.zero;
}

function GetVelocity() : Vector3{
    return velocity;
}

function stopMove(){
	velocity = Vector3.zero;
}