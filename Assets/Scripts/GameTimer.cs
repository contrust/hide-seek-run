using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer: MonoBehaviour
{
    public const float gameTimeInSeconds = 15*60;
    public static UnityEvent OnTimeIsOver = new UnityEvent();

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(gameTimeInSeconds);
        OnTimeIsOver.Invoke();
    }
}