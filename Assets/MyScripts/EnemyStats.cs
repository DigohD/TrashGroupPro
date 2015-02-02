using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

	public float health;
	public float dmg;
	private float speed;
	private float happiness;

	// Use this for initialization
	void Start () {
	
	}

	public void TakesDamage (float damage) {
		health -= damage;
		if (health <= 0)
			Destroy (this.gameObject);
	}
}
