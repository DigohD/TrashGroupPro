using UnityEngine;
using System.Collections;


//Chase Player State
//--------------------------------------------------
public class StateChasePlayer : FSMState
{
	private NPCAI npcClass;
	private float chaseTimer = 3f;					//Chase for 3 seconds
	private float chaseSpeedMultiplier = 1.5f;		//50% speed boost when chasing

	public StateChasePlayer(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Chase;
	}
	
	public override void Reason()
	{

		// If the player has gone 5 meters away from the NPC, fire LostPlayer transition or If the chase timer is up
		if (Vector3.Distance(npcClass.getNPC().transform.position, npcClass.getTargetPlayer().transform.position) >= 5 || chaseTimer < 0)
		{
			npcClass.SetTransition(Transition.LostPlayer);
			chaseTimer = 3f;
		}


	}
	
	public override void Act()
	{
		chaseTimer -= Time.deltaTime;
		
		npcClass.Move (npcClass.getTargetPlayer().transform, chaseSpeedMultiplier, true);
	}
	
} 
