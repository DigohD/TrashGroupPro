using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour {

	public MovementScript ms;
	

	void Update () {
		if (Input.GetKey ("up"))
						ms.MoveUp ();
		else if (Input.GetKey ("down"))
			ms.MoveDown ();
	
		if (Input.GetKey ("right"))
			ms.MoveRight ();
		else if (Input.GetKey ("left"))
			ms.MoveLeft ();

		
		if (Input.GetKey ("e"))
			ms.RotateRight ();
		else if (Input.GetKey ("q"))
			ms.RotateLeft ();
	}
}
