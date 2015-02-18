using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender;
	public float dmg;
	// Use this for initialization


	void OnTriggerEnter ( Collider other ) {
		dmg = 30;
		if (other.tag == "Player") {
			PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();

			if(EnemyStats.ID != this.sender){
				EnemyStats.takeDamage(dmg);
				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}

		}
	}



}
