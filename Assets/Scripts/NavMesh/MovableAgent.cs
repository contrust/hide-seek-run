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
        return  !(target is null) && !agent.isStopped;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.isStopped = false;
        StartCoroutine(UpdatePathCoroutine());
    }

    public void UnsetTarget()
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
        if (!agent)
        {
            return;
        }
        if (!target)
        {
            UnsetTarget();
            return;
        }
        
        var path = new NavMeshPath();
        agent.CalculatePath(target.position, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return;
        }

        agent.SetDestination(target.position);
    }
}