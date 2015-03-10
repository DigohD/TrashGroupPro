using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

	public float health;
	public float dmg;
	private float speed;
	private float happiness;
	public GameObject drop;
	public GameObject blood;

	// Use this for initialization
	void Start () {
	
	}

	public void takeDamage (float damage) {
		health -= damage;

		Network.Instantiate (blood, gameObject.transform.position, blood.transform.rotation, 0);
		if (health <= 0){
			Vector3 pos = this.gameObject.transform.position;
			pos.z = 2f;
			Destroy (this.gameObject);
			if(drop != null);
			GameObject t = (GameObject) Network.Instantiate (drop, pos, drop.transform.rotation, 0);
		}
	}
}
