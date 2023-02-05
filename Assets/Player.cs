using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] PlayerTeam playerTeam;
	[SerializeField] GameObject DestructionEffect;
	
	public delegate void DestructionHandler();
	public event DestructionHandler OnDestruction;
	const float reviveTime = 5f;
	public PlayerTeam team
	{
		get{
			return playerTeam;
		}
	}
	void Start()
	{
		PlayerManager.instance.AddPlayer(this);
	}
	void Update(){
		
	}
	public void Despawn(){
		PlayerManager.instance.RemovePlayer(this);
	}
	public void DestroyPlayer(){
		PlayerManager.instance.RemovePlayer(this);
		PlayerManager.instance.SpawnPlayer(reviveTime, team);
		// Instantiate(DestructionEffect,transform.position,transform.rotation);
		if(OnDestruction != null){
			OnDestruction.Invoke();
		}
		
		Destroy(gameObject);
	}
}
public enum PlayerTeam{
	A,
	B
}
