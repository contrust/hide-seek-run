using Mirror;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DogAnimation : NetworkBehaviour
{
	private static readonly int RunningHash = Animator.StringToHash("Running");

	private Animator animator;
	[SerializeField] private MovableAgent agent;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (!isServer) return;
		Debug.Log(agent.agent.velocity);
		Debug.Log(agent.agent.path.status);
		SetRunning(agent.agent.velocity != Vector3.zero);
	}

	[ClientRpc]
	private void SetRunning(bool running)
	{
		animator.SetBool(RunningHash, running);
	}
}
