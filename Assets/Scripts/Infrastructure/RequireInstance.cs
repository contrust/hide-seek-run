using System.Collections;
using Mirror;
using UnityEngine;

public abstract class RequireInstance<T>: NetworkBehaviour where T: MonoBehaviour 
{
	protected virtual void CallbackAll(T instance) {}
	protected virtual void CallbackServer() {}
	protected virtual void OnStart(){}

	private bool callbackAllIsFinished;

	private void Start()
	{
		StartCoroutine(FindObject());
	}

	public override void OnStartServer()
	{
		StartCoroutine(WaitFindObject());
	}

	private IEnumerator FindObject()
	{
		Debug.Log($"Finding {typeof(T)}");
		var instance = FindObjectOfType<T>();
		while (instance is null)
		{
			yield return new WaitForSeconds(0.1f);
			instance = FindObjectOfType<T>();
		}
		Debug.Log($"Found {typeof(T)}");
		CallbackAll(instance);
		Debug.Log($"Callback {typeof(T)}");
		callbackAllIsFinished = true;
	}

	private IEnumerator WaitFindObject()
	{
		while (!callbackAllIsFinished)
		{
			yield return new WaitForSeconds(0.1f);
		}
		CallbackServer();
	}
}

