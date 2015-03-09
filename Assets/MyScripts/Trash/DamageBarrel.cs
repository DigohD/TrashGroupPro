using UnityEngine;
using System.Collections;

public class DamageBarrel : MonoBehaviour {

	public Mesh brokenBarrel;
	private float healthPercent;
	private TrashStats stats;
	public GameObject barrel; 

	void Start(){
		stats = gameObject.GetComponent<TrashStats> ();
	}

	// Set barrel model to the broken version if health percent is below 40%
	public void barrelHit(){
		healthPercent = stats.health / stats.maxHealth;

		if(healthPercent<0.40f)
		barrel.GetComponent<MeshFilter> ().mesh = brokenBarrel;
	}
}
