using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

	public PlayerStats stats;
	//public bool isOn;
	public GameObject position;
	public float radius;
	public GameObject particles;
	public ParticleSystem ps;

	// Turn on the magnet visual effects for this player object on all clients, using RPC
	public void turnOn(){
		networkView.RPC("rpcTurnPSOn", RPCMode.All, 0);
	}

	// Turn off the magnet visual effects for this player object on all clients, using RPC
	public void turnOff(){
		networkView.RPC("rpcTurnPSOff", RPCMode.All, 0);
	}

	void FixedUpdate () {
		// If this is the local clients magnet, and the magnet is turned on
		if (networkView.isMine && stats.magnetOn) 
		{
			// Find trash in magnet vicinity
			Collider[] magnetTrash = Physics.OverlapSphere (position.transform.position, radius);
	
			// For every piece of trash within vicinity
			for (int i = 0; i < magnetTrash.Length; i++) {
				// Do this with trash inside the danger zone
				if (magnetTrash [i].tag == "Trash" ) {	
					// If the trash is not already owned by a player
					if(!magnetTrash[i].GetComponent<TrashStats>().isTaken){
						// Drag the trash towards the player object
						Transform trash = magnetTrash[i].transform;
						float dist = Vector3.Distance(position.transform.position, trash.position);
						float vel = radius/ dist;				//make speed proportional to how far away the trash is
						trash.position = Vector3.MoveTowards(trash.position, position.transform.position, vel * Time.deltaTime);
					}

				// magnetTrash [i].rigidbody.velocity = new Vector3 (0, 0, 0);
				}
				// Do the same procedure if the trash is a turret
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

	[RPC]
	void rpcTurnPSOn(int wasted){
		ps.Play();
	}

	[RPC]
	void rpcTurnPSOff(int wasted){
		ps.Stop();
		ps.Clear();
	}
}
