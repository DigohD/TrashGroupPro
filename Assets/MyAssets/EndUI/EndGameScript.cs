using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	public UIHandler ui;
	private bool active = false, twoConnected = false;

	private GameObject[] players;

	// Update is called once per frame
	void Update () {
		if(!active)
			return;

		players = GameObject.FindGameObjectsWithTag("Player");

		if(players.Length > 1)
			twoConnected = true;

		if(twoConnected){
			if(players.Length == 1)
				if(!players[0].networkView.isMine)
					ui.setDefeat();
			else
				ui.setVictory();
		}

	}

	public void activate(){
		active = true;
	}
}
