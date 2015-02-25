using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	public GameObject healthBarR;

	public PlayerStats stats;
	private GameObject healthBar;
	private GameObject healthBar2;

	private float healthPercent = 1f;
	private float maxHealth;

	void Start () {
		healthBar = (GameObject) Instantiate(healthBarR, gameObject.transform.position, Quaternion.identity);
		maxHealth = stats.health;

		foreach (Transform child in healthBar.transform)
		{
			if(child.gameObject.CompareTag("HealthBarG")){
				healthBar2 = child.gameObject;
			}
		}


		healthBar.transform.position = gameObject.transform.position + new Vector3(0f, 1.3f, -1f);
		healthBar.transform.localScale = new Vector3(10f, 1f, 1f);
		healthBar2.transform.localScale = new Vector3(healthPercent, 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		healthBar.transform.position = gameObject.transform.position + new Vector3(0f, 1.3f, -1f);

		healthBar.transform.localScale = new Vector3(10f, 1f, 1f);
		healthBar2.transform.localScale = new Vector3(healthPercent, 1f, 1f);
	}

	public void isHit(){
		healthPercent = stats.health/maxHealth;
	}

	void OnDestroy(){
		Destroy(healthBar);
	}
}
