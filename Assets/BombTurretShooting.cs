using UnityEngine;
using System.Collections;

public class BombTurretShooting : MonoBehaviour {

	
	public float timeBetweenBullets;
	public GameObject bombShot;
	public WildTurretControlScript turret;
	public TurretStats stats;
	
	float timer;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	private bool hasShot;
	
	void Awake ()
	{
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}
	
	void Update ()
	{
		timer += Time.deltaTime;
		
		if(Input.GetMouseButton (1) && timer >= stats.timeBetweenBullets && Time.timeScale != 0 && turret.active) // leave it for now, move it into inputscript laters
		{
			Shoot ();
			hasShot = true;
		}
		
		if(timer >= stats.timeBetweenBullets * effectsDisplayTime && hasShot)
		{
			// Disable visual gun effects on all connected clients
			networkView.RPC ("rpcDisableGunEffects", RPCMode.All, 0);
			hasShot = false;
		}
	}
	
	
	public void DisableEffects ()
	{
		//gunLine.enabled = false;
		gunLight.enabled = false;
	}
	
	//private bool alternate = true;
	//private Transform cannon;
	private float speed;
	
	private int cannonNumber;
	
	void Shoot ()
	{
		// If this turret is not activated, return
		if(!turret.active)
			return;
		
		PlayerStats stats = transform.GetComponentInParent<PlayerStats> ();

		// Create a new projectile
		speed = 1;
		GameObject bomb =  (GameObject) Network.Instantiate (bombShot, transform.position, transform.rotation, 0);
		bomb.rigidbody.velocity = transform.forward * speed*10;

		//bomb.rigidbody.AddForce (transform.up*speed);
		ShotHitExplosive sh = (ShotHitExplosive) bomb.GetComponent("ShotHitExplosive");
		sh.setSender(stats.ID);
		sh.dmg = stats.dmg;
		timer = 0f;
		
		// Enable turret visual effects on all clients over the network
		networkView.RPC("rpcShootTurretEffects", RPCMode.All, cannonNumber);
		
		/*
        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
*/
	}
	
	// Used to activate turret visual effects over the network
	[RPC]
	void rpcShootTurretEffects(int newCannonNumber){
		gunAudio.Play ();
		
		//cannon = transform.GetChild (newCannonNumber);
		Transform LaserFlashT = transform.GetChild(0);
		ParticleSystem ps = LaserFlashT.gameObject.GetComponent<ParticleSystem>();
		ps.Play();
		
		gunLight.enabled = true;
	}
	
	// Used to disable turret visual effects over the network
	[RPC]
	void rpcDisableGunEffects(int wasted){
		DisableEffects();
	}
}
