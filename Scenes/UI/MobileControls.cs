using Godot;
using System;

namespace Demonteks.UI
{
	/// <summary>
	/// Mobile touch controls UI for on-screen buttons and virtual joystick.
	/// Automatically shows/hides based on platform detection.
	/// </summary>
	public partial class MobileControls : CanvasLayer
	{
		#region Export Variables
		[ExportCategory("Control References")]
		[Export] public NodePath JoystickPath { get; set; }
		[Export] public NodePath JumpButtonPath { get; set; }
		[Export] public NodePath AttackButtonPath { get; set; }
		[Export] public NodePath CrouchButtonPath { get; set; }
		
		[ExportCategory("Settings")]
		[Export] public bool ShowControls { get; set; } = true;
		[Export] public float JoystickDeadzone { get; set; } = 0.2f;
		[Export] public float Opacity { get; set; } = 0.6f;
		#endregion

		#region Private Variables
		private TouchScreenButton _jumpButton;
		private TouchScreenButton _attackButton;
		private TouchScreenButton _crouchButton;
		private Control _joystick;
		private Sprite2D _joystickBase;
		private Sprite2D _joystickKnob;
		
		private Vector2 _joystickDirection = Vector2.Zero;
		private Vector2 _joystickCenterPos = Vector2.Zero;
		private int _joystickTouchIndex = -1;
		private float _joystickRadius = 50f;
		
		private bool _jumpPressed = false;
		private bool _attackPressed = false;
		private bool _crouchPressed = false;
		
		private Node2D _player;
		private bool _isMobilePlatform = false;
		#endregion

		#region Signals
		[Signal] public delegate void JoystickMovedEventHandler(Vector2 direction);
		[Signal] public delegate void JumpPressedEventHandler();
		[Signal] public delegate void AttackPressedEventHandler();
		[Signal] public delegate void CrouchPressedEventHandler();
		[Signal] public delegate void CrouchReleasedEventHandler();
		#endregion

		#region Godot Lifecycle
		public override void _Ready()
		{
			// Detect platform
			DetectPlatform();
			
			// Get control references
			if (JoystickPath != null)
				_joystick = GetNode<Control>(JoystickPath);
			
			if (JumpButtonPath != null)
				_jumpButton = GetNode<TouchScreenButton>(JumpButtonPath);
			
			if (AttackButtonPath != null)
				_attackButton = GetNode<TouchScreenButton>(AttackButtonPath);
			
			if (CrouchButtonPath != null)
				_crouchButton = GetNode<TouchScreenButton>(CrouchButtonPath);
			
			// Setup joystick
			if (_joystick != null)
			{
				_joystickBase = _joystick.GetNode<Sprite2D>("Base");
				_joystickKnob = _joystick.GetNode<Sprite2D>("Knob");
				_joystickCenterPos = _joystick.Position;
			}
			
			// Connect button signals
			if (_jumpButton != null)
			{
				_jumpButton.Pressed += OnJumpPressed;
				_jumpButton.Released += OnJumpReleased;
			}
			
			if (_attackButton != null)
			{
				_attackButton.Pressed += OnAttackPressed;
				_attackButton.Released += OnAttackReleased;
			}
			
			if (_crouchButton != null)
			{
				_crouchButton.Pressed += OnCrouchPressed;
				_crouchButton.Released += OnCrouchReleased;
			}
			
			// Set opacity
			//TODO make this work
			//Modulate = new Color(1, 1, 1, Opacity);
			
			// Show/hide based on platform
			Visible = _isMobilePlatform && ShowControls;
			
			GD.Print($"MobileControls initialized. Platform: {OS.GetName()}, Visible: {Visible}");
		}

		public override void _Process(double delta)
		{
			if (!_isMobilePlatform || !Visible) return;
			
			// Find player if not already found
			if (_player == null)
			{
				_player = GetTree().GetFirstNodeInGroup("player") as Node2D;
			}
			
			// Send input to player
			if (_player != null && _player.HasMethod("SetMobileInput"))
			{
				_player.Call("SetMobileInput", _joystickDirection, _jumpPressed, _attackPressed, _crouchPressed);
			}
			
			// Reset single-frame inputs
			_jumpPressed = false;
			_attackPressed = false;
		}

		public override void _Input(InputEvent @event)
		{
			if (!_isMobilePlatform || !Visible || _joystick == null) return;
			
			// Handle virtual joystick
			if (@event is InputEventScreenTouch touchEvent)
			{
				HandleJoystickTouch(touchEvent);
			}
			else if (@event is InputEventScreenDrag dragEvent)
			{
				HandleJoystickDrag(dragEvent);
			}
		}
		#endregion

		#region Platform Detection
		private void DetectPlatform()
		{
			string os = OS.GetName();
			_isMobilePlatform = (os == "Android" || os == "iOS");
			
			GD.Print($"Platform detected: {os}, Mobile: {_isMobilePlatform}");
		}
		#endregion

		#region Virtual Joystick
		private void HandleJoystickTouch(InputEventScreenTouch touchEvent)
		{
			Vector2 touchPos = touchEvent.Position;
			
			if (touchEvent.Pressed)
			{
				// Check if touch is within joystick area
				float distanceToJoystick = touchPos.DistanceTo(_joystickCenterPos);
				
				if (distanceToJoystick <= _joystickRadius * 2)
				{
					_joystickTouchIndex = touchEvent.Index;
					UpdateJoystickKnob(touchPos);
				}
			}
			else if (touchEvent.Index == _joystickTouchIndex)
			{
				// Release joystick
				_joystickTouchIndex = -1;
				_joystickDirection = Vector2.Zero;
				
				if (_joystickKnob != null)
				{
					_joystickKnob.Position = Vector2.Zero;
				}
				
				EmitSignal(SignalName.JoystickMoved, Vector2.Zero);
			}
		}

		private void HandleJoystickDrag(InputEventScreenDrag dragEvent)
		{
			if (dragEvent.Index != _joystickTouchIndex) return;
			
			UpdateJoystickKnob(dragEvent.Position);
		}

		private void UpdateJoystickKnob(Vector2 touchPos)
		{
			Vector2 direction = touchPos - _joystickCenterPos;
			
			// Clamp to joystick radius
			if (direction.Length() > _joystickRadius)
			{
				direction = direction.Normalized() * _joystickRadius;
			}
			
			// Apply deadzone
			if (direction.Length() < _joystickRadius * JoystickDeadzone)
			{
				direction = Vector2.Zero;
			}
			
			// Update knob position
			if (_joystickKnob != null)
			{
				_joystickKnob.Position = direction;
			}
			
			// Calculate normalized direction
			_joystickDirection = direction / _joystickRadius;
			
			EmitSignal(SignalName.JoystickMoved, _joystickDirection);
		}
		#endregion

		#region Button Handlers
		private void OnJumpPressed()
		{
			_jumpPressed = true;
			EmitSignal(SignalName.JumpPressed);
		}

		private void OnJumpReleased()
		{
			// Jump is handled as a press event
		}

		private void OnAttackPressed()
		{
			_attackPressed = true;
			EmitSignal(SignalName.AttackPressed);
		}

		private void OnAttackReleased()
		{
			// Attack is handled as a press event
		}

		private void OnCrouchPressed()
		{
			_crouchPressed = true;
			EmitSignal(SignalName.CrouchPressed);
		}

		private void OnCrouchReleased()
		{
			_crouchPressed = false;
			EmitSignal(SignalName.CrouchReleased);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Show the mobile controls
		/// </summary>
		public new void Show()
		{
			if (_isMobilePlatform)
			{
				Visible = true;
				ShowControls = true;
			}
		}

		/// <summary>
		/// Hide the mobile controls
		/// </summary>
		public new void Hide()
		{
			Visible = false;
			ShowControls = false;
		}

		/// <summary>
		/// Toggle visibility
		/// </summary>
		public void Toggle()
		{
			if (Visible)
				Hide();
			else
				Show();
		}

		/// <summary>
		/// Set control opacity
		/// </summary>
		public void SetOpacity(float opacity)
		{
			Opacity = Mathf.Clamp(opacity, 0.1f, 1.0f);
			//TODO make this work
			//Modulate = new Color(1, 1, 1, Opacity);
		}

		/// <summary>
		/// Set joystick deadzone
		/// </summary>
		public void SetDeadzone(float deadzone)
		{
			JoystickDeadzone = Mathf.Clamp(deadzone, 0.1f, 0.5f);
		}
		#endregion

		#region Properties
		public Vector2 JoystickDirection => _joystickDirection;
		public bool IsJumpPressed => _jumpPressed;
		public bool IsAttackPressed => _attackPressed;
		public bool IsCrouchPressed => _crouchPressed;
		public bool IsMobilePlatform => _isMobilePlatform;
		#endregion
	}
}
