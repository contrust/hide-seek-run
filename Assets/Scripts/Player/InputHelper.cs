using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class InputHelper : MonoBehaviour
	{	
		public InputSystemAsset InputActionAsset;
		
		private static readonly Dictionary<InputAction, KeyCode> DefaultControls = new Dictionary<InputAction, KeyCode>
		{
			[InputAction.Forward] = KeyCode.W,
			[InputAction.Back] = KeyCode.S,
			[InputAction.Left] = KeyCode.A,
			[InputAction.Right] = KeyCode.D,
			[InputAction.Jump] = KeyCode.Space,
			[InputAction.Shot] = KeyCode.Mouse0,
			[InputAction.Walk] = KeyCode.LeftShift,
			[InputAction.Interact] = KeyCode.E,
		};

		private static readonly Dictionary<InputAction, KeyCode> CurrentControls = new Dictionary<InputAction, KeyCode>();

		public static void SaveControls()
		{
			foreach (InputAction inputAction in Enum.GetValues(typeof(InputAction)))
			{
				PlayerPrefs.SetInt(inputAction.ToString(), (int) CurrentControls[inputAction]);
			}
			PlayerPrefs.Save();
		}

		public static void LoadControls()
		{
			foreach (InputAction inputAction in Enum.GetValues(typeof(InputAction)))
			{
				if (PlayerPrefs.HasKey(inputAction.ToString()))
					CurrentControls[inputAction] = (KeyCode) PlayerPrefs.GetInt(inputAction.ToString());
				else
					CurrentControls[inputAction] = DefaultControls[inputAction];
			}
		}

		public static bool GetKey(InputAction inputAction) => Input.GetKey(CurrentControls[inputAction]);
		public static bool GetKeyDown(InputAction inputAction) => Input.GetKeyDown(CurrentControls[inputAction]);
	}

	public enum InputAction
	{
		Forward,
		Back,
		Left,
		Right,
		Jump,
		Shot,
		Walk,
		Interact,
		// DarkVision - может быть полезно, если охотник сможет с чем то взаимодействовать
	}
}
