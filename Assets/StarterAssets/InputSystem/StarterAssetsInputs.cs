using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool changeCameraMode;
		public bool shot;
		public bool slap;
		public bool showPhone;
		public bool showCursor;
		public bool nextCamera;
		public bool previousCamera;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")] 
		private Slider slider;
		public float mouseSensitivity;
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		
		void Start()
		{
			slider = GameObject.FindWithTag("SensSlider").GetComponent<Slider>();
			mouseSensitivity = slider.value;

			slider.onValueChanged.AddListener((v) =>
			{
				mouseSensitivity = v;
			});
		}
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnChangeCameraMode(InputValue value)
		{
			ChangeCameraModeInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		
		public void OnShowPhone(InputValue value)
		{
			ShowPhoneInput(value.isPressed);
		}

		public void OnShowCursor(InputValue value)
		{
			ShowCursorInput(value.isPressed);
		}
		
		public void OnSlap(InputValue value)
		{
			SlapInput(value.isPressed);
		}

		public void OnNextCamera(InputValue value)
		{
			NextCameraInput(value.isPressed);
		}
		
		public void OnPreviousCamera(InputValue value)
		{
			PreviousCameraInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection * mouseSensitivity;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ChangeCameraModeInput(bool newChangeCameraModeState)
		{
			changeCameraMode = newChangeCameraModeState;
		}

		public void ShootInput(bool shot)
		{
			this.shot = shot;
		}
		
		public void ShowPhoneInput(bool newShowPhoneState)
		{
			showPhone = newShowPhoneState;
		}

		public void ShowCursorInput(bool newShowCursorState)
		{
			showCursor = newShowCursorState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}
		
		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void SlapInput(bool slapped)
		{
			slap = slapped;
		}
		
		private void NextCameraInput(bool next)
		{
			nextCamera = next;
		}
		
		private void PreviousCameraInput(bool previous)
		{
			previousCamera = previous;
		}
	}
	
}