using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TntRun : MonoBehaviour
{
    [SerializeField] private float fallDistance = 2f;
    [SerializeField] private float fallTime = 1f;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float restoreTime = 5f;

    private Coroutine fallBlockCoroutine;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.GetComponent<Victim>())
        {
            fallBlockCoroutine ??= StartCoroutine(FallBlock());
        }
    }

    private IEnumerator FallBlock()
    {
        var material = gameObject.GetComponent<MeshRenderer>().material;
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        var t = 0f;
        var position = transform.position;
        var startPos = position;
        var endPos = position + Vector3.down * fallDistance;

        yield return new WaitForSeconds(waitTime);
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            var newColor = material.color;
            newColor.a = Mathf.Lerp(1, 0, t);
            material.color = newColor;
            t += Time.deltaTime / fallTime;
            yield return null;
        }
        boxCollider.enabled = false;
        yield return new WaitForSeconds(restoreTime);

        transform.position = startPos;
        var newNewColor = material.color;
        newNewColor.a = 1;
        material.color = newNewColor;
        boxCollider.enabled = true;
        fallBlockCoroutine = null;
    }
}
