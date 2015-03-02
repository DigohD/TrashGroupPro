using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour {

	public PlayerStats pStats;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Enemy") {
			other.GetComponent<EnemyStats>().takeDamage(pStats.dmg);

		}
	}
}
