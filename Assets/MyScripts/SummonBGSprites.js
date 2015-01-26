#pragma strict

//Author: Simon Eliasson

public var BGSprite : GameObject;

function Update(){

	if(Random.Range(0, 100) < 2){
		var rngX : float = Random.Range(-8f, 8f) + gameObject.transform.position.x;
		var rngY : float = Random.Range(-5f, 5f) + gameObject.transform.position.y;
		var pos : Vector3 = Vector3(rngX, rngY, gameObject.transform.position.z);

		var spr : GameObject = Instantiate (BGSprite, pos, Quaternion(0, 0, 0, 0));
 	}
}