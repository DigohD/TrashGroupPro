using UnityEngine;
using System.Collections;


//Run Away from Player State
//--------------------------------------------------
public class StateRunAwayFromPlayer : FSMState
{
	private NPCAI npcClass;
	
	public StateRunAwayFromPlayer(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Running;
	}
	
	public override void Reason()
	{
		
		// If the player has gone 30 meters away from the NPC, fire LostPlayer transition
		if (npcClass.getChaseTimer() < 0)
			npcClass.SetTransition(Transition.LostPlayer);
	}
	
	public override void Act()
	{
		npcClass.decrementChaseTimer ();
		
		npcClass.Move (npcClass.getTargetPlayer().transform, npcClass.chaseSpeed, false);
	}
	
} 


