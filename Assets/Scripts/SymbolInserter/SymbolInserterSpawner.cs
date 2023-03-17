using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SymbolInserterSpawner : NetworkBehaviour
{
	[SerializeField] private GameObject inserter;
	[SerializeField] private Transform position;


	private void Start()
	{
		SpawnInserter();
	}
	
	private void SpawnInserter()
	{
		Debug.Log("spawn");
		var obj = Instantiate(inserter, position.position, Quaternion.identity);
		NetworkServer.Spawn(obj);
	}
}
