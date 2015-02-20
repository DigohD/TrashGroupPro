using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject trash;
	public GameObject battery;
	public GameObject barrelYellow;
	public Vector3 posSpawnValues;
	public Vector3 negSpawnValues;
	public int trashCount;
	public float spawnWait;
	//public float startWait;
	public float waveWait;
	public bool initialSpawning;
	
	void Start()
	{	
		StartCoroutine (SpawnCont ());
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
				if (Random.value * 1 < 1){
					GameObject t = (GameObject) Network.Instantiate (barrelYellow, spawnPosition, spawnRotation, 0);
					t.name="Barrel Yellow";
				}
				else{
					GameObject t = (GameObject) Network.Instantiate (battery, spawnPosition, spawnRotation, 0);
					t.name="Trash Battery";
				}
			}
		}
}
