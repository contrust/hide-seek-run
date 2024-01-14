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
        onBite.AddListener(LogBite);
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

    public void SetVictim(Victim victim)
    {
        this.victim = victim;
        this.victim.onDeath.AddListener(UnsetVictim);
        agent.SetTarget(this.victim.transform);
    }

    public void UnsetVictim()
    {
        agent.UnsetTarget();
        victim = null;
    }

    public void BiteVictim()
    {
        victim.GetDamage(biteDamage, transform);
        victim.GetStun();
        ResetBiteCooldown();
    }

    private void ResetBiteCooldown()
    {
        biteCooldown = bitePeriod;
    }

    private void LogBite()
    {
        Debug.Log("I'm gonna bite this chubby little girl.");
    }
}