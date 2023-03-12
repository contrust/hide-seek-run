using System;
using Mirror;
using UnityEngine;

namespace Transport.Player
{
	public class eyesTODELETE : NetworkBehaviour
	{
		public GameObject[] eyes;


		private void Start()
		{
			if (isLocalPlayer)
				foreach (var e in eyes)
				{
					e.layer = LayerMask.NameToLayer("IgnoreCamera");
				}
		}

	}
}
