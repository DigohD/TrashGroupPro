#pragma strict

var clickSound : AudioClip;

function Start()
{
	xxxBtn.onClick.AddListener(YourDisposeFunction); 
	playClickSound();
}

function playClickSound(){
	AudioSource.PlayClipAtPoint(clickSound, transform.position);
}
