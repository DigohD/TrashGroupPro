﻿using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public GameObject laserShot;

	private bool hasShot;
    float timer;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Awake ()
    {
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }

    void Update ()
    {
        timer += Time.deltaTime;

		// If this turret is not owned by the local player, return
		if(!networkView.isMine)
			return;

		if(Input.GetMouseButton (0) && timer >= timeBetweenBullets && Time.timeScale != 0) // leave it for now, move it into inputscript laters
        {
            Shoot ();
			hasShot = true;
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime && hasShot)
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

	private bool alternate = true;
	private Transform cannon;
	private float speed;

	private int cannonNumber;

    void Shoot ()
    {
		// If this turret is not owned by the local player, return
		if(!networkView.isMine)
			return;

		PlayerStats stats = transform.GetComponentInParent<PlayerStats> ();

		if (alternate){//alternate between the two cannons
			cannon = transform.GetChild (2);
			cannonNumber = 2;
			alternate = false;
		}
		else {
			cannon = transform.GetChild (3);
			cannonNumber = 3;
			alternate = true;
		}
		// Create a new projectile
		speed = 1;
		GameObject laser =  (GameObject) Network.Instantiate (laserShot, cannon.position, cannon.transform.rotation, 0);
		laser.rigidbody.velocity = cannon.up * speed*10;
		shotHit sh = (shotHit) laser.GetComponent("shotHit");
		sh.setSender(stats.ID);
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

		cannon = transform.GetChild (newCannonNumber);
		Transform LaserFlashT = cannon.GetChild(0);
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
