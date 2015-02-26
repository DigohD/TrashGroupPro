using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

	public PlayerStats stats;
	//public bool isOn;
	public GameObject position;
	public float radius;
	public GameObject particles;
	public ParticleSystem ps;

	public void turnOn(){
		ps.Play ();
		print ("turnon");
	}
	public void turnOff(){
		ps.Stop ();
		ps.Clear ();
	}

	void FixedUpdate () {
		if (networkView.isMine && stats.magnetOn) 
		{
			Collider[] magnetTrash = Physics.OverlapSphere (position.transform.position, radius);
	
			for (int i = 0; i < magnetTrash.Length; i++) {
		
					if (magnetTrash [i].tag == "Trash" ) {	//Do this when trash is inside the danger zone
					if(!magnetTrash[i].GetComponent<TrashStats>().isTaken){
						Transform trash = magnetTrash[i].transform;
						float dist = Vector3.Distance(position.transform.position, trash.position);
						float vel = radius/ dist;				//make speed proportional to how far away the trash is
						trash.position = Vector3.MoveTowards(trash.position, position.transform.position, vel * Time.deltaTime);
					}

					//		magnetTrash [i].rigidbody.velocity = new Vector3 (0, 0, 0);
					}
				else if (magnetTrash [i].tag == "PassiveTurret" ) {
					if(!magnetTrash[i].GetComponent<TurretStats>().isTaken){

						float dist = Vector3.Distance(position.transform.position, magnetTrash[i].transform.position);
						float vel = radius/ dist;				//make speed proportional to how far away the trash is
						magnetTrash[i].transform.position = Vector3.MoveTowards(magnetTrash[i].transform.position, position.transform.position, vel * Time.deltaTime);
			
					}
				}
		}
	}
}
}
