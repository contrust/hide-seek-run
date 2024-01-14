using System;
using System.Collections.Generic;
using UnityEngine;

public class DogArea: MonoBehaviour
{
    private List<Transform> victimsTransforms = new List<Transform>();
    public List<Dog> dogs = new List<Dog>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("New collider entered");
        victimsTransforms.Add(other.transform);
        SetTargetToInactiveDog(other.transform);
    }

    private void SetTargetToInactiveDog(Transform target)
    {
        foreach (var dog in dogs)
        {
            if (!dog.agent.IsActive())
            {
                dog.agent.SetTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider exited");
        victimsTransforms.Remove(other.transform);
        var dog = GetDogWithTarget(other.transform);
        if (dog is null)
        {
            return;
        }
        dog.agent.Disable();
        var victimTransform = GetUnusedVictimTransform();
        if (victimTransform is null)
        {
            return;
        }
        dog.agent.SetTarget(victimTransform);
        
    }

    private Dog GetDogWithTarget(Transform target)
    {
        foreach (var dog in dogs)
        {
            if (dog.agent.GetTarget() == target)
            {
                return dog;
            }
        }

        return null;
    }

    private Transform GetUnusedVictimTransform()
    {
        foreach (var victimTransform in victimsTransforms)
        {
            var dog = GetDogWithTarget(victimTransform);
            if (dog is null)
            {
                return victimTransform;
            }
        }

        return null;
    }
}