using Mirror;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationHelper : NetworkBehaviour
{
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int RunningHash = Animator.StringToHash("Running");
    private static readonly int VelocityYHash = Animator.StringToHash("VelocityY");
    
    private Animator animator;
    private StarterAssetsInputs input;
    private CharacterController controller;

    private void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        
        if (input.jump) TriggerJump();
        SetRunning(input.move != Vector2.zero);
        SetVelocity(controller.velocity.y);
    } 
    
    [Command]
    private void TriggerJump()
    {
        animator.SetTrigger(JumpHash);
    }
    
    [Command]
    private void SetRunning(bool running)
    {
        animator.SetBool(RunningHash, running);
    }

    [Command]
    private void SetVelocity(float velocityY)
    {
        animator.SetFloat(VelocityYHash, velocityY);
    }
}
