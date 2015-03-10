﻿using UnityEngine;
using System.Collections;

public class ShotHitExplosive : MonoBehaviour {

	public string sender = "BigDaddy";
	public float dmg;
	public float explosionRadius;
	private bool damageDealt = false, playingPFX;
	// Use this for initialization

	public GameObject explosion;
	

	// When a projectile collides with an object
	void OnTriggerEnter (Collider other){
		// If this projectile was fired by the local player and hits another player's core
		if (networkView.isMine && other.tag == "Player"){
			PlayerStats EnemyStats = other.GetComponent<PlayerStats> ();
			// If the player hit is not the owner of this projectile
			if (!(sender.Length < 3) && !(EnemyStats.ID.Length < 3) && !EnemyStats.ID.Equals (this.sender)) {
				// If the projectile has not already dealt damage
				if(!damageDealt){
					// Deal damage to the enemy player
					EnemyStats.takeDamage (dmg);
					Collider[] explosionAOE = Physics.OverlapSphere (transform.position, explosionRadius);
					
					// For every piece of trash within vicinity
					for (int i = 0; i < explosionAOE.Length; i++) {
						// Do this with trash inside the danger zone
						if (explosionAOE [i].tag == "BodyPart" ) {	
							TrashType tType = explosionAOE [i].GetComponent<TrashType> ();
							if(tType.type == "Turret" && tType != null){
								Transform trash = explosionAOE[i].transform;
								float dist = Vector3.Distance(transform.position, trash.position);
								//adjust dmg depending on distance to center of expl.
								
								explosionAOE [i].GetComponent<TurretStats>().damageTaken(20, 0);
							}
							if(tType.type == "Trash" && tType != null){
								Transform trash = explosionAOE[i].transform;
								float dist = Vector3.Distance(transform.position, trash.position);
								//adjust dmg depending on distance to center of expl.
								print ("damage: " + (dmg/2 -dist*200));
								explosionAOE [i].GetComponent<TrashStats>().takeDamage(20, 0);
							}
							
						}
						if(explosionAOE [i].tag == "Player"){
							PlayerStats pStats = explosionAOE[i].GetComponent<PlayerStats>();
							Transform player = explosionAOE[i].transform;
							float dist = Vector3.Distance(transform.position, player.position);
							pStats.takeDamage(20);
						}
					}
				}
				// Destroy the projectile on all clients
				networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			}
		}
		// If this projectile was fired by the local player and hits an owned piece of trash
		else if (networkView.isMine && other.tag == "BodyPart") {
			// Fetch trash stats of hit object
			TrashStats tStats = other.GetComponent<TrashStats> ();
			
			// If tStats == null, the trash hit is a turret
			if(tStats == null){
				// Fetch turret stats of hit object
				TurretStats tuStats = other.GetComponent<TurretStats> ();
				
				// If the projectile has a different owner than the turrret hit
				if (!(sender.Length < 3) && tuStats.ownerID != null && !tuStats.ownerID.Equals(this.sender)) {	
					// If the projectile has not already dealt damage
					if(!damageDealt){
						// Combocount is the number of trash pieces destroyed this hit
						int comboCount = tuStats.damageTaken (dmg, 0);

						Collider[] explosionAOE = Physics.OverlapSphere (transform.position, explosionRadius);
						
						// For every piece of trash within vicinity
						for (int i = 0; i < explosionAOE.Length; i++) {
							// Do this with trash inside the danger zone
							if (explosionAOE [i].tag == "BodyPart" ) {	
								TrashType tType = explosionAOE [i].GetComponent<TrashType> ();
								if(tType.type == "Turret" && tType != null){
									Transform trash = explosionAOE[i].transform;
									float dist = Vector3.Distance(transform.position, trash.position);
									//adjust dmg depending on distance to center of expl.
									
									comboCount += explosionAOE [i].GetComponent<TurretStats>().damageTaken(20, 0);
								}
								if(tType.type == "Trash" && tType != null){
									Transform trash = explosionAOE[i].transform;
									float dist = Vector3.Distance(transform.position, trash.position);
									//adjust dmg depending on distance to center of expl.
									print ("damage: " + (dmg/2 -dist*200));
									comboCount += explosionAOE [i].GetComponent<TrashStats>().takeDamage(20, 0);
								}
								
							}
							if(explosionAOE [i].tag == "Player"){
								PlayerStats pStats = explosionAOE[i].GetComponent<PlayerStats>();
								Transform player = explosionAOE[i].transform;
								float dist = Vector3.Distance(transform.position, player.position);
								pStats.takeDamage(20);
							}
						}
						
						// If enough pieces were destroyed in one hit
						if(comboCount >= 3){
							// Fetch the UI handler and announce the combo
							GameObject[] go = GameObject.FindGameObjectsWithTag("mainControl");
							UIHandler uih = go[0].GetComponent<UIHandler>();
							uih.combo(comboCount);
						}
					}
					// Destroy the projectile on all clients
					networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
					return;
				}
				return;
			}
			// If tStats does exist, see if it is owned by another player
			else if (!(sender.Length < 3) && tStats.ownerID != null && !tStats.ownerID.Equals(this.sender)) {
				// If the projectile has not already dealt damage
				if(!damageDealt){
					// Combocount is the number of trash pieces destroyed this hit
					int comboCount = tStats.takeDamage (dmg, 0);
					
					// If enough pieces were destroyed in one hit
					if(comboCount >= 3){
						// Fetch the UI handler and announce the combo
						GameObject[] go = GameObject.FindGameObjectsWithTag("mainControl");
						UIHandler uih = go[0].GetComponent<UIHandler>();
						uih.combo(comboCount);
					}
				}
				// Destroy the projectile on all clients
				networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
			}
		}
		// If the projectile is shot by the local player and hits a piece of trash
		else if (networkView.isMine && other.tag == "Trash") { // for offline testing
			// fetch trash stats and deal damage to it
			TrashStats tStats = other.GetComponent<TrashStats> ();
			if(!damageDealt){
				tStats.takeDamage (dmg, 0);
				Collider[] explosionAOE = Physics.OverlapSphere (transform.position, explosionRadius);
				
				// For every piece of trash within vicinity
				for (int i = 0; i < explosionAOE.Length; i++) {
					// Do this with trash inside the danger zone
					if (explosionAOE [i].tag == "BodyPart" ) {	
						TrashType tType = explosionAOE [i].GetComponent<TrashType> ();
						if(tType.type == "Turret" && tType != null){
							Transform trash = explosionAOE[i].transform;
							float dist = Vector3.Distance(transform.position, trash.position);
							//adjust dmg depending on distance to center of expl.
							
							explosionAOE [i].GetComponent<TurretStats>().damageTaken(20, 0);
						}
						if(tType.type == "Trash" && tType != null){
							Transform trash = explosionAOE[i].transform;
							float dist = Vector3.Distance(transform.position, trash.position);
							//adjust dmg depending on distance to center of expl.
							print ("damage: " + (dmg/2 -dist*200));
							explosionAOE [i].GetComponent<TrashStats>().takeDamage(20, 0);
						}
						
					}
					if(explosionAOE [i].tag == "Player"){
						PlayerStats pStats = explosionAOE[i].GetComponent<PlayerStats>();
						Transform player = explosionAOE[i].transform;
						float dist = Vector3.Distance(transform.position, player.position);
						pStats.takeDamage(20);
					}
				}
			}
			// Destroy the projectile on all clients
			networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
		}
		// If the projectile is shot by the local player and hits an NPC
		else if (networkView.isMine && other.tag == "Enemy") { // for offline testing
			// fetch enemy stats and deal damage to it
			EnemyStats eStats = other.GetComponent<EnemyStats> ();
			if(!damageDealt){
				eStats.takeDamage (dmg);
				Collider[] explosionAOE = Physics.OverlapSphere (transform.position, explosionRadius);
				
				// For every piece of trash within vicinity
				for (int i = 0; i < explosionAOE.Length; i++) {
					// Do this with trash inside the danger zone
					if (explosionAOE [i].tag == "BodyPart" ) {	
						TrashType tType = explosionAOE [i].GetComponent<TrashType> ();
						if(tType.type == "Turret" && tType != null){
							Transform trash = explosionAOE[i].transform;
							float dist = Vector3.Distance(transform.position, trash.position);
							//adjust dmg depending on distance to center of expl.
							
							explosionAOE [i].GetComponent<TurretStats>().damageTaken(20, 0);
						}
						if(tType.type == "Trash" && tType != null){
							Transform trash = explosionAOE[i].transform;
							float dist = Vector3.Distance(transform.position, trash.position);
							//adjust dmg depending on distance to center of expl.
							print ("damage: " + (dmg/2 -dist*200));
							explosionAOE [i].GetComponent<TrashStats>().takeDamage(20, 0);
						}
						
					}
					if(explosionAOE [i].tag == "Player"){
						PlayerStats pStats = explosionAOE[i].GetComponent<PlayerStats>();
						Transform player = explosionAOE[i].transform;
						float dist = Vector3.Distance(transform.position, player.position);
						pStats.takeDamage(20);
					}
				}
			}
			// Destroy the projectile on all clients
			networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
		}
		// If the projectile is shot by the local player and hits terrain
		else if (networkView.isMine && other.tag == "Untagged") {
			// Destroy the projectile on all clients
			networkView.RPC ("rpcDestroyShot", RPCMode.All, 0);
		}
	}
	
	// Set the owner of this projectile on all clients over the network
	public void setSender(string newSender){
		networkView.RPC ("rpcSetSender", RPCMode.All, newSender);
	}
	
	// Destroy the projectile when it has played its death particles
	void Update(){
		if(playingPFX)
			if(!transform.GetComponentInChildren<ParticleSystem>().isPlaying)
				Destroy(gameObject);
	}
	
	// Used to synchronize projectile ownership over the network
	[RPC]
	void rpcSetSender(string newSender){
		sender = newSender;
	}
	
	// Used to initiate particle destruction over the network
	[RPC]
	void rpcDestroyShot(int wasted){
		// This projectile has dealt its damage
		damageDealt = true;
		// Enable particle effects for the destroyed shot.
		// NOTE: This does not work over the network for some reason...
		Destroy (this.gameObject);
		Instantiate (explosion, transform.position, transform.rotation);	
		// Set the playing particles to true
	}
}
