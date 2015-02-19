using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public GameObject laserShot;

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

		if(!networkView.isMine)
			return;

		if(Input.GetMouseButtonDown (0) && timer >= timeBetweenBullets && Time.timeScale != 0) // leave it for now, move it into inputscript laters
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
			networkView.RPC ("rpcDisableGunEffects", RPCMode.All, 0);
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
    void Shoot ()
    {
		if(!networkView.isMine)
			return;

		PlayerStats stats = transform.GetComponentInParent<PlayerStats> ();

		if (alternate){//alternate between the two cannons
			cannon = transform.GetChild (2);
			alternate = false;
		}
		else {
			cannon = cannon = transform.GetChild (3);
			alternate = true;
		}
		speed = 1;
		GameObject laser =  (GameObject) Network.Instantiate (laserShot, cannon.position, cannon.transform.rotation, 0);
		laser.rigidbody.velocity = cannon.up * speed*10;
		shotHit sh = (shotHit) laser.GetComponent("shotHit");
		sh.setSender(stats.ID);
        timer = 0f;

		networkView.RPC("rpcShootTurretEffects", RPCMode.All, 0);


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

	[RPC]
	void rpcShootTurretEffects(int wasted){
		gunAudio.Play ();
		
		gunLight.enabled = true;
	}

	[RPC]
	void rpcDisableGunEffects(int wasted){
		DisableEffects();
	}
}
