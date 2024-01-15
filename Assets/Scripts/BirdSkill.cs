using UnityEngine;

public class BirdSkill : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Transform raycastPoint;
	[SerializeField] private float distanceToObjectForVerticalFly = 2f;
	[SerializeField] private Transform view;
	[SerializeField] private float speed;
	[SerializeField]
	private LayerMask layerMask = Physics.DefaultRaycastLayers;


	private void Update()
	{
		if (target)
		{
			Follow();
		}
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
}
