using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour // spaghetti xd
{
	const float respawnTime = 1f;
	[SerializeField] float maxTravelDistance;
	[SerializeField] float maxLifetime = 1;
	[SerializeField] GameObject DestructionEffect;
	[SerializeField] Rigidbody2D rb;
	const int EnemyLayerIndex = 11;
	Vector2 pastPos;
	float accumDistance = 0;
	Coroutine lifetimeRoutine;
	bool destroyed = false;
	PlayerTeam team;
	void Awake(){
		pastPos = transform.position;
		lifetimeRoutine = StartCoroutine(LifetimeDelay(maxLifetime));
	}
	public void SetTeam(PlayerTeam _team){
		team = _team;
	}
	void Update(){
		accumDistance += (pastPos - (Vector2)transform.position).magnitude;
		pastPos = transform.position;

		transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(rb.velocity.y, rb.velocity.x)*Mathf.Rad2Deg);
	}
	void OnCollisionEnter2D(Collision2D col){
		if(accumDistance > maxTravelDistance){
			DestroyBullet();
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == EnemyLayerIndex){
			EnemyController enemy = col.attachedRigidbody.gameObject.GetComponent<EnemyController>();
			enemy.DestroyEnemy();
		}
	}
	IEnumerator LifetimeDelay(float lifetimeSeconds){
		yield return new WaitForSeconds(lifetimeSeconds);
		DestroyBullet();
	}
	void DestroyBullet(){
		if(destroyed){
			return;
		}
		destroyed = true;
		// Instantiate(DestructionEffect,transform.position, Quaternion.Identity);
		PlayerManager.instance.SpawnPlayer(respawnTime, team);
		Destroy(gameObject);

	}
}
