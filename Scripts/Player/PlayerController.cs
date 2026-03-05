using Godot;
using System;

namespace Demonteks.Player
{
	/// <summary>
	/// Main player controller handling movement, jumping, and input.
	/// Inspired by Levrault's state machine approach with modern C# patterns.
	/// Supports both desktop (WASD) and mobile (touch) controls.
	/// </summary>
	public partial class PlayerController : CharacterBody2D
	{
		#region Export Variables
		[ExportCategory("Movement")]
		[Export] public float MoveSpeed { get; set; } = 200f;
		[Export] public float RunMultiplier { get; set; } = 1.5f;
		[Export] public float JumpVelocity { get; set; } = -400f;
		[Export] public float Gravity { get; set; } = 980f;
		[Export] public float CoyoteTime { get; set; } = 0.1f; // Time after leaving platform where jump is still valid
		[Export] public float JumpBufferTime { get; set; } = 0.15f; // Input buffer for jump
		[Export] public int MaxJumps { get; set; } = 2; // Number of jumps allowed (1 = single jump, 2 = double jump)

		[ExportCategory("Combat")]
		[Export] public float AttackCooldown { get; set; } = 0.5f;
		[Export] public NodePath AnimatedSpritePath { get; set; }
		[Export] public NodePath CollisionShapePath { get; set; }

		[ExportCategory("Mobile Controls")]
		[Export] public bool IsMobileMode { get; set; } = false;
		#endregion

		#region Private Variables
		private AnimatedSprite2D _animatedSprite;
		private CollisionShape2D _collisionShape;
		private PlayerStats _stats;
		private PlayerCombat _combat;
		private PlayerStateMachine _stateMachine;
		private Management.GameManager _gameManager;

		// Movement tracking
		private Vector2 _velocity = Vector2.Zero;
		private float _coyoteTimer = 0f;
		private float _jumpBufferTimer = 0f;
		private bool _isRunning = false;
		private bool _isCrouching = false;
		private int _jumpsRemaining;

		// Input tracking
		private Vector2 _inputDirection = Vector2.Zero;
		private bool _jumpPressed = false;
		private bool _attackPressed = false;
		private bool _crouchPressed = false;

		// Directional facing
		private bool _facingRight = true;

		// Death animation tracking
		private bool _deathAnimationPlayed = false;
		#endregion

		#region Signals
		[Signal] public delegate void PlayerDeathEventHandler();
		[Signal] public delegate void PlayerHitEventHandler(int damage);
		[Signal] public delegate void PlayerAttackEventHandler();
		[Signal] public delegate void StateChangedEventHandler(string newState);
		#endregion

		#region Godot Lifecycle
		public override void _Ready()
		{
			// Get node references
			_animatedSprite = GetNode<AnimatedSprite2D>(AnimatedSpritePath);
			_collisionShape = GetNode<CollisionShape2D>(CollisionShapePath);

			// Initialize components
			_stats = GetNode<PlayerStats>("PlayerStats");
			_combat = GetNode<PlayerCombat>("PlayerCombat");

			// Setup state machine
			_stateMachine = new PlayerStateMachine(this);

			// Connect signals
			_stats.HealthChanged += OnHealthChanged;
			_stats.Died += OnPlayerDied;
			_combat.AttackExecuted += OnAttackExecuted;

			// Detect platform
			DetectPlatform();

			// Register with GameManager
			_gameManager = GetNode<Management.GameManager>("/root/GameManager");
			if (_gameManager != null)
			{
				_gameManager.Player = this;
				// Set initial spawn position as checkpoint
				_gameManager.SetCheckpoint(GlobalPosition);
				GD.Print($"Player registered with GameManager. Spawn checkpoint set at {GlobalPosition}");
			}

			// Initialize jumps
			_jumpsRemaining = MaxJumps;

			GD.Print($"PlayerController initialized. Mobile mode: {IsMobileMode}");
		}

		public override void _PhysicsProcess(double delta)
		{
			float deltaF = (float)delta;

			// When dead: only apply gravity so the body falls to the floor, then freeze
			if (_stats != null && _stats.IsDead)
			{
				if (!IsOnFloor())
				{
					_velocity.Y += Gravity * deltaF;
				}
				else
				{
					_velocity.Y = 0;
					_velocity.X = 0;
				}
				Velocity = _velocity;
				MoveAndSlide();
				UpdateAnimation();
				return;
			}

			// Update timers
			UpdateTimers(deltaF);

			// Process input (reads current input state, does NOT reset single-frame flags)
			ProcessInput();

			// Update state machine (reads _attackPressed / _jumpPressed here)
			_stateMachine.Update(deltaF);

			// Reset single-frame inputs AFTER the state machine has had a chance to read them
			_jumpPressed = false;
			_attackPressed = false;

			// Apply gravity
			if (!IsOnFloor())
			{
				_velocity.Y += Gravity * deltaF;
			}
			else
			{
				_coyoteTimer = CoyoteTime;
				_velocity.Y = 0;
				// Reset jumps when landing
				_jumpsRemaining = MaxJumps;
			}

			// Handle jump buffering
			if (_jumpBufferTimer > 0 && (_coyoteTimer > 0 || IsOnFloor()))
			{
				Jump();
				_jumpBufferTimer = 0;
				_coyoteTimer = 0;
			}

			// Apply movement
			Velocity = _velocity;
			MoveAndSlide();

			// Update animation
			UpdateAnimation();
		}

		public override void _Input(InputEvent @event)
		{
			if (IsMobileMode) return; // Mobile input handled separately

			// Jump input
			if (@event.IsActionPressed("jump"))
			{
				_jumpPressed = true;
				_jumpBufferTimer = JumpBufferTime;
			}

			// Attack input
			if (@event.IsActionPressed("attack"))
			{
				_attackPressed = true;
			}

			// Crouch input
			_crouchPressed = Input.IsActionPressed("crouch");

			// Run input
			_isRunning = Input.IsActionPressed("run");
		}
		#endregion

		#region Input Processing
		private void ProcessInput()
		{
			if (IsMobileMode)
			{
				ProcessMobileInput();
			}
			else
			{
				ProcessDesktopInput();
			}
			// Note: single-frame resets (_jumpPressed, _attackPressed) happen in _PhysicsProcess
			// AFTER _stateMachine.Update(), so the state machine can read them.
		}

		private void ProcessDesktopInput()
		{
			// WASD controls: A=left, D=right, W=jump, S=crouch
			_inputDirection = Vector2.Zero;

			if (Input.IsActionPressed("move_left"))
				_inputDirection.X -= 1;

			if (Input.IsActionPressed("move_right"))
				_inputDirection.X += 1;

			if (Input.IsActionPressed("attack"))
				_attackPressed = true;

			if (Input.IsActionPressed("crouch"))
				_crouchPressed = true;

			_inputDirection = _inputDirection.Normalized();
		}

		private void ProcessMobileInput()
		{
			// Mobile input is set by MobileControls.cs via SetMobileInput method
			// This allows the on-screen controls to directly control the player
		}

		/// <summary>
		/// Called by MobileControls to set input direction
		/// </summary>
		public void SetMobileInput(Vector2 direction, bool jump, bool attack, bool crouch)
		{
			if (!IsMobileMode) return;

			_inputDirection = direction;
			if (jump) _jumpBufferTimer = JumpBufferTime;
			_attackPressed = attack;
			_crouchPressed = crouch;
		}
		#endregion

		#region Movement Methods
		public void Move(float deltaF)
		{
			if (_isCrouching)
			{
				_velocity.X = 0;
				return;
			}

			float targetSpeed = MoveSpeed;
			if (_isRunning) targetSpeed *= RunMultiplier;

			_velocity.X = _inputDirection.X * targetSpeed;

			// Update facing direction
			if (_inputDirection.X > 0)
			{
				_facingRight = true;
				// Ensure combat hitbox faces right
				if (_combat != null) _combat.Scale = new Vector2(1, 1);
			}
			else if (_inputDirection.X < 0)
			{
				_facingRight = false;
				// Ensure combat hitbox faces left
				if (_combat != null) _combat.Scale = new Vector2(-1, 1);
			}
		}

		public void Jump()
		{
			if (!_stats.CanAct || _jumpsRemaining <= 0) return;

			_velocity.Y = JumpVelocity;
			_jumpsRemaining--;
			_coyoteTimer = 0;

			GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("CharacterAudioController")?.PlayAction("Jump");
			GD.Print($"Player jumped! Jumps remaining: {_jumpsRemaining}");
		}

		public void Crouch()
		{
			_isCrouching = true;
			// Modify collision shape for crouching (implementation depends on your setup)
			// _collisionShape.Scale = new Vector2(1, 0.5f);
		}

		public void StandUp()
		{
			_isCrouching = false;
			// _collisionShape.Scale = Vector2.One;
		}
		#endregion

		#region Combat Methods
		public void Attack()
		{
			if (!_stats.CanAct || !_combat.CanAttack) return;

			_combat.ExecuteAttack();
			EmitSignal(SignalName.PlayerAttack);
		}

		public void TakeDamage(int damage)
		{
			_stats.TakeDamage(damage);
			EmitSignal(SignalName.PlayerHit, damage);
		}
		#endregion

		#region Animation
		private void UpdateAnimation()
		{
			if (_animatedSprite == null) return;

			string currentState = _stateMachine.CurrentState.GetType().Name;

			// Determine base flip based on facing direction
			bool shouldFlip = !_facingRight;
			string animToPlay = "idle";
			string fallbackAnim = null;

			// Map state to animation
			switch (currentState)
			{
				case "IdleState":
					animToPlay = "idle";
					break;
				case "MoveState":
					animToPlay = _isRunning ? "run" : "walk";
					break;
				case "JumpState":
					animToPlay = "jump";
					break;
				case "FallState":
					animToPlay = "fall";
					fallbackAnim = "jump"; // fall back to jump if no fall anim
					break;
				case "AttackState":
					animToPlay = _combat?.CurrentAttackAnim ?? "attack";
					fallbackAnim = "attack";

					// Specific fix for machete/alien animations which are drawn backwards in the assets
					if (animToPlay == "attack_machete" || animToPlay == "attack_alien")
					{
						shouldFlip = _facingRight;
					}
					break;
				case "CrouchState":
					animToPlay = "crouch";
					fallbackAnim = "idle"; // fall back to idle if no crouch anim
					break;
				case "HurtState":
					animToPlay = "hurt";
					break;
				case "DeathState":
					// Only play the death animation once and never restart it
					if (!_deathAnimationPlayed)
					{
						PlaySafe("death");
						_deathAnimationPlayed = true;
					}
					// Apply flip even if not actively playing new frame
					_animatedSprite.FlipH = shouldFlip;
					return;
			}

			_animatedSprite.FlipH = shouldFlip;
			PlaySafe(animToPlay, fallbackAnim);
		}

		/// <summary>
		/// Safely play animation with optional fallback
		/// </summary>
		private void PlaySafe(string animName, string fallback = null)
		{
			if (_animatedSprite.SpriteFrames == null) return;
			if (_animatedSprite.SpriteFrames.HasAnimation(animName))
			{
				_animatedSprite.Play(animName);
			}
			else if (fallback != null && _animatedSprite.SpriteFrames.HasAnimation(fallback))
			{
				_animatedSprite.Play(fallback);
			}
		}
		#endregion

		#region Helper Methods
		private void UpdateTimers(float delta)
		{
			if (_coyoteTimer > 0)
				_coyoteTimer -= delta;

			if (_jumpBufferTimer > 0)
				_jumpBufferTimer -= delta;
		}

		private void DetectPlatform()
		{
			// Auto-detect if running on mobile
			string os = OS.GetName();
			IsMobileMode = (os == "Android" || os == "iOS");
		}

		public bool IsMoving()
		{
			return Mathf.Abs(_inputDirection.X) > 0.01f;
		}

		public bool IsFalling()
		{
			return _velocity.Y > 0 && !IsOnFloor();
		}

		public bool IsJumping()
		{
			return _velocity.Y < 0;
		}

		public bool WantsToJump()
		{
			return _jumpBufferTimer > 0;
		}

		public bool CanDoubleJump()
		{
			return _jumpsRemaining > 0;
		}

		public bool WantsToAttack()
		{
			return _attackPressed;
		}

		public bool WantsToCrouch()
		{
			return _crouchPressed;
		}

		/// <summary>
		/// Revive the player after death (called by GameManager)
		/// </summary>
		public void Revive()
		{
			// Reset stats (health, death state, etc.)
			if (_stats != null)
			{
				_stats.Revive();
			}

			// Reset velocity
			_velocity = Vector2.Zero;
			Velocity = Vector2.Zero;

			// Reset state to idle
			if (_stateMachine != null)
			{
				_stateMachine.ChangeState(new IdleState());
			}

			// Reset death animation flag so death can be played again
			_deathAnimationPlayed = false;

			// Reset jumps
			_jumpsRemaining = MaxJumps;

			GD.Print("Player revived!");
		}
		#endregion

		#region Signal Handlers
		private void OnHealthChanged(int currentHealth, int maxHealth)
		{
			GD.Print($"Player health: {currentHealth}/{maxHealth}");
		}

		private void OnPlayerDied()
		{
			EmitSignal(SignalName.PlayerDeath);
			_stateMachine.ChangeState(new DeathState());

			// Tell GameManager to start the respawn timer
			if (_gameManager != null)
				_gameManager.OnPlayerDied();
		}

		private void OnAttackExecuted(string weaponName, int damage)
		{
			GD.Print($"Attack executed with {weaponName} for {damage} damage");
		}
		#endregion

		#region Properties
		public new Vector2 GetVelocity() => _velocity;
		public new void SetVelocity(Vector2 velocity) => _velocity = velocity;
		public Vector2 InputDirection => _inputDirection;
		public bool FacingRight => _facingRight;
		public bool IsCrouching => _isCrouching;
		public PlayerStats Stats => _stats;
		public PlayerCombat Combat => _combat;
		#endregion
	}

	#region State Machine
	/// <summary>
	/// Hierarchical state machine for player states
	/// Based on GDQuest's HSM pattern but adapted for C#
	/// </summary>
	public class PlayerStateMachine
	{
		private PlayerController _player;
		private IPlayerState _currentState;

		public PlayerStateMachine(PlayerController player)
		{
			_player = player;
			_currentState = new IdleState();
			_currentState.Enter(_player);
		}

		public void Update(float delta)
		{
			IPlayerState nextState = _currentState.Update(_player, delta);

			if (nextState != null && nextState != _currentState)
			{
				ChangeState(nextState);
			}
		}

		public void ChangeState(IPlayerState newState)
		{
			_currentState.Exit(_player);
			_currentState = newState;
			_currentState.Enter(_player);

			_player.EmitSignal(PlayerController.SignalName.StateChanged, newState.GetType().Name);
		}

		public IPlayerState CurrentState => _currentState;
	}

	public interface IPlayerState
	{
		void Enter(PlayerController player);
		IPlayerState Update(PlayerController player, float delta);
		void Exit(PlayerController player);
	}

	public class IdleState : IPlayerState
	{
		public void Enter(PlayerController player) { }

		public IPlayerState Update(PlayerController player, float delta)
		{
			if (!player.IsOnFloor()) return new FallState();
			if (player.WantsToJump()) return new JumpState();
			if (player.WantsToAttack()) return new AttackState();
			if (player.WantsToCrouch()) return new CrouchState();
			if (player.IsMoving()) return new MoveState();

			// Apply movement even in idle (allows player to start moving)
			player.Move(delta);
			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class MoveState : IPlayerState
	{
		public void Enter(PlayerController player) { }

		public IPlayerState Update(PlayerController player, float delta)
		{
			if (!player.IsOnFloor()) return new FallState();
			if (player.WantsToJump()) return new JumpState();
			if (player.WantsToAttack()) return new AttackState();
			if (!player.IsMoving()) return new IdleState();

			player.Move(delta);
			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class JumpState : IPlayerState
	{
		public void Enter(PlayerController player)
		{
			player.Jump();
		}

		public IPlayerState Update(PlayerController player, float delta)
		{
			player.Move(delta);

			if (player.IsFalling()) return new FallState();
			if (player.IsOnFloor()) return new IdleState();

			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class FallState : IPlayerState
	{
		public void Enter(PlayerController player) { }

		public IPlayerState Update(PlayerController player, float delta)
		{
			player.Move(delta);

			// Allow double jumping while falling
			if (player.WantsToJump() && player.CanDoubleJump()) return new JumpState();

			if (player.IsOnFloor())
			{
				// Play landing sound
				player.GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("CharacterAudioController")?.PlayAction("Jump");
				return player.IsMoving() ? new MoveState() : (IPlayerState)new IdleState();
			}

			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class AttackState : IPlayerState
	{
		private float _attackDuration = 0.6f;
		private float _timer = 0f;

		public void Enter(PlayerController player)
		{
			player.Attack();
			_timer = 0f;
		}

		public IPlayerState Update(PlayerController player, float delta)
		{
			_timer += delta;

			if (_timer >= _attackDuration)
			{
				return new IdleState();
			}

			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class CrouchState : IPlayerState
	{
		public void Enter(PlayerController player)
		{
			player.Crouch();
		}

		public IPlayerState Update(PlayerController player, float delta)
		{
			if (!player.WantsToCrouch())
			{
				return new IdleState();
			}

			if (player.WantsToAttack())
			{
				return new AttackState();
			}

			return this;
		}

		public void Exit(PlayerController player)
		{
			player.StandUp();
		}
	}

	public class HurtState : IPlayerState
	{
		private float _hurtDuration = 0.5f;
		private float _timer = 0f;

		public void Enter(PlayerController player)
		{
			_timer = 0f;
		}

		public IPlayerState Update(PlayerController player, float delta)
		{
			_timer += delta;

			if (_timer >= _hurtDuration)
			{
				return new IdleState();
			}

			return this;
		}

		public void Exit(PlayerController player) { }
	}

	public class DeathState : IPlayerState
	{
		public void Enter(PlayerController player)
		{
			player.SetVelocity(Vector2.Zero);
		}

		public IPlayerState Update(PlayerController player, float delta)
		{
			// Stay in death state
			return this;
		}

		public void Exit(PlayerController player) { }
	}
	#endregion
}
