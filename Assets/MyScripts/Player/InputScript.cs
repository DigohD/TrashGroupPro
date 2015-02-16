using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour {

	public MovementScript ms;
	public PlayerStats stats;

	private float magnetDelay = 3;
	

	void Update () {
		if(!networkView.isMine)
			return;

		if (Input.GetKey ("w"))
			ms.MoveUp ();
		else if (Input.GetKey ("s"))
			ms.MoveDown ();
	
		if (Input.GetKey ("d"))
			ms.MoveRight ();
		else if (Input.GetKey ("a"))
			ms.MoveLeft ();

		
		if (Input.GetKey ("e"))
			ms.RotateRight ();
		else if (Input.GetKey ("q"))
			ms.RotateLeft ();

		/*if (Input.GetMouseButtonDown (0)){//fire all turrets when mouse button is pressed
				if (stats.goList.Count > 0)
						foreach (GameObject go in stats.goList)
								go.GetComponent<TurretControlScript> ().fire ();
		}*/

		if (Input.GetKey ("left ctrl")&& magnetDelay >2){//disable on magneteffect as well as the ability to pile on new trash
			print ("hollaa");
			stats.magnetSwitch ();
			magnetDelay = 0;
	    }
		magnetDelay += Time.deltaTime;

		//Turretcontrol



	}
}
