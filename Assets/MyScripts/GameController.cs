using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject trash;
	public GameObject battery;
	public GameObject barrelYellow;
	public GameObject barrelGreen;
	public GameObject barrelBlack;
	public GameObject laserTurret;

	public GameObject WhaleClass;
	public GameObject BlowfishClass;


	public Vector3 posSpawnValues;
	public Vector3 negSpawnValues;
	public int trashCount;
	public float spawnWait;
	//public float startWait;
	public float waveWait;
	public bool initialSpawning;
	
	void Start()
	{	
		StartCoroutine (SpawnCont());
		spawnNPCs();
	}

	void spawnNPCs(){
		Network.Instantiate (WhaleClass, new Vector3(80, 10, 80), Quaternion.identity, 0);
		Network.Instantiate (BlowfishClass, new Vector3(37, 15, 2), Quaternion.identity, 0);
	}

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
		/*if(!Network.isMessageQueueRunning)
			return;*/

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
				GameObject t = (GameObject) Network.Instantiate (laserTurret, spawnPosition, laserTurret.transform.rotation, 0);
					t.name="Barrel Green";
				}
			else if (rnd > 1) {
				GameObject t = (GameObject) Network.Instantiate (barrelBlack, spawnPosition, spawnRotation, 0);
				t.name="Barrel Black";
			}
			}
		}
}
