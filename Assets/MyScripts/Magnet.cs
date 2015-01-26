using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {


	public bool isOn;
	public GameObject position;
	public float radius;


	void FixedUpdate () {
		if (isOn) 
		{
			Collider[] magnetTrash = Physics.OverlapSphere (position.transform.position, radius);
	
			for (int i = 0; i < magnetTrash.Length; i++) {
		
					if (magnetTrash [i].tag == "Trash") {	//Do this when trash is inside the danger zone
					Transform trash = magnetTrash[i].transform;
					float dist = Vector3.Distance(position.transform.position, trash.position);
					float vel = radius/ dist;				//make speed proportional to how far away the trash is
					trash.position = Vector3.MoveTowards(trash.position, position.transform.position, vel * Time.deltaTime);


					//		magnetTrash [i].rigidbody.velocity = new Vector3 (0, 0, 0);
					}
			}
		}
	}
}
