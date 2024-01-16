using System.Linq;
using UnityEngine;

namespace DefaultNamespace.SymbolInserter
{
	public class Heal : MonoBehaviour
	{
		private Transform target;
		public int healCount = 20;
		
		private void FindTarget()
		{
			var victims = FindObjectsOfType<Victim>().Select(x => x.transform).ToArray();

			var minDist = float.MaxValue;
			Transform targetVictim = null;

			foreach (Transform victim in victims)
			{
				var dist = Vector3.Distance(victim.position, transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					targetVictim = victim;
				}
			}
			target = targetVictim;
		}

		public void HealVictim()
		{
			FindTarget();
			target.GetComponent<Victim>().Health += healCount;
		}
	}
}
