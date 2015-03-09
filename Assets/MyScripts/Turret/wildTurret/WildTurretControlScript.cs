using UnityEngine;
using System.Collections;

public class WildTurretControlScript : MonoBehaviour {
	
	//Control this from the inputscript, attach to turret-top
	public string towerType;
	
	private Vector3 newDir;
	private float step;
	public bool active;
	
	public void Update(){//just rotate towards mouse..
		// If turret is not owned by the local player, return
		if(networkView == null)
			return;

		if(active){
			//Mouse Position in the world. It's important to give it some distance from the camera. 
			//If the screen point is calculated right from the exact position of the camera, then it will
			//just return the exact same position as the camera, which is no good.
			Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
		
			//Angle between mouse and this object
			float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
		
		
			transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle + 90));
		
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
		}	
	}

	public void activate(){
		active = true;
	}
	
	float AngleBetweenPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
	
	
	
	
	
}
