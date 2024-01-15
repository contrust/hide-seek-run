using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Dog: RequireInstance<DogArea>
{
    public MovableAgent agent;
    public AudioSource barkSound;
    public AudioSource biteSound;
    private Victim victim;
    public float biteDistance;
    public int biteDamage;
    public float bitePeriod;
    private float biteCooldown;
    public float stunDuration;
    public UnityEvent onBite = new UnityEvent();
    public UnityEvent onUnsetVictim = new UnityEvent();
    public UnityEvent<Victim> onVictimDeath = new UnityEvent<Victim>();
    
    private bool CanReachVictim()
    {
        return agent.GetDistance() < biteDistance;
    }

    private bool IsBiteCooldownPositive()
    {
        return biteCooldown <= 0;
    }

    private bool ShouldBite()
    {
        return agent.IsActive() &&
               CanReachVictim() && 
               IsBiteCooldownPositive();
    }

    private void Start()
    {
        onBite.AddListener(BiteVictim);
        onBite.AddListener(PlayBiteSoundCommand);
    }

    private void Update()
    {
        if (agent.GetTarget().IsDestroyed() && agent.IsActive())
        {
            UnsetVictim();
        }
        if (agent.IsActive())
        {
            if (!barkSound.isPlaying)
            {
                PlayBarkSoundCommand();
            }
        }
        else
        {
            if (barkSound.isPlaying)
            {
                StopBarkSoundCommand();
            }
        }
        UpdateBiteCooldown();
        if (ShouldBite())
        {
            Bite();
        }
    }
    
    [Command(requiresAuthority = false)]
    private void PlayBiteSoundCommand()
    {
        PlayBiteSoundRpc();
    }

    [ClientRpc]
    private void PlayBiteSoundRpc()
    {
        biteSound.Play();
    }

    [Command(requiresAuthority = false)]
    private void StopBarkSoundCommand()
    {
        StopBarkSoundRpc();
    }

    [ClientRpc]
    private void StopBarkSoundRpc()
    {
        barkSound.Stop();
    }

    [Command(requiresAuthority = false)]
    private void PlayBarkSoundCommand()
    {
        PlayBarkSoundRpc();
    }

    [ClientRpc]
    private void PlayBarkSoundRpc()
    {
        barkSound.Play();
    }
    

    private void UpdateBiteCooldown()
    {
        if (biteCooldown > 0)
        {
            biteCooldown -= Time.deltaTime;
        }
    }

    private void Bite()
    {
        onBite.Invoke();
    }

    public Victim GetVictim()
    {
        return victim;
    }

    private void SetVictim(Victim victim)
    {
        if (victim is null)
        {
            return;
        }
        this.victim = victim;
        this.victim.onDeath.AddListener(UnsetDeadVictim);
        agent.SetTarget(this.victim.transform);
    }

    private void UnsetDeadVictim()
    {
        var victimToUnset = victim;
        UnsetVictim();
        onVictimDeath.Invoke(victimToUnset);
    }

    private void UnsetVictim()
    {
        agent.UnsetTarget();
        victim = null;
        onUnsetVictim.Invoke();
    }

    private void BiteVictim()
    {
        victim.GetDamage(biteDamage, transform);
        victim.GetStun(stunDuration);
        ResetBiteCooldown();
    }

    private void ResetBiteCooldown()
    {
        biteCooldown = bitePeriod;
    }
    
    
    [Command(requiresAuthority = false)]
    public void SetVictimCommand(Victim victim)
    {
        SetVictimRpc(victim);
    }

    [ClientRpc]
    private void SetVictimRpc(Victim victim)
    {
        SetVictim(victim);
    }
    
    [Command(requiresAuthority = false)]
    public void UnsetVictimCommand()
    {
        UnsetVictimRpc();
    }
    
    [ClientRpc]
    private void UnsetVictimRpc()
    {
        UnsetVictim();
    }
}