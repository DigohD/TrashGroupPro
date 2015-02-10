using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public GameObject laserShot;


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        //shootableMask = LayerMask.GetMask ("Shootable"); // use Layers to define shootable components
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) // leave it for now, move it into inputscript laters
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

	private bool alternate = true;
	private Transform cannon;
	private float speed;
    void Shoot ()
    {
		if (alternate){
			cannon = transform.GetChild (2);
			alternate = false;
			}
		else {
			cannon = cannon = transform.GetChild (3);
			alternate = true;
		}
		speed = 1;
		GameObject laser =  Instantiate (laserShot, cannon.position, transform.rotation) as GameObject;
		laser.rigidbody.velocity = cannon.forward * speed*10;

        timer = 0f;

        gunAudio.Play ();

        /*gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
*/

    }
}
