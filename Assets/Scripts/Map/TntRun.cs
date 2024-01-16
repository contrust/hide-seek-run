using System.Collections;
using Mirror;
using UnityEngine;

public class TntRun : NetworkBehaviour
{
    [SerializeField] private float fallDistance = 2f;
    [SerializeField] private float fallTime = 1f;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float restoreTime = 5f;

    [SyncVar(hook = nameof(SetAlpha))] public float alpha;
    [SyncVar(hook = nameof(SetColliderEnabled))] public bool colliderEnabled;
    
    private Coroutine fallBlockCoroutine;
    private Material material;
    private BoxCollider boxCollider;

    private void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void SetAlpha(float _, float newValue)
    {
        alpha = newValue;
        var newColor = material.color;
        newColor.a = alpha;
        material.color = newColor;
    }

    private void SetColliderEnabled(bool _, bool newValue)
    {
        colliderEnabled = newValue;
        boxCollider.enabled = newValue;
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Victim>())
        {
            if (isServer)
            {   
                fallBlockCoroutine ??= StartCoroutine(FallBlock());
            }
            else
            {
                TriggerOnServer();
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void TriggerOnServer()
    {
        fallBlockCoroutine ??= StartCoroutine(FallBlock());
    }

    private IEnumerator FallBlock()
    {
        var t = 0f;
        var position = transform.position;
        var startPos = position;
        var endPos = position + Vector3.down * fallDistance;

        yield return new WaitForSeconds(waitTime);
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            alpha = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / fallTime;
            yield return null;
        }
        colliderEnabled = false;
        yield return new WaitForSeconds(restoreTime);

        transform.position = startPos;
        alpha = 1;
        colliderEnabled = true;
        fallBlockCoroutine = null;
    }
}
