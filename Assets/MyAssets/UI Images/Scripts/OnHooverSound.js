#pragma strict

var hoover : AudioClip;
var rectangle : Rect;
private var played : boolean;

function Start()
{
	rectangle = Rect(gameObject.transform.position.x, 
			gameObject.transform.position.y,
			422 * 0.5f,
			77 * 0.5f);
}

function Update () 
{
	if(!played && rectangle.Contains(Input.mousePosition)){
		AudioSource.PlayClipAtPoint(hoover, transform.position);
		played = true;
	}else if(!rectangle.Contains(Input.mousePosition)){
		played = false;
	}
}