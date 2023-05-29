using System.Collections;
using Mirror;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationHelper : NetworkBehaviour
{
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int HitAngle = Animator.StringToHash("HitAngle");
    private static readonly int RunningHash = Animator.StringToHash("Running");
    private static readonly int VelocityYHash = Animator.StringToHash("VelocityY");
    
    private Animator animator;
    private StarterAssetsInputs input;
    private CharacterController controller;
    
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    
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

    [ClientRpc]
    public void TriggerDead(float angle)
    {
        animator.SetBool(Dead, true);
        animator.SetFloat(HitAngle, angle);
        StartCoroutine(DeadCoroutine());
    }

    private IEnumerator DeadCoroutine()
    {
        var t = 0f;
        while (t <= 1)
        {
            cameraRoot.position = Vector3.Lerp(from.position, to.position, t);
            cameraRoot.LookAt(transform.position);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
