using UnityEngine;
using System.Collections;


//Follow Player State
//--------------------------------------------------
public class StateFollow : FSMState
{
	private NPCAI npcClass;
	private float followTimer = 10f;				//follow for 10 seconds
	private float followSpeedMultiplier = 0.8f;		//20% speed reduction when following
	
	public StateFollow(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Follow;
	}
	
	public override void Reason()
	{


		float distanceToPlayer = Vector3.Distance (npcClass.getNPC ().transform.position, npcClass.getTargetPlayer ().transform.position);

		// If the player has gone 11 meters away from the NPC, fire LostPlayer transition or If the follow timer is up
		if (distanceToPlayer >= 11f || followTimer < 0)
		{
			npcClass.SetTransition(Transition.LostPlayer);
			followTimer = 10f;
		}

		//Got close enough to attack!
		if(distanceToPlayer < 2f)
		{
			npcClass.SetTransition(Transition.SawPlayer);
		}
	
		
	}
	
	public override void Act()
	{
		followTimer -= Time.deltaTime;
		
		npcClass.Move (npcClass.getTargetPlayer().transform, followSpeedMultiplier, true);
	}
	
} 
