using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour {


	GameObject[] players;
	GameObject playerToChase;
	Rigidbody npc;              							// Reference to the nav mesh agent.
	//float rotationSpeed = 2f;								// NPC rotation speed


	public bool teleportWaypointCycle = false;				// When cycling waypoints (reaching the end), teleport to the 1st one
	public float patrolSpeed = 2f;                          // The NPC patrol speed.
	public float chaseSpeed = 5f;                           // The NPC chase speed.
	public bool playerSighted = true;                      // The NPC chase speed.

	public bool doesItChasePlayers = true;
	public float chaseWaitTime = 8f;                        // The amount of time to wait when the last sighting is reached.
	public int minChaseDistance = 10;						// The distance before the NPC chases after the (nearest) player
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.


	private Vector3 direction;								// The direction the NPC is travelling in
	private Quaternion lookRotation;						// The rotation angle the NPC is spinning in


	private float chaseTimer = 3f;                          // A timer for the chaseWaitTime.
	private int wayPointIndex = 0;                          // A counter for the way point array.
	
	void Awake ()
	{
		npc = GetComponent <Rigidbody> ();

		//TODO Move getplayer here after switching to spawning NPC

	}
	


	void FixedUpdate () {


		checkForNearbyPlayers ();

		if(	chaseTimer > 0 && doesItChasePlayers)	
		{
			Chasing();
		}
		else
		{
			Patrolling ();
		}


	}

	void checkForNearbyPlayers()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		
		
		if(players.Length > 0)
		{
			for(int i = 0; i < players.Length; i++){
				
				float distanceToPlayer = (players[i].transform.position - patrolWayPoints[wayPointIndex].transform.position).sqrMagnitude;
				
				if(distanceToPlayer < minChaseDistance)
				{
					playerToChase = players[i];
					chaseTimer = 3f;	
					break;
				}
				
			}
		}

	}

	void Update ()
	{


	} 


	void Chasing()
	{
		chaseTimer -= Time.deltaTime;

		MoveTowards (playerToChase.transform, chaseSpeed);

	}


	void Patrolling()
	{

		float sqrMgn = (npc.position - patrolWayPoints [wayPointIndex].position).sqrMagnitude;

		//Reached a waypoint, increment waypoint index 
		if (sqrMgn < 1) 
		{
			wayPointIndex = wayPointIndex == patrolWayPoints.Length - 1 ? 0 : wayPointIndex + 1;

		}

		if (teleportWaypointCycle && wayPointIndex == 0) 
		{
			npc.transform.position = patrolWayPoints [wayPointIndex].transform.position;
				
		}else
		{
			MoveTowards (patrolWayPoints [wayPointIndex], patrolSpeed);
		}


	}

	//Move towards target position with the rigid body's specified speed
	void MoveTowards(Transform targetTransform, float speed)
	{
		//Move in direction
		direction = (targetTransform.position - npc.position).normalized;
		npc.MovePosition(npc.position + direction * Time.deltaTime * speed);

		//Rotate towards direction over time
		float angleZ = AngleBetweenPoints(npc.position, targetTransform.position);
		float angleX = 0f;

		//If The Z rotation is 'upside down' then flip X and Z
		if (angleZ > 90 && gameObject.tag != "Whales") 
		{
			angleX = 180;
			angleZ = -angleZ;

			npc.rotation = Quaternion.Slerp (npc.rotation, Quaternion.Euler (new Vector3 (angleX, 0f, angleZ)), Time.deltaTime * speed);
		} 


	}


	float AngleBetweenPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

}
