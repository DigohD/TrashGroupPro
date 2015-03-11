using UnityEngine;
using System.Collections;


//Scatter Player State
//--------------------------------------------------
public class StateScatter : FSMState
{
	private NPCAI npcClass;
	private float scatterTimer = 5f;				//scatter for 5 seconds

	public StateScatter(NPCAI o)
	{
		//access to outer class members
		npcClass = o;
		
		stateID = StateID.Scatter;
	}
	
	public override void Reason()
	{

		float magnitude = (npcClass.getNPC().transform.position - npcClass.waypoints [0].position).magnitude;

		if ( scatterTimer < 0 || magnitude < 1)
		{
			npcClass.SetTransition(Transition.LostPlayer);
			scatterTimer = 5f;
		}

	
	}
	
	public override void Act()
	{
		scatterTimer -= Time.deltaTime;
		//Go back to the start
		npcClass.Move (npcClass.waypoints [0].transform, 1, true);
	}
	
} 
