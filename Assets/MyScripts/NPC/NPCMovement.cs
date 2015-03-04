using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour {


	GameObject[] players;
	GameObject playerToChase;
	Rigidbody npc;              							// Reference to the nav mesh agent.
	//float rotationSpeed = 2f;								// NPC rotation speed


	public bool travelsOnX = true;							// Turn off specifically for NPCs which traverse non X directions									
	public bool teleportWaypointCycle = false;				// When cycling waypoints (reaching the end), teleport to the 1st one
	public float patrolSpeed = 3f;                          // The NPC patrol speed.
	public float chaseSpeed = 6f;                           // The NPC chase speed.

	public bool doesItChasePlayers = true;
	public float chaseWaitTime = 8f;                        // The amount of time to wait when the last sighting is reached.
	public int minChaseDistance = 10;						// The distance before the NPC chases after the (nearest) player
	public Transform[] waypoints;                     // An array of transforms for the patrol route.


	private Vector3 direction;								// The direction the NPC is travelling in
	private Quaternion lookRotation;						// The rotation angle the NPC is spinning in


	private float chaseTimer = 3f;                          // A timer for the chaseWaitTime.


	//FSM Build 

	private FSMSystem fsm;
	
	public void SetTransition(Transition t) { fsm.PerformTransition(t); }

	public void Start()
	{
		MakeFSM();
	}
	
	private void MakeFSM()
	{
		Patrolling patrol = new Patrolling(this);
		patrol.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
		
		ChasePlayerState chase = new ChasePlayerState(this);
		chase.AddTransition(Transition.LostPlayer, StateID.Patrolling);
		
		fsm = new FSMSystem();
		fsm.AddState(patrol);
		fsm.AddState(chase);
	}


	void Awake ()
	{
		npc = GetComponent <Rigidbody> ();

		//TODO Move getplayer here after switching to spawning NPC

	}
	


	void FixedUpdate () {


		fsm.CurrentState.Reason();
		fsm.CurrentState.Act();

	}



	public class Patrolling : FSMState
	{
		private NPCMovement npclass;

		private int wayPointIndex;

		public Patrolling(NPCMovement o) 
		{ 
			//access to outer class members
			npclass = o;

			wayPointIndex = 0;
			stateID = StateID.Patrolling;

		}


		public override void Reason()
		{

			// If the Player passes less than 15 meters away in front of the NPC
			RaycastHit hit;
			if (Physics.Raycast(npclass.npc.transform.position, npclass.direction, out hit, 15F))
			{

				if (hit.transform.gameObject.tag == "Player"){
					npclass.playerToChase = hit.transform.gameObject;
					npclass.chaseTimer = 3f;
					npclass.SetTransition(Transition.SawPlayer);
				}
					
			}
		}
		
		public override void Act()
		{
		
			float magnitude = (npclass.npc.transform.position - npclass.waypoints [wayPointIndex].position).magnitude;
			
			//Reached a waypoint, increment waypoint index 
			if (magnitude < 1) 
			{
				wayPointIndex = wayPointIndex == npclass.waypoints.Length - 1 ? 0 : wayPointIndex + 1;
			}
			
			if (npclass.teleportWaypointCycle && wayPointIndex == 0) 
			{
				npclass.TeleportTo(npclass.waypoints [wayPointIndex].transform);
				
			}else
			{
				npclass.MoveTowards (npclass.waypoints [wayPointIndex], npclass.patrolSpeed);
			}
		}
		
	} 



	public class ChasePlayerState : FSMState
	{
		private NPCMovement npclass;

		public ChasePlayerState(NPCMovement o)
		{
			//access to outer class members
			npclass = o;

			stateID = StateID.ChasingPlayer;
		}
		
		public override void Reason()
		{
			// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
			if (Vector3.Distance(npclass.npc.transform.position, npclass.playerToChase.transform.position) >= 30)
				npclass.SetTransition(Transition.LostPlayer);

			// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
			if (npclass.chaseTimer < 0)
				npclass.SetTransition(Transition.LostPlayer);
		}
		
		public override void Act()
		{
			npclass.chaseTimer -= Time.deltaTime;

			npclass.MoveTowards (npclass.playerToChase.transform, npclass.chaseSpeed);
		}
		
	} // ChasePlayerState


	/*
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
	


	void Chasing()
	{
		chaseTimer -= Time.deltaTime;

		MoveTowards (playerToChase.transform, chaseSpeed);

	}*/

	
	//Move towards target position with the rigid body's specified speed
	void MoveTowards(Transform targetTransform, float speed)
	{

		//Move in direction
		direction = (targetTransform.position - npc.position).normalized;
		npc.MovePosition(npc.position + direction * Time.deltaTime * speed );

		//Rotate towards direction over time
		float angleZ = AngleBetweenPoints(npc.position, targetTransform.position);
		float angleX = 0f;

		//Keep things positive!
		//If The Z rotation is 'upside down' then flip X and Z
		if (angleZ > 90 || angleZ < -90 ) 
		{
			angleX = 180;
			angleZ = (-1)*angleZ;
		} 

		if (travelsOnX) {
			npc.rotation = Quaternion.Slerp (npc.rotation, Quaternion.Euler (new Vector3 (angleX, 0f, angleZ)), Time.deltaTime * speed * 2);
		}

	}

	void TeleportTo(Transform targetTransform)
	{
		npc.transform.position = targetTransform.position;
	}


	float AngleBetweenPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

}
