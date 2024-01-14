using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovableAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform target;
    public float updatePathPeriod = 0.5f;

    public void Start()
    {
        agent.isStopped = true;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public float GetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public bool IsActive()
    {
        return !agent.isStopped;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.isStopped = false;
        StartCoroutine(UpdatePathCoroutine());
        Debug.Log("Target set successfully");
    }

    public void Disable()
    {
        target = null;
        agent.isStopped = true;
    }

    private IEnumerator UpdatePathCoroutine()
    {
        while (IsActive())
        {
            UpdatePath();
            yield return new WaitForSeconds(updatePathPeriod);
        }
    }

    private void UpdatePath()
    {
        if (!agent || !target)
        {
            Debug.Log(agent, target);
            return;
        }
        
        var path = new NavMeshPath();
        agent.CalculatePath(target.position, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log(path.status);
            return;
        }

        agent.SetDestination(target.position);
        Debug.Log("Destination set successfully");
    }
}