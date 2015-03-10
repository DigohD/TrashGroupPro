using UnityEngine;
using System.Collections;


//Chase Player State
//--------------------------------------------------
public class StateChasePlayer : FSMState
{
	private NPCAI npcClass;
	
	public StateChasePlayer(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.ChasingPlayer;
	}
	
	public override void Reason()
	{
		// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
		if (Vector3.Distance(npcClass.getNPC().transform.position, npcClass.getTargetPlayer().transform.position) >= 30)
			npcClass.SetTransition(Transition.LostPlayer);
		
		// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
		if (npcClass.getChaseTimer() < 0)
			npcClass.SetTransition(Transition.LostPlayer);
		
		//Agressive NPCs can continue chasing
		npcClass.checkForPlayersInSight ();
	}
	
	public override void Act()
	{
		npcClass.decrementChaseTimer ();
		
		npcClass.Move (npcClass.getTargetPlayer().transform, npcClass.chaseSpeed, true);
	}
	
} 
