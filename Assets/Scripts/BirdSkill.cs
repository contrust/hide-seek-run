using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class BirdSkill : NetworkBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Transform raycastPoint;
	[SerializeField] private float distanceToObjectForVerticalFly = 2f;
	[SerializeField] private Transform view;
	[SerializeField] private float speed;
	[SerializeField] private float stunTimeInSeconds = 5;
	[SerializeField]
	private LayerMask layerMask = Physics.DefaultRaycastLayers;

	private void Start()
	{
		FindTarget();
	}

	private void Update()
	{
		if (target)
		{
			Follow();
		}
	}

	private void FindTarget()
	{
		var victims = FindObjectsOfType<Victim>().Select(x => x.transform).ToArray();

		var minDist = float.MaxValue;
		Transform targetVictim = null;

		foreach (Transform victim in victims)
		{
			var dist = Vector3.Distance(victim.position, view.position);
			if (dist < minDist)
			{
				minDist = dist;
				targetVictim = victim;
			}
		}
		target = targetVictim;
	}

	private void Follow()
	{
		var targetPos = target.position;
		var currentPos = raycastPoint.position;

		var direction2dRaycast = targetPos - currentPos;
		direction2dRaycast.y = 0;
		var flyDirection = direction2dRaycast.normalized;
		
		Debug.DrawRay(
			currentPos, 
			direction2dRaycast.normalized * direction2dRaycast.magnitude, 
			Color.blue, 
			0.1f);
		Debug.DrawRay(currentPos, targetPos - currentPos, Color.green, 0.1f);
		if (currentPos.y < targetPos.y)
			flyDirection.y = 1;
		else if (Physics.Raycast(
			    currentPos,
			    direction2dRaycast.normalized,
			    out var hitInfo,
			    direction2dRaycast.magnitude,
			    layerMask,
			    QueryTriggerInteraction.Ignore))
		{
			if (hitInfo.distance <= distanceToObjectForVerticalFly)
			{
				flyDirection = Vector3.zero;
			}
			flyDirection.y = 1;
		} else if (!Physics.Raycast(
			           currentPos,
			           targetPos - currentPos,
			           out _,
			           (targetPos - currentPos).magnitude,
			           layerMask,
			           QueryTriggerInteraction.Ignore) && currentPos.y > targetPos.y)
		{
			flyDirection.y = -1;
		}
		transform.Translate(flyDirection * speed * Time.deltaTime);
		view.LookAt(targetPos);
		view.rotation = Quaternion.Euler(0, view.rotation.eulerAngles.y, 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		var victim = other.gameObject.GetComponent<Victim>();
		if (victim != null)
		{
			victim.GetStun(stunTimeInSeconds);
			StartCoroutine(DestroyCoroutine());
		}
	}

	private IEnumerator DestroyCoroutine()
	{
		yield return new WaitForSeconds(1);
		NetworkServer.Destroy(gameObject);
	}
}
