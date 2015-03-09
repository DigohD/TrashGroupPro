using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	public GameObject healthBarR;

	public PlayerStats stats;
	public GameObject mid;
	private GameObject healthBar;
	private GameObject healthBar2;

	private float healthPercent = 1f;
	private float maxHealth;

	/*
	 * The health bar is basically two planes on top of each other, one green and one red
	 */
	void Start () {
		// instantiate the health bar
		healthBar = (GameObject) Instantiate(healthBarR, mid.transform.position, Quaternion.identity);
		// Set the max health to the player health
		maxHealth = stats.health;

		// Find the green portion of the health bar and put it in healthBar2
		foreach (Transform child in healthBar.transform)
		{
			if(child.gameObject.CompareTag("HealthBarG")){
				healthBar2 = child.gameObject;
			}
		}

		// Set the position of, and scale, the health bar. Green is dependent on player health
		healthBar.transform.position = mid.transform.position + new Vector3(0f, 1.3f, -1f);
		healthBar.transform.localScale = new Vector3(10f, 1f, 1f);
		healthBar2.transform.localScale = new Vector3(healthPercent, 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		// Update position
		healthBar.transform.position = mid.transform.position + new Vector3(0f, 1.3f, -1f);

		// Update Green area to represent player remaining health
		healthBar.transform.localScale = new Vector3(10f, 1f, 1f);
		healthBar2.transform.localScale = new Vector3(healthPercent, 1f, 1f);
	}

	// When player is hit, update the remaining health in percent
	public void isHit(){
		healthPercent = stats.health/maxHealth;
	}

	void OnDestroy(){
		Destroy(healthBar);
	}
}
