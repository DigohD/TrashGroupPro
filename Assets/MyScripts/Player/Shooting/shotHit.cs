using UnityEngine;
using System.Collections;

public class shotHit : MonoBehaviour {

	public string sender;
	public float dmg;
	// Use this for initialization

	void Update(){
		print ("penisnssis");
	}
	void OnTriggerEnter ( Collider other ) {
		print ("hit");
		dmg = 30;
		if (other.tag == "Player") {
			PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();

			if(EnemyStats.pName != sender){
				EnemyStats.takeDamage(dmg);
				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}

		}
	}



}
