using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject trash;
	public GameObject battery;
	public Vector3 spawnValues;
	public int trashCount;
	public float spawnWait;
	//public float startWait;
	public float waveWait;
	
	void Start()
	{
		StartCoroutine (SpawnCont ());
	}

	IEnumerator SpawnCont()
	{
		//yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < trashCount; i++)
			{
				SpawnTrash ();
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
		
	}
	
	void SpawnTrash()
	{
		Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x),Random.Range(-spawnValues.y, spawnValues.y),spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity;
		if(Random.value*2<1)
			Instantiate (trash, spawnPosition, spawnRotation);
		else
			Instantiate (battery, spawnPosition, spawnRotation);
	}
}
