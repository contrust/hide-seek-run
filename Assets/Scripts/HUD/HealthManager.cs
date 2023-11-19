using System;
using System.Collections;
using System.Linq;
using Player;
using UnityEngine;

namespace HUD
{
	public class HealthManager : MonoBehaviour
	{
		[SerializeField] private Health MainBar;
		[SerializeField] private Health[] friendBars;
		[SerializeField] private Sprite[] playerImages;

		public void Setup()
		{
			StartCoroutine(Wait());
		}

		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(1f);
			SetupHealthBars();
		}

		private void SetupHealthBars()
		{
			var victims = FindObjectsOfType<Victim>();

			var i = 0;
			foreach (var victim in victims)
			{
				if (victim.isLocalPlayer)
					MainBar.SetVictim(victim, playerImages[(int)victim.color]);
				else
					friendBars[i++].SetVictim(victim, playerImages[(int)victim.color]);
			}
		}
	}
}
