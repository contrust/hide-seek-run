using System;
using UnityEngine;
using Mirror;
using UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class FirstPersonController : NetworkBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 6.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 2.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;

		private bool GroundedBefore = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		public UnityEvent onJump;
		public UnityEvent onGround;
		public UnityEvent onMoving;
		public UnityEvent onStopMoving;
		public AudioSource JumpingSound;
		public AudioSource GroundingSound;
		public AudioSource FootstepsSound;

		// cinemachine
		public float cinemachineTargetPitch;

		// player
		private float speed;
		private float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;

		// timeout deltatime
		private float jumpTimeoutDelta;
		private float fallTimeoutDelta;

		
		private PlayerInput playerInput;
		private CharacterController controller;
		[SerializeField] private StarterAssetsInputs input;
		private GameObject mainCamera;

		private const float Threshold = 0.01f;
		
		//key bindings
		[SerializeField] private KeyCode showPauseKey = KeyCode.Escape;

		[SerializeField] private UIController uiController;



		private bool IsCurrentDeviceMouse
		{
			get
			{
				return playerInput.currentControlScheme == "KeyboardMouse";
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			controller = GetComponent<CharacterController>();
			input = GetComponent<StarterAssetsInputs>();
			playerInput = GetComponent<PlayerInput>();
			uiController = GameObject.FindWithTag("UIHelper").GetComponent<UIController>();

			// reset our timeouts on start
			jumpTimeoutDelta = JumpTimeout;
			fallTimeoutDelta = FallTimeout;
			onJump.AddListener(PlayJumpingSound);
			onGround.AddListener(PlayGroundingSound);
			onMoving.AddListener(PlayFootstepsSound);
			onStopMoving.AddListener(StopPlayingFootstepsSound);
		}

		private void Update()
		{
			if(!controller.enabled) return;
			JumpAndGravity();
			GroundedCheck();
			Move();
			PauseMenu();
		}

		private void LateUpdate()
		{
			if(!controller.enabled || Cursor.visible) return;
			Debug.Log("Cursor !visible");
			CameraRotation();
		}

		private void PauseMenu()
		{
			if (Input.GetKeyDown(showPauseKey))
			{
				uiController.Pause();
			}
		}
		
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			GroundedBefore = Grounded;
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			if (Grounded && !GroundedBefore)
			{
				onGround.Invoke();
			}
		}

		private void CameraRotation()
		{
			// if there is an input
			if (input.look.sqrMagnitude >= Threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				cinemachineTargetPitch += input.look.y * RotationSpeed * deltaTimeMultiplier;
				rotationVelocity = input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
				onStopMoving.Invoke();
			}
			else
			{
				onMoving.Invoke();
			}

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * input.move.x + transform.forward * input.move.y;
			}

			// move the player
			controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (input.jump && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					onJump.Invoke();
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		private void PlayJumpingSound()
		{
			if (!input.sprint && !JumpingSound.isPlaying)
			{
				PlayJumpingSoundCommand();
			}
		}

		[Command]
		private void PlayJumpingSoundCommand()
		{
			PlayJumpingSoundRpc();
		}

		[ClientRpc]
		private void PlayJumpingSoundRpc()
		{
			JumpingSound.Play();
		}

		private void PlayGroundingSound()
		{
			if (!input.sprint && !GroundingSound.isPlaying)
			{
				PlayGroundingSoundCommand();
			}
		}

		[Command]
		private void PlayGroundingSoundCommand()
		{
			PlayGroundingSoundRpc();
		}

		[ClientRpc]
		private void PlayGroundingSoundRpc()
		{
			GroundingSound.Play();
		}

		private void PlayFootstepsSound()
		{
			if (!input.sprint && Grounded)
			{
				if (!FootstepsSound.isPlaying)
				{
					PlayFootstepsSoundCommand();	
				}
			}
			else
			{
				StopPlayingFootstepsSoundCommand();
			}
		}

		[Command]
		private void PlayFootstepsSoundCommand()
		{
			PlayFootstepsSoundRpc();
		}

		[ClientRpc]
		private void PlayFootstepsSoundRpc()
		{
			FootstepsSound.Play();
		}

		private void StopPlayingFootstepsSound()
		{
			StopPlayingFootstepsSoundCommand();
		}

		[Command]
		private void StopPlayingFootstepsSoundCommand()
		{
			StopPlayingFootstepsSoundRpc();
		}

		[ClientRpc]
		private void StopPlayingFootstepsSoundRpc()
		{
			FootstepsSound.Stop();
		}

		public void AddForce(float force)
		{
			verticalVelocity += force;
		}
	}
}