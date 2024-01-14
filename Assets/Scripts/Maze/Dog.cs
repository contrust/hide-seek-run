using UnityEngine;
using UnityEngine.Events;

public class Dog: MonoBehaviour
{
    public MovableAgent agent;
    public float biteDistance;
    public UnityEvent onBite = new UnityEvent();

    private void Start()
    {
        onBite.AddListener(LogBite);
    }

    private void Update()
    {
        BiteIfClose();
    }

    private void BiteIfClose()
    {
        if (agent.IsActive() && agent.GetDistance() < biteDistance)
        {
            onBite.Invoke();
        }
    }

    private void LogBite()
    {
        Debug.Log("I'm gonna bite this chubby little girl.");
    }
}