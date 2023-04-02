﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SymbolInserterSpawner : NetworkBehaviour
{
	[SerializeField] private GameObject inserter;
	[SerializeField] private Transform[] positions;


	public override void OnStartServer()
	{
		SpawnInserter();
	}

	[Server]
	private void SpawnInserter()
	{
		Debug.Log("spawn");
		var i = 0;
		foreach (var position in positions)
		{
			var obj = Instantiate(inserter, position.position, position.rotation);
			obj.GetComponent<SymbolInserter>().id = i++;
			NetworkServer.Spawn(obj);
		}
	}


	private IEnumerator WaitNetworkServerActive(Action callback)
	{
		while (!NetworkServer.active)
		{
			yield return null;
		}
		callback();
	}
}