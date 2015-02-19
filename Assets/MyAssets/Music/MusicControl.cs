using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour {

	public AudioSource ambient, battle;
	public bool goAmb, goBattle;

	GameObject[] players;

	// Use this for initialization
	void Start () {
		battle.volume = 0;
		ambient.volume = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("Player");

		GameObject myPlayer = null;
		if(players.Length > 0)
			for(int i = 0; i < players.Length; i++){
				if(players[i].networkView.isMine)
					myPlayer = players[i];
			}

		if(myPlayer == null)
			Debug.Log("Player Not Found!!!");

		float shortest = 10000;
		float dist = 100000;
		if(players.Length > 0)
			for(int i = 0; i < players.Length; i++){
				if(!players[i].networkView.isMine)
					dist = Vector3.Distance(players[i].transform.position,
				                                myPlayer.transform.position);
				if(dist < shortest)
					shortest = dist;
			}

		if(shortest < 15){
			goAmb = false;
			goBattle = true;
		}else{
			goAmb = true;
			goBattle = false;
		}

		if(goAmb && ambient.volume < 0.5f){
			ambient.volume = ambient.volume + 0.01f;
		}
		if(goAmb && battle.volume > 0f){
			battle.volume = battle.volume - 0.01f;
		}

		if(goBattle && ambient.volume > 0f){
			ambient.volume = ambient.volume - 0.01f;
		}
		if(goBattle && battle.volume < 0.5f){
			battle.volume = battle.volume + 0.01f;
		}

	}
}
