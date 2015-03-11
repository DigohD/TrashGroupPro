using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

	public float health;
	public float dmg;
	private float speed;
	private float happiness;
	public GameObject[] drops = new GameObject[2];
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
			if(Random.value > 0.66){
				GameObject t = (GameObject) Network.Instantiate (drops[0], pos, drops[0].transform.rotation, 0);
			}else{
				GameObject t = (GameObject) Network.Instantiate (drops[1], pos, drops[1].transform.rotation, 0);
			}

		}
	}
}
