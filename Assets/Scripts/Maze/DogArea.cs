using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DogArea: NetworkBehaviour
{
    [SyncVar] private List<Victim> victims = new List<Victim>();
    [SyncVar] public List<Dog> dogs = new List<Dog>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("New collider entered");
        var victim = other.gameObject.GetComponent<Victim>();
        victims.Add(victim);
        SetVictimToInactiveDog(victim);
    }

    private void SetVictimToInactiveDog(Victim victim)
    {
        foreach (var dog in dogs)
        {
            if (dog.GetVictim() == null)
            {
                dog.SetVictim(victim);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider exited");
        var victim = other.gameObject.GetComponent<Victim>();
        victims.Remove(victim);
        var dog = GetDogWithVictim(victim);
        if (dog is null)
        {
            return;
        }
        dog.UnsetVictim();
        var unusedVictim = GetUnusedVictim();
        if (unusedVictim is null)
        {
            return;
        }
        dog.SetVictim(unusedVictim);
        
    }

    private Dog GetDogWithVictim(Victim victim)
    {
        foreach (var dog in dogs)
        {
            if (dog.GetVictim() == victim)
            {
                return dog;
            }
        }

        return null;
    }

    private Victim GetUnusedVictim()
    {
        foreach (var victim in victims)
        {
            var dog = GetDogWithVictim(victim);
            if (dog is null)
            {
                return victim;
            }
        }

        return null;
    }
}