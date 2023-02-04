using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayerController : MonoBehaviour
{
	[SerializeField] TriggerCol grabZone;
	[SerializeField] Transform grabPoint;
	[SerializeField] float maxDistance;
	[SerializeField] Vector2 bulletVelocity;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Rigidbody2D PlayerBullet;
	[SerializeField] KeyCode throwKey;
	[SerializeField] Player selfPlayer;
	[SerializeField] GameObject SmallPlayerPrefab;
	const int playerLayerIndex = 6;
	Player grabbedPlayer;
	void Start()
	{
		PlayerManager.instance.OnPlayersChanged += UpdatePlayers;
	}
	void UpdatePlayers(Player[] players){
		if(players.Length == 1 && players[0] == selfPlayer){
			//await animation
			ReplacePlayer();
		}
	}
	void ReplacePlayer(){
		Instantiate(SmallPlayerPrefab, transform.position, Quaternion.identity);
		selfPlayer.Despawn();
		Destroy(gameObject);
	}
	void LateUpdate()
	{
		if(!grabbedPlayer){
			CheckGrabOverlap();
		}
		else{
			GrabLerp();
			if(Input.GetKeyDown(throwKey)){
				StartThrow();
			}
		}
	}
	void CheckGrabOverlap(){
		if(grabZone.colCount == 0){
			return;
		}
		Collider2D[] cols = grabZone.overlapCols;

		for(int i = 0; i < cols.Length; i++){
			if(cols[i].gameObject.layer == playerLayerIndex){
				GrabPlayer(cols[i].attachedRigidbody.gameObject);
				return;
			}
		}
	}
	void GrabLerp(){
		// grabbedPlayer.position =grabPoint.position;
		Vector2 lerpDelta = grabPoint.position - grabbedPlayer.transform.position;
		if(lerpDelta.magnitude > maxDistance){

			grabbedPlayer.transform.position = (Vector2)grabPoint.position - lerpDelta.normalized*maxDistance;
		}
		// grabbedPlayer.position = Vector2.Lerp(grabbedPlayer.position, grabPoint.position, Time.deltaTime*lerpSpeed);
	}
	void GrabPlayer(GameObject player){
		SmallPlayerController controller = player.GetComponent<SmallPlayerController>();
		controller.EnableStruggleMode();
		grabbedPlayer = player.GetComponent<Player>();
	}
	void StartThrow(){
		//Handle animator
		ThrowPlayer(); // remove call when animating
	}
	void ThrowPlayer(){

		
		Rigidbody2D bulletRB = Instantiate(PlayerBullet, grabPoint.position, grabPoint.rotation);
		Vector2 vel = bulletVelocity;
		vel.x *= Mathf.Sign(rb.velocity.x);
		bulletRB.velocity = vel;
		BulletController bullet = bulletRB.gameObject.GetComponent<BulletController>();
		bullet.SetTeam(grabbedPlayer.team);
		grabbedPlayer.Despawn();
		Destroy(grabbedPlayer.gameObject);
	}
}