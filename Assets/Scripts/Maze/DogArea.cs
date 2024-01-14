using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DogArea: NetworkBehaviour
{
    [SyncVar] private List<Victim> victims = new List<Victim>();
    [SyncVar] public List<Dog> dogs = new List<Dog>();

    private void Start()
    {
        foreach (var dog in dogs)
        {
            var dogToAddListener = dog;
            dog.onUnsetVictim.AddListener(() => FindAndSetVictimToDog(dogToAddListener));
            dog.onVictimDeath.AddListener(victim => victims.Remove(victim));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        var victim = other.gameObject.GetComponent<Victim>();
        victims.Add(victim);
        if (SetVictimToInactiveDogs(victim))
        {
            return;
        }
        SetVictimToDogWithDuplicateVictim(victim);
    }

    private bool SetVictimToDogWithDuplicateVictim(Victim victim)
    {
        var usedVictims = new HashSet<Victim>();
        foreach (var dog in dogs)
        {
            var dogVictim = dog.GetVictim();
            if (dogVictim is null)
            {
                continue;
            }
            if (usedVictims.Contains(dogVictim))
            {
                dog.UnsetVictimCommand();
                dog.SetVictimCommand(victim);
                return true;
            }

            usedVictims.Add(dogVictim);
        }

        return false;
    }

    private bool SetVictimToInactiveDogs(Victim victim)
    {
        var doesInactiveDogExist = false;
        foreach (var dog in dogs)
        {
            if (dog.GetVictim() == null)
            {
                dog.SetVictimCommand(victim);
                doesInactiveDogExist = true;
            }
        }

        return doesInactiveDogExist;
    }

    private void OnTriggerExit(Collider other)
    {
        var victim = other.gameObject.GetComponent<Victim>();
        RemoveVictim(victim);
    }

    private void RemoveVictim(Victim victim)
    {
        victims.Remove(victim);
        foreach (var dog in dogs)
        {
            if (dog.GetVictim() == victim)
            {
                ResetVictim(dog);
            }
        }
    }

    private void ResetVictim(Dog dog)
    {
        dog.UnsetVictimCommand();
        FindAndSetVictimToDog(dog);
    }

    private void FindAndSetVictimToDog(Dog dog)
    {
        if (SetUnusedVictimToDog(dog))
        {
            return;
        }
        SetFirstVictimToDog(dog);
    }

    private void SetFirstVictimToDog(Dog dog)
    {
        foreach (var victim in victims)
        {
            dog.SetVictimCommand(victim);
            break;
        }
    }

    private bool SetUnusedVictimToDog(Dog dog)
    {
        var unusedVictim = GetUnusedVictim();
        if (unusedVictim is null)
        {
            return false;
        }
        dog.SetVictimCommand(unusedVictim);
        return true;
    }

    private bool DoesDogWithVictimExist(Victim victim)
    {
        foreach (var dog in dogs)
        {
            if (dog.GetVictim() == victim)
            {
                return true;
            }
        }

        return false;
    }

    private Victim GetUnusedVictim()
    {
        foreach (var victim in victims)
        {
            if (!DoesDogWithVictimExist(victim))
            {
                return victim;
            }
        }

        return null;
    }
}