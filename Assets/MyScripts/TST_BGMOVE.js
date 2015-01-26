#pragma strict

//Author: Simon Eliasson

var scaleC : float = 2f;

function Update () {
	gameObject.transform.localScale = Vector3(scaleC, scaleC, scaleC);
	scaleC = scaleC -0.005f;
	
	if(scaleC < 0.001f){
		Destroy(gameObject);
	}
}