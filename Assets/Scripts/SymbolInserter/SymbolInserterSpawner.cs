using Mirror;
using UnityEngine;

public class SymbolInserterSpawner : NetworkBehaviour
{
	[SerializeField] private SymbolInserter inserter;
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
			var symbolInserter = Instantiate(inserter, position.position, position.rotation);
			symbolInserter.ID = i++;
			NetworkServer.Spawn(symbolInserter.gameObject);
		}
	}
}
