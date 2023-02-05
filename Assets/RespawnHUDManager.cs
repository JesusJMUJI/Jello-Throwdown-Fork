using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHUDManager : MonoBehaviour
{
	[SerializeField] RespawnHUD respawnA, respawnB;
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	public void AddRespawn(PlayerTeam team, float respawnTime){
		RespawnHUD respawnPrefab;
		switch(team){
			case PlayerTeam.A:
				respawnPrefab = respawnA;
				break;
			case PlayerTeam.B:
				respawnPrefab = respawnB;
				break;
			default:
				respawnPrefab = null;
				break;
		}
		Instantiate(respawnPrefab, transform).StartTimer(respawnTime);
	}
}
