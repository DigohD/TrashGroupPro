﻿using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour {


	GameObject[] players;
	GameObject targetPlayer;								// The current targetted player, ie to chase or run away from
	Rigidbody npc;              							// Reference to the nav mesh agent.
	//float rotationSpeed = 2f;								// NPC rotation speed


	public bool travelsOnX = true;							// Turn off specifically for NPCs which traverse non X directions									
	public bool teleportWaypointCycle = false;				// When cycling waypoints (reaching the end), teleport to the 1st one
	public float patrolSpeed = 3f;                          // The NPC patrol speed.
	public float chaseSpeed = 6f;                           // The NPC chase speed.

	public float chaseWaitTime = 8f;                        // The amount of time to wait when the last sighting is reached.
	public int minChaseDistance = 10;						// The distance before the NPC chases after the (nearest) player
	public Transform[] waypoints;                     		// An array of transforms for the patrol route.


	private Vector3 direction;								// The direction the NPC is travelling in
	private Quaternion lookRotation;						// The rotation angle the NPC is spinning in


	private float chaseTimer = 3f;                          // A timer for the chaseWaitTime.


	//create some randomness in the movement of NPCs
	private float randomTimer = 3f;
	private float randomVelocityMultiplier = 1f;
	private Vector3 randomOffsetDirection;

	public enum npcTypes
	{
		standard,
	 	aggressive,
		shy,
		ignoring

	}

	public npcTypes npcType = npcTypes.standard;			//Allow a number of differnt types of NPCs

	//FSM Build 

	private FSMSystem fsm;
	
	public void SetTransition(Transition t) { fsm.PerformTransition(t); }

	public void Start()
	{
		if(!networkView.isMine)
			return;

		MakeFSM();
		SetWaypoints ();

	}
	

	private void MakeFSM()
	{

		Patrolling patrol = new Patrolling(this);
		ChasePlayerState chase = new ChasePlayerState(this);
		RunAwayFromPlayer run = new RunAwayFromPlayer(this);

		fsm = new FSMSystem();


		//Set up transitions for different types of NPCs
		switch (npcType)
		{
			case npcTypes.standard:
				patrol.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
				patrol.AddTransition(Transition.ScaredOfPlayer, StateID.Running);
				chase.AddTransition(Transition.LostPlayer, StateID.Patrolling);
				break;
			case npcTypes.aggressive:
				patrol.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
				patrol.AddTransition(Transition.ScaredOfPlayer, StateID.Running);
				chase.AddTransition(Transition.LostPlayer, StateID.Patrolling);
				chase.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
				break;
			case npcTypes.shy:
				patrol.AddTransition(Transition.ScaredOfPlayer, StateID.Running);
				chase.AddTransition(Transition.LostPlayer, StateID.Patrolling);
				run.AddTransition(Transition.LostPlayer, StateID.Patrolling);

				break;
			case npcTypes.ignoring:
				//Ignoring never goes into any states!
				break;
			default:
				
				break;
		}


		fsm.AddState(patrol);
		fsm.AddState(chase);
		fsm.AddState(run); 
		
		
	}


	void Awake ()
	{
		npc = GetComponent <Rigidbody> ();
	}
	


	void FixedUpdate () {
		if(!networkView.isMine)
			return;
		fsm.CurrentState.Reason();
		fsm.CurrentState.Act();

	}


	
	//Patrolling State
	//--------------------------------------------------
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

			npclass.checkForPlayersInSight ();

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
				npclass.Move (npclass.waypoints [wayPointIndex], npclass.patrolSpeed, true);
			}
		}
		
	} 


	//Chase Player State
	//--------------------------------------------------
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
			if (Vector3.Distance(npclass.npc.transform.position, npclass.targetPlayer.transform.position) >= 30)
				npclass.SetTransition(Transition.LostPlayer);

			// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
			if (npclass.chaseTimer < 0)
				npclass.SetTransition(Transition.LostPlayer);

			//Agressive NPCs can continue chasing
			npclass.checkForPlayersInSight ();
		}
		
		public override void Act()
		{
			npclass.chaseTimer -= Time.deltaTime;

			npclass.Move (npclass.targetPlayer.transform, npclass.chaseSpeed, true);
		}
		
	} // ChasePlayerState



	
	//Run Away from Player State
	//--------------------------------------------------
	public class RunAwayFromPlayer : FSMState
	{
		private NPCMovement npclass;
		
		public RunAwayFromPlayer(NPCMovement o)
		{
			//access to outer class members
			npclass = o;
			
			stateID = StateID.Running;
		}
		
		public override void Reason()
		{

			// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
			if (npclass.chaseTimer < 0)
				npclass.SetTransition(Transition.LostPlayer);
		}
		
		public override void Act()
		{
			npclass.chaseTimer -= Time.deltaTime;
			
			npclass.Move (npclass.targetPlayer.transform, npclass.chaseSpeed, false);
		}
		
	} 



	
	//Helped Functions
	//--------------------------------------------------

	void checkForPlayersInSight()
	{
		RaycastHit hit;
		if (Physics.Raycast(npc.transform.position, direction, out hit, 5f))
		{
			
			if (hit.transform.gameObject.tag == "Player"){
				targetPlayer = hit.transform.gameObject;
				chaseTimer = 3f;
				
				
				if(npcType == npcTypes.shy)
				{
					SetTransition(Transition.ScaredOfPlayer);
				}
				else
				{
					SetTransition(Transition.SawPlayer);
				}
				
			}
			
		}
	}

	//Move towards or away from target position with the rigid body's specified speed
	void Move(Transform targetTransform, float speed, bool towards)
	{

		float angleZ = 0f;
		float angleX = 0f;


		//Move in specified direction 
		if (towards) 
		{
			direction = (targetTransform.position - npc.position).normalized;
			angleZ = AngleBetweenPoints(npc.position, targetTransform.position);

		}
		else
		{
			direction = (npc.position - targetTransform.position).normalized;
			angleZ = AngleBetweenPoints(targetTransform.position, npc.position);

		}

		//Add some randomness to movement direction and movement speed
		if (randomTimer < 0 && travelsOnX) 
		{
			randomOffsetDirection =  new Vector3 (0f, Random.Range (-0.2f,0.2f), Random.Range (-0.2f,0.2f));
		}

		npc.MovePosition(npc.position + (direction + randomOffsetDirection) * Time.deltaTime * speed * randomVelocityMultiplier);



		//Keep things positive!
		//If The Z rotation is 'upside down' then flip X and Z
		if (angleZ > 90 || angleZ < -90 ) 
		{
			angleX = 180;
			angleZ = (-1)*angleZ;
		} 

		if (travelsOnX) {
			//Rotate towards direction over time
			npc.rotation = Quaternion.Slerp (npc.rotation, Quaternion.Euler (new Vector3 (angleX, 0f, angleZ)), Time.deltaTime * speed * 2);
		}

		//Update the velocity multiplier
		setRandomVelocityMultiplier ();

	}


	void TeleportTo(Transform targetTransform)
	{
		npc.transform.position = targetTransform.position;
	}


	float AngleBetweenPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}


	private void SetWaypoints()
	{

		
		//Get waypoint data by looking up the name of the current NPC in the waypoints object
		Transform waypointsObj = GameObject.Find ("Waypoints/" + this.gameObject.name).transform;
		int children = waypointsObj.childCount;
		
		//Set waypoint array size
		waypoints = new Transform[children];
		
		//Add waypoints
		for (int i = 0; i < children; ++i)
			waypoints[i] = waypointsObj.GetChild(i);
		
	}

	private void setRandomVelocityMultiplier()
	{
		if(randomTimer < 0)
		{
			randomTimer = Random.Range(0.5f, 2.0f);
			randomVelocityMultiplier = Random.Range(1.0f, 2.0f);
		}
		
		randomTimer -= Time.deltaTime;
		
	}


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
					targetPlayer = players[i];
					chaseTimer = 3f;	
					break;
				}
				
			}
		}

	}
	

*/

}
