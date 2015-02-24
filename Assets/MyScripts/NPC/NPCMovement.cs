using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour {


	GameObject[] players;
	Rigidbody rigidBody;              						// Reference to the nav mesh agent.
	float speed = 4f;										// NPC movement speed
	float rotationSpeed = 8f;								// NPC rotation speed

	public float patrolSpeed = 2f;                          // The NPC patrol speed.
	public float chaseSpeed = 5f;                           // The NPC chase speed.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.


	private Vector3 direction;								// The direction the NPC is travelling in
	private Quaternion lookRotation;						// The rotation angle the NPC is spinning in


	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex = 0;                          // A counter for the way point array.
	
	
	void Awake ()
	{
		rigidBody = GetComponent <Rigidbody> ();

		//TODO Move getplayer here after switching to spawning NPC

	}
	


	void FixedUpdate () {


	}


	void Update ()
	{

		/*// If the player is in sight and is alive...
		if(enemySight.playerInSight && playerHealth.health > 0f)
			// ... shoot.
			Shooting();
		
		// If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
			// ... chase.
			Chasing();
		
		// Otherwise...
		else
			// ... patrol.
			Patrolling();
	

		/*
		// Set up the references.
		players = GameObject.FindGameObjectsWithTag ("Player");

		if(players.Length > 0)
		for(int i = 0; i < players.Length; i++){
			if(players[i].networkView.isMine)
				nav.SetDestination (players[i].transform.position);
		}

	*/

			Patrolling ();

	} 


	void Patrolling()
	{

		float sqrMgn = (rigidBody.position - patrolWayPoints [wayPointIndex].position).sqrMagnitude;

		//Reached a waypoint, increment waypoint index 
		if (sqrMgn < 1) 
		{
			wayPointIndex = wayPointIndex == patrolWayPoints.Length - 1 ? 0 : wayPointIndex + 1;

		}


		MoveTowards (patrolWayPoints [wayPointIndex]);

	}

	//Move towards target position with the rigid body's specified speed
	void MoveTowards(Transform targetTransform)
	{
		//Move in direction
		direction = (targetTransform.position - rigidBody.position).normalized;
		rigidBody.MovePosition(rigidBody.position + direction * Time.deltaTime  * speed);

		//Rotate towards direction over time
		lookRotation = Quaternion.LookRotation(direction);

		//print (lookRotation);
		rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation, lookRotation, Time.deltaTime * rotationSpeed);


	}
}
