using UnityEngine;
using System.Collections;


//Feign Player State
//--------------------------------------------------
public class StateFeign : FSMState
{
	private NPCAI npcClass;
	private float feignTimer = 10f;				//feign for 10 seconds
	private float feignSpeedMultiplier = 0.8f;		//20% speed reduction when feigning
	
	public StateFeign(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Feign;
	}
	
	public override void Reason()
	{


		float distanceToPlayer = Vector3.Distance (npcClass.getNPC ().transform.position, npcClass.getTargetPlayer ().transform.position);

		// If the player has gone 15 meters away from the NPC, fire LostPlayer transition or If the feign timer is up
		if (distanceToPlayer >= 10f || feignTimer < 0)
		{
			npcClass.SetTransition(Transition.LostPlayer);
			feignTimer = 10f;
		}

		//See the player. Decide if it should pursue it or not. Pursue only if its close, otherwise get freaked out and run!
		if(npcClass.playerInSight())
		{
			if(distanceToPlayer > 3f)
			{
				npcClass.SetTransition(Transition.ScaredOfPlayer);
			}
			else
			{
				npcClass.SetTransition(Transition.SawPlayer);
			}

		}
		
		
	}
	
	public override void Act()
	{
		feignTimer -= Time.deltaTime;
		
		npcClass.Move (npcClass.getTargetPlayer().transform, feignSpeedMultiplier, true);
	}
	
} 
