#pragma strict

@script RequireComponent(Rigidbody)

var rigidBody : Rigidbody;

public static var waterThickness : double = 1.5;

function Update () {
	rigidBody = GetComponent(Rigidbody);
	
	var v : Vector3;
	v = rigidBody.velocity;
	
	var waterResistance : Vector3;
	waterResistance = Vector3(-v.x, -v.y, -v.z) * waterThickness;
	
	rigidBody.angularDrag = waterThickness / 4;
	rigidBody.AddForce(waterResistance);
}