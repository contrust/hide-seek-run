using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Dog: RequireInstance<DogArea>
{
    public MovableAgent agent;
    private Victim victim;
    public float biteDistance;
    public int biteDamage;
    public float bitePeriod;
    private float biteCooldown;
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
    }

    private void Update()
    {
        UpdateBiteCooldown();
        if (ShouldBite())
        {
            Bite();
        }
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
        victim.onDeath.RemoveListener(UnsetDeadVictim);
        victim = null;
        onUnsetVictim.Invoke();
    }

    private void BiteVictim()
    {
        victim.GetDamage(biteDamage, transform);
        victim.GetStun();
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