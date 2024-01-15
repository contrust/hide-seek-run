using Mirror;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DogAnimation : MonoBehaviour
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
		SetRunning(agent.agent.velocity != Vector3.zero);
	}
	
	private void SetRunning(bool running)
	{
		animator.SetBool(RunningHash, running);
	}
}
