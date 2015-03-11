using UnityEngine;
using System.Collections;


public class NPCAI : MonoBehaviour {


	public behaviours behaviour = behaviours.ignoring;		//Allow a number of differnt types of AIs
	public float baseSpeed = 3f;                          	// The NPC patrol speed.
	public Transform[] waypoints;                     		// An array of transforms for the patrol route.
	public bool travelsOnX = true;							// Turn off specifically for NPCs which traverse non X directions									
	public bool teleportWaypointCycle = false;				// When cycling waypoints (reaching the end), teleport to the 1st one


	private GameObject[] players;							// All players on the map
	private GameObject targetPlayer;						// The current targetted player, ie to chase or run away from
	private Rigidbody npc;              					// Reference to the player .


	private Vector3 direction;								// The direction the NPC is travelling in
	private Quaternion lookRotation;						// The rotation angle the NPC is spinning in


	private Vector3[] searchDirections = new Vector3[5];     // An array of transforms for the patrol route.


	//create some randomness in the movement of NPCs
	private float randomTimer = 3f;
	private float randomVelocityMultiplier = 1f;
	private Vector3 randomOffsetDirection;

	public enum behaviours
	{
	 	aggressive,
		ambush,
		whimsical,
		feigning,
		ignoring
	}



	//FSM 
	private FSMSystem fsm;
	private StatePatrolling patrol;
	private StateChasePlayer chase;
	private StateRunAwayFromPlayer run;
	private StateFollow follow;
	private StateScatter scatter;
	private StateFeign feign;

	public void SetTransition(Transition t) { fsm.PerformTransition(t); }

	public void Start()
	{
		if(!networkView.isMine)
			return;

		MakeFSM();
		SetWaypoints();

		//Scatter every 20 seconds after 30 seconds
		InvokeRepeating("Scatter", 30, 20);

	}
	


	private void MakeFSM()
	{

		patrol = new StatePatrolling(this);
		chase = new StateChasePlayer(this);
		run = new StateRunAwayFromPlayer(this);
		follow = new StateFollow(this);
		scatter = new StateScatter (this);
		feign = new StateFeign (this);

		fsm = new FSMSystem();
	
		//get back to patrolling after the scatter event
		scatter.AddTransition(Transition.LostPlayer, StateID.Patrol);

		//Scatter always triggers scatter state for all states
		patrol.AddTransition(Transition.Scatter, StateID.Scatter);
		chase.AddTransition(Transition.Scatter, StateID.Scatter);
		run.AddTransition(Transition.Scatter, StateID.Scatter);
		follow.AddTransition(Transition.Scatter, StateID.Scatter);


		//Running always resets back to patrol
		run.AddTransition(Transition.LostPlayer, StateID.Patrol);

		//Chasing always scatters after it's done
		chase.AddTransition(Transition.LostPlayer, StateID.Scatter);

		//Set up transitions for different types of NPCs
		switch (behaviour)
		{

			case behaviours.aggressive:
				patrol.AddTransition(Transition.SawPlayer, StateID.Chase);
				patrol.AddTransition(Transition.SmellPlayer, StateID.Patrol);
				break;
			case behaviours.ambush:
				patrol.AddTransition(Transition.SawPlayer, StateID.Patrol);
				patrol.AddTransition(Transition.SmellPlayer, StateID.Follow);
				follow.AddTransition(Transition.LostPlayer, StateID.Patrol);
				follow.AddTransition(Transition.SawPlayer, StateID.Chase);
				break;
			case behaviours.whimsical:
				patrol.AddTransition(Transition.SawPlayer, StateID.Chase);
				patrol.AddTransition(Transition.SmellPlayer, StateID.Run);
				run.AddTransition(Transition.SawPlayer, StateID.Chase);
				break;
			case behaviours.feigning:
				patrol.AddTransition(Transition.SawPlayer, StateID.Patrol);
				patrol.AddTransition(Transition.SmellPlayer, StateID.Feign);
				feign.AddTransition(Transition.LostPlayer, StateID.Patrol);
				feign.AddTransition(Transition.ScaredOfPlayer, StateID.Run);
				feign.AddTransition(Transition.SawPlayer, StateID.Chase);
				break;
			case behaviours.ignoring:
				//Ignoring never goes into any states!
				patrol.AddTransition(Transition.SmellPlayer, StateID.Patrol);
				patrol.AddTransition(Transition.SawPlayer, StateID.Patrol);
				break;
			default:
				
				break;
		}

		//Don't forget to add the state! Or else you lose 5 hours debugging nonsense.
		fsm.AddState(patrol);
		fsm.AddState(chase);
		fsm.AddState(run); 
		fsm.AddState(follow); 
		fsm.AddState(scatter); 
		fsm.AddState(feign);
		
		
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

		//if(npc.gameObject.name == "Clyde")
		//print (fsm.CurrentState.ID);

	}


	



	//Helped Functions
	//--------------------------------------------------


	public bool nearbyPlayers()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		
		
		if(players.Length > 0)
		{
			for(int i = 0; i < players.Length; i++){


				float distanceToPlayer = Vector3.Distance(npc.transform.position, players[i].transform.position);
				
				if(distanceToPlayer < 5f)
				{

					targetPlayer = players[i];

					//print (npc.gameObject.name + "smelled" + targetPlayer.gameObject.name);

					return true;
				}
				
			}
		}

		return false;

	}
	
	public bool playerInSight()
	{

		
		RaycastHit hit = new RaycastHit();
		bool isHit = false;

		//if(this.gameObject.name == "Blowfish")
		//print (direction + new Vector3(0f,0.1f,0f);

		//Simulate a sort of field of view
		searchDirections[0] = direction;
		searchDirections[1] = direction +  new Vector3(0f,0.05f,0f);
		searchDirections[2] = direction +  new Vector3(0f,0.1f,0f);
		searchDirections[3] = direction -  new Vector3(0f,0.05f,0f);
		searchDirections[4] = direction -  new Vector3(0f,0.1f,0f);


		// Search for hits
		for (int i = 0; i < searchDirections.Length; i++)
		{
			if (Physics.Raycast(npc.transform.position, searchDirections[i], out hit, 3f))
			{
				if (hit.transform.gameObject.tag == "Player")
				{
					isHit = true;
					break;
				}
			}
		}


		//We've hit a player - trigger transitions
		if (isHit)
		{
			
			targetPlayer = hit.transform.gameObject;
			return true;

		}

		return false;

	}

	//Move towards or away from target position with the rigid body's specified speed
	public void Move(Transform targetTransform, float speedMultiplier, bool towards)
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
			randomOffsetDirection =  new Vector3 (0f, Random.Range (-0.2f,0.2f), 0f);
		}

		npc.MovePosition(npc.position + (direction + randomOffsetDirection) * Time.deltaTime * baseSpeed *speedMultiplier * randomVelocityMultiplier);



		//Keep things positive!
		//If The Z rotation is 'upside down' then flip X and Z
		if (angleZ > 90 || angleZ < -90 ) 
		{
			angleX = 180;
			angleZ = (-1)*angleZ;
		} 

		if (travelsOnX) {
			//Rotate towards direction over time
			npc.rotation = Quaternion.Slerp (npc.rotation, Quaternion.Euler (new Vector3 (angleX, 0f, angleZ)), Time.deltaTime * baseSpeed * 5);
		}

		//Update the velocity multiplier
		setRandomVelocityMultiplier ();

	}


	public void TeleportTo(Transform targetTransform)
	{
		npc.transform.position = targetTransform.position;
	}


	private float AngleBetweenPoints(Vector2 a, Vector2 b) {
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


	private void Scatter () 
	{
		if(behaviour.ToString() != "ignoring")
			SetTransition(Transition.Scatter);
	}
	
	//Getter Setter Functions
	//--------------------------------------------------

	public Rigidbody getNPC()
	{
		return this.npc;
	}

	
	public GameObject getTargetPlayer ()
	{
		return this.targetPlayer;
	}

	



}
