#pragma strict

//Author: Simon Eliasson

@script RequireComponent(PhysController2D)

public var Sparks1 : GameObject;

function OnCollisionEnter (col : Collision){
	if(col.gameObject.name == "Capsule"){
		//var exp : GameObject = Instantiate (Sparks1, col.gameObject.transform.position, col.gameObject.transform.rotation);
 		//exp.particleEmitter.Emit();
  		//Destroy (exp, 2);
 		
        //Destroy(col.gameObject);
    }
    
    
	
	Debug.Log("COLLIDE!!!!!!");
}