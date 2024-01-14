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
                SetVictimCommand(victim, dog);
                break;
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void SetVictimCommand(Victim victim, Dog dog)
    {
        SetVictimRpc(victim, dog);
    }

    [ClientRpc]
    private void SetVictimRpc(Victim victim, Dog dog)
    {
        dog.SetVictim(victim);
    }
    
    [Command(requiresAuthority = false)]
    private void UnsetVictimCommand(Dog dog)
    {
        UnsetVictimRpc(dog);
    }
    
    [ClientRpc]
    private void UnsetVictimRpc(Dog dog)
    {
        dog.UnsetVictim();
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

        UnsetVictimCommand(dog);
        var unusedVictim = GetUnusedVictim();
        if (unusedVictim is null)
        {
            return;
        }
        SetVictimCommand(unusedVictim, dog);
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