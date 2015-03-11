using UnityEngine;
using System.Collections;


//Patrolling State
//--------------------------------------------------
public class StatePatrolling : FSMState
{
	private NPCAI npcClass;
	
	private int wayPointIndex;
	
	public StatePatrolling(NPCAI o) 
	{ 
		//access to outer class members
		npcClass = o;
		
		wayPointIndex = 0;
		stateID = StateID.Patrol;
		
	}
	
	
	public override void Reason()
	{
	
		if(npcClass.playerInSight ())
			npcClass.SetTransition(Transition.SawPlayer);

		if(npcClass.nearbyPlayers ())
			npcClass.SetTransition(Transition.SmellPlayer);

		
	}
	
	public override void Act()
	{
		
		float magnitude = (npcClass.getNPC().transform.position - npcClass.waypoints [wayPointIndex].position).magnitude;
		
		//Reached a waypoint, increment waypoint index 
		if (magnitude < 1) 
		{
			wayPointIndex = wayPointIndex == npcClass.waypoints.Length - 1 ? 0 : wayPointIndex + 1;
		}
		
		if (npcClass.teleportWaypointCycle && wayPointIndex == 0) 
		{
			npcClass.TeleportTo(npcClass.waypoints [wayPointIndex].transform);
			
		}else
		{
			npcClass.Move (npcClass.waypoints [wayPointIndex], 1, true);
		}
	}
	
} 