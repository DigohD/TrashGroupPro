﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject trash;
	public GameObject battery;
	public GameObject barrelYellow;
	public GameObject barrelGreen;
	public GameObject barrelBlack;
	public GameObject laserTurret;

	public GameObject WhaleObj;
	public GameObject BlinkyObj;
	public GameObject PinkyObj;
	public GameObject InkyObj;
	public GameObject ClydeObj;
	public GameObject CoralFishObj;

	public Vector3 posSpawnValues;
	public Vector3 negSpawnValues;
	public int trashCount;
	public float spawnWait;
	//public float startWait;
	public float waveWait;
	public bool initialSpawning;

	public Texture cursorTexture;

	// Start the trash spawner and spawn NPCs
	void Start()
	{	
		StartCoroutine (SpawnCont());

		//Try to spawn NPCs every 30 seconds
		InvokeRepeating("spawnNPCs", 30, 30);

		spawnNPCs();
		Screen.showCursor = false;
	}

	private int cursorWidth = 46;
	private int cursorHeight = 46;
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Input.mousePosition.x - cursorWidth/2, Screen.height - Input.mousePosition.y - cursorHeight/2, cursorWidth, cursorHeight), cursorTexture);
	}

	void spawnNPCs()
	{
		if (!GameObject.Find("/Whale") ){
			GameObject Whale = (GameObject) Network.Instantiate (WhaleObj, GameObject.Find ("Waypoints/Whale/1").transform.position, Quaternion.identity, 0);
			Whale.name = WhaleObj.name;
		}

		if (!GameObject.Find("/Blinky")){
			GameObject Blinky = (GameObject) Network.Instantiate (BlinkyObj, GameObject.Find ("Waypoints/Blinky/1").transform.position, Quaternion.identity, 0);
			Blinky.name = BlinkyObj.name;
		}

		if (!GameObject.Find("/Pinky")){
			GameObject Pinky = (GameObject) Network.Instantiate (PinkyObj, GameObject.Find ("Waypoints/Pinky/1").transform.position, Quaternion.identity, 0);
			Pinky.name = PinkyObj.name;
		}

		if (!GameObject.Find("/Inky")){
			GameObject Inky = (GameObject) Network.Instantiate (InkyObj, GameObject.Find ("Waypoints/Inky/1").transform.position, Quaternion.identity, 0);
			Inky.name = InkyObj.name;
		}

		if (!GameObject.Find("/Clyde")){
			GameObject Clyde = (GameObject) Network.Instantiate (ClydeObj, GameObject.Find ("Waypoints/Clyde/1").transform.position, Quaternion.identity, 0);
			Clyde.name = ClydeObj.name;

		}

		/*if (!GameObject.Find("/CoralFish")){
			GameObject CoralFish = (GameObject) Network.Instantiate (CoralFishObj, GameObject.Find ("Waypoints/CoralFish/1").transform.position, Quaternion.identity, 0);
			CoralFish.name = CoralFishObj.name;
			
		}*/


	}

	// Spawns trash on the map
	IEnumerator SpawnCont()
	{
		//yield return new WaitForSeconds (startWait);
		while (true)
		{
			if(initialSpawning){
				for (int i = 0; i < trashCount; i++)
				{
					SpawnTrash ();
					yield return new WaitForSeconds (spawnWait);
				}break;
			}

			for (int i = 0; i < trashCount; i++)
			{
				SpawnTrash ();
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
		
	}

	private Vector3 spawnPosition;
	Collider[] spawnZone;

	void SpawnTrash()
	{
		int i = 0;
		while (true && i<=5) {						
			spawnPosition = new Vector3 (Random.Range (negSpawnValues.x, posSpawnValues.x), Random.Range (negSpawnValues.y, posSpawnValues.y), posSpawnValues.z);
			spawnZone = Physics.OverlapSphere (spawnPosition, 2);//2 hardcoded for now, just to make sure stuff don't spawn onto eachother
			i++;
			if (spawnZone.Length == 0)
				break;
			}
			if (i <= 5) {
				Quaternion spawnRotation = Quaternion.identity;
			float rnd = Random.value *3;
				if (rnd < 1){
					GameObject t = (GameObject) Network.Instantiate (barrelYellow, spawnPosition, spawnRotation, 0);
					t.name="Barrel Yellow";
				}
				else if (rnd < 2) {
				GameObject t = (GameObject) Network.Instantiate (barrelGreen, spawnPosition, spawnRotation, 0);
					t.name="Barrel Green";
				}
			else if (rnd > 1) {
				GameObject t = (GameObject) Network.Instantiate (barrelBlack, spawnPosition, spawnRotation, 0);
				t.name="Barrel Black";
			}
			}
		}
}
