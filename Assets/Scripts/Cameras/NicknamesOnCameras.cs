using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cameras
{
	public class NicknamesOnCameras : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI[] nicknameFields;

		public void UpdateNicknames(List<Victim> victims)
		{
			for (int i = 0; i < nicknameFields.Length; i++)
			{
				nicknameFields[i].text = victims.Count > i ? victims[i].steamName : "";
			}
		}

		public void ClearNicknames()
		{
			foreach (var nick in nicknameFields)
			{
				nick.text = "";
			}
		}
	}
}
