using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {


	GameObject[] players;
	NavMeshAgent nav;               // Reference to the nav mesh agent.
	

	void Awake ()
	{
		// Set up the references.
		players = GameObject.FindGameObjectsWithTag ("Player");
		nav = GetComponent <NavMeshAgent> ();
	}
	
	
	void Update ()
	{


		if(players.Length > 0)
		for(int i = 0; i < players.Length; i++){
			if(players[i].networkView.isMine)
				nav.SetDestination (players[i].transform.position);
		}





	} 
}
