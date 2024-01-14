using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        StartCoroutine(TeleportCoroutine(other.gameObject));
    }

    private IEnumerator TeleportCoroutine(GameObject player)
    {
        while (Vector3.Distance(player.transform.position, destination.position) > 1f)
        {
            player.transform.position = destination.position;
            yield return null;
        }
    }
}
