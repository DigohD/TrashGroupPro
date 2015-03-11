using UnityEngine;
using System.Collections;


//Run Away from Player State
//--------------------------------------------------
public class StateRunAwayFromPlayer : FSMState
{
	private NPCAI npcClass;
	private float runAwayTimer = 3f;					//Run away for 3 seconds
	private float runSpeedMultiplier = 1.2f;			//20% speed boost when running away

	public StateRunAwayFromPlayer(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Run;
	}
	
	public override void Reason()
	{
		
		if (runAwayTimer < 0)
		{
			npcClass.SetTransition(Transition.LostPlayer);
			runAwayTimer = 3f;
		}

		if(npcClass.playerInSight ())
			npcClass.SetTransition(Transition.SawPlayer);
	}
	
	public override void Act()
	{
		runAwayTimer -= Time.deltaTime;
		
		npcClass.Move (npcClass.getTargetPlayer().transform, runSpeedMultiplier, false);
	}
	
} 


