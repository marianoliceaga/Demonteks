using Godot;
using System;

namespace Demonteks.Enemy
{
    /// <summary>
    /// AI controller for Demontek enemies with patrol, chase, and attack behaviors.
    /// Implements FSM pattern for clean state management.
    /// </summary>
    public partial class DemontekAI : CharacterBody2D
    {
        #region Export Variables
        [ExportCategory("Enemy Type")]
        [Export] public EnemyType Type { get; set; } = EnemyType.Normal;

        [ExportCategory("Movement")]
        [Export] public float PatrolSpeed { get; set; } = 50f;
        [Export] public float ChaseSpeed { get; set; } = 150f;
        [Export] public float DetectionRange { get; set; } = 200f;
        [Export] public float AttackRange { get; set; } = 70f;
        [Export] public float PatrolDistance { get; set; } = 200f;

        [ExportCategory("Combat")]
        [Export] public int MaxHealth { get; set; } = 20;
        [Export] public int Damage { get; set; } = 10;
        [Export] public float AttackCooldown { get; set; } = 0.6f;
        [Export] public float Gravity { get; set; } = 980f;

        [ExportCategory("References")]
        [Export] public NodePath AnimatedSpritePath { get; set; }
        [Export] public NodePath DetectionAreaPath { get; set; }
        [Export] public NodePath AttackAreaPath { get; set; }
        [Export] public NodePath FloorRayPath { get; set; }

        [ExportCategory("Void Detection")]
        [Export] public float VoidThreshold { get; set; } = 1000f; // Y position below which to respawn
        #endregion

        #region Private Variables
        private AnimatedSprite2D _animatedSprite;
        private Area2D _detectionArea;
        private Area2D _attackArea;
        private RayCast2D _floorRay;

        private int _currentHealth;
        private float _attackCooldownTimer = 0f;
        private bool _isDead = false;
        private bool _canAct = true;

        // Movement
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _startPosition;
        private bool _facingRight = true;

        // AI State Machine
        private EnemyStateMachine _stateMachine;
        private Node2D _target; // Player reference

        // Patrol
        private float _patrolDirection = 1f;
        private float _patrolDistanceTraveled = 0f;
        #endregion

        #region Signals
        [Signal] public delegate void DiedEventHandler();
        [Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
        [Signal] public delegate void AttackedPlayerEventHandler(int damage);
        [Signal] public delegate void PlayerDetectedEventHandler();
        [Signal] public delegate void PlayerLostEventHandler();
        #endregion

        #region Godot Lifecycle
        public override void _Ready()
        {
            // Get references
            _animatedSprite = GetNode<AnimatedSprite2D>(AnimatedSpritePath);
            _detectionArea = GetNode<Area2D>(DetectionAreaPath);
            _attackArea = GetNode<Area2D>(AttackAreaPath);

            // Setup floor ray for edge detection
            if (FloorRayPath != null)
            {
                _floorRay = GetNode<RayCast2D>(FloorRayPath);
                _floorRay.Enabled = true;
                _floorRay.CollisionMask = 1; // World layer
            }

            // Initialize stats based on type
            InitializeByType();

            _currentHealth = MaxHealth;
            _startPosition = GlobalPosition;

            // Setup state machine
            _stateMachine = new EnemyStateMachine(this);
            _stateMachine.ChangeState(new PatrolState());

            // Connect detection areas
            _detectionArea.BodyEntered += OnDetectionAreaEntered;
            _detectionArea.BodyExited += OnDetectionAreaExited;
            _attackArea.BodyEntered += OnAttackAreaEntered;

            // Add to enemies group
            AddToGroup("enemies");

            GD.Print($"{Type} Demontek initialized. Health: {MaxHealth}, Damage: {Damage}");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_isDead) return;

            float deltaF = (float)delta;

            // Update cooldowns
            if (_attackCooldownTimer > 0)
                _attackCooldownTimer -= deltaF;

            // Apply gravity
            if (!IsOnFloor())
            {
                _velocity.Y += Gravity * deltaF;
            }
            else
            {
                _velocity.Y = 0;
            }

            // Check for void - respawn if fallen too far
            if (GlobalPosition.Y > VoidThreshold)
            {
                Respawn();
                return; // Skip rest of physics frame
            }

            // Update state machine
            _stateMachine.Update(deltaF);

            // Apply movement
            Velocity = _velocity;

            // Check for collision with player specifically to apply a small pushback
            bool hitPlayer = false;
            Vector2 pushDir = Vector2.Zero;

            MoveAndSlide();

            // Post-move collision check to gently push enemies away from the player
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Node2D collider && collider.IsInGroup("player"))
                {
                    hitPlayer = true;
                    // Push direction is from collider to this enemy
                    pushDir = GlobalPosition - collider.GlobalPosition;

                    // If they are stacked perfectly on top of each other
                    if (pushDir.LengthSquared() < 0.1f)
                    {
                        pushDir = new Vector2(_facingRight ? 1f : -1f, 0);
                    }
                    else
                    {
                        pushDir = pushDir.Normalized();
                    }
                    break;
                }
            }

            if (hitPlayer)
            {
                // Push the enemy back slightly to create space for the player
                GlobalPosition += new Vector2(pushDir.X * 1.5f, 0);
            }

            // Update animation
            UpdateAnimation();
        }
        #endregion

        #region Type Initialization
        private void InitializeByType()
        {
            switch (Type)
            {
                case EnemyType.Normal:
                    MaxHealth = 20;
                    Damage = 10;
                    ChaseSpeed = 140f;
                    DetectionRange = 200f;
                    break;

                case EnemyType.Guard:
                    MaxHealth = 25;
                    Damage = 10;
                    ChaseSpeed = 120f;
                    DetectionRange = 250f;
                    PatrolDistance = 150f;
                    break;

                case EnemyType.Mutant:
                    MaxHealth = 50;
                    Damage = 15;
                    ChaseSpeed = 160f;
                    DetectionRange = 220f;
                    AttackCooldown = 0.6f;
                    break;

                case EnemyType.Goliath:
                    MaxHealth = 200;
                    Damage = 25;
                    ChaseSpeed = 100f;
                    DetectionRange = 300f;
                    AttackCooldown = 0.8f;
                    break;

                case EnemyType.King:
                    MaxHealth = 500;
                    Damage = 35;
                    ChaseSpeed = 130f;
                    DetectionRange = 350f;
                    AttackCooldown = 0.5f;
                    break;
            }
        }
        #endregion

        #region Movement
        public void Patrol(float delta)
        {
            if (!_canAct) return;

            // Move in patrol direction
            _velocity.X = _patrolDirection * PatrolSpeed;
            _patrolDistanceTraveled += Mathf.Abs(_velocity.X * delta);

            // Check for edge using raycast
            bool edgeAhead = false;
            if (_floorRay != null)
            {
                _floorRay.ForceRaycastUpdate();
                edgeAhead = !_floorRay.IsColliding();
            }

            // Turn around at patrol distance or edges
            if (_patrolDistanceTraveled >= PatrolDistance || edgeAhead)
            {
                TurnAround();
                _patrolDistanceTraveled = 0;
            }

            UpdateFacing();
        }

        public void ChaseTarget(float delta)
        {
            if (!_canAct || _target == null) return;

            float direction = Mathf.Sign(_target.GlobalPosition.X - GlobalPosition.X);
            _velocity.X = direction * ChaseSpeed;

            UpdateFacing();
        }

        public void MoveTowards(Vector2 targetPosition, float speed)
        {
            if (!_canAct) return;

            float direction = Mathf.Sign(targetPosition.X - GlobalPosition.X);
            _velocity.X = direction * speed;

            UpdateFacing();
        }

        public void StopMoving()
        {
            _velocity.X = 0;
        }

        private void TurnAround()
        {
            _patrolDirection *= -1;
        }

        private void UpdateFacing()
        {
            // Do not override with horizontal flipping since we have dynamic directional animations now
            // We just track facing direction internally
            if (_velocity.X > 0 && !_facingRight)
            {
                _facingRight = true;
            }
            else if (_velocity.X < 0 && _facingRight)
            {
                _facingRight = false;
            }
        }
        #endregion

        #region Combat
        public void Attack()
        {
            if (_attackCooldownTimer > 0 || _target == null) return;

            // Face the target before attacking (update direction flag only, let animation handle visuals)
            float direction = Mathf.Sign(_target.GlobalPosition.X - GlobalPosition.X);
            if (direction > 0)
            {
                _facingRight = true;
                _animatedSprite.FlipH = false; // reset flip for directional anims
            }
            else if (direction < 0)
            {
                _facingRight = false;
                _animatedSprite.FlipH = false; // reset flip for directional anims
            }

            _attackCooldownTimer = AttackCooldown;
            EmitSignal(SignalName.AttackedPlayer, Damage);

            // Apply damage to player
            if (_target.HasMethod("TakeDamage"))
            {
                _target.Call("TakeDamage", Damage);
            }

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("CharacterAudioController")?.PlayAction("Attack");

            GD.Print($"{Type} Demontek attacked for {Damage} damage!");
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;

            _currentHealth -= damage;
            EmitSignal(SignalName.HealthChanged, _currentHealth, MaxHealth);

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("CharacterAudioController")?.PlayAction("Hurt");

            GD.Print($"{Type} Demontek took {damage} damage. Health: {_currentHealth}/{MaxHealth}");

            // Visual feedback - Flash Red
            if (_animatedSprite != null)
            {
                var tween = CreateTween();
                tween.TweenProperty(_animatedSprite, "modulate", Colors.Red, 0.1f);
                tween.TweenProperty(_animatedSprite, "modulate", Colors.White, 0.1f);
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Enter hurt state briefly
                _stateMachine.ChangeState(new HurtState());
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _canAct = false;
            _velocity = Vector2.Zero;

            EmitSignal(SignalName.Died);
            _stateMachine.ChangeState(new DeathState());

            // Record enemy kill in GameManager
            var gameManager = GetNode<Management.GameManager>("/root/GameManager");
            gameManager?.RecordEnemyKilled();

            // Remove collision
            SetCollisionLayerValue(2, false); // Disable Enemy layer
            SetCollisionMaskValue(1, false); // Disable World collision

            // Queue free after death animation
            GetTree().CreateTimer(2.0f).Timeout += QueueFree;

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("CharacterAudioController")?.PlayAction("Death");

            GD.Print($"{Type} Demontek died!");
        }

        /// <summary>
        /// Respawn enemy at starting position with health intact (called when falling into void)
        /// </summary>
        public void Respawn()
        {
            if (_isDead) return;

            GlobalPosition = _startPosition;
            _velocity = Vector2.Zero;

            _patrolDirection = 1f;
            _patrolDistanceTraveled = 0f;
            _target = null;

            _stateMachine.ChangeState(new PatrolState());

            GD.Print($"{Type} Demontek respawned at {_startPosition}. Health: {_currentHealth}/{MaxHealth}");
        }
        #endregion

        #region Detection
        private void OnDetectionAreaEntered(Node2D body)
        {
            if (body.IsInGroup("player"))
            {
                _target = body;
                EmitSignal(SignalName.PlayerDetected);

                if (_stateMachine.CurrentState is PatrolState)
                {
                    _stateMachine.ChangeState(new ChaseState());
                }
            }
        }

        private void OnDetectionAreaExited(Node2D body)
        {
            if (body == _target)
            {
                EmitSignal(SignalName.PlayerLost);

                if (_stateMachine.CurrentState is ChaseState)
                {
                    _stateMachine.ChangeState(new ReturnState());
                }
            }
        }

        private void OnAttackAreaEntered(Node2D body)
        {
            if (body == _target && _stateMachine.CurrentState is ChaseState)
            {
                _stateMachine.ChangeState(new AttackState());
            }
        }
        #endregion

        #region Animation
        private void UpdateAnimation()
        {
            if (_animatedSprite == null || _isDead) return;

            string currentState = _stateMachine.CurrentState.GetType().Name;

            // Start by assuming no flip, we will apply it if a generic animation is used
            _animatedSprite.FlipH = false;

            string suffix = _facingRight ? "east" : "west";
            string animPlayed = "";

            switch (currentState)
            {
                case "IdleState":
                    animPlayed = PlaySafe($"idle_{suffix}", "idle");
                    break;
                case "PatrolState":
                    animPlayed = PlaySafe($"walk_{suffix}", "walk");
                    break;
                case "ChaseState":
                    animPlayed = PlaySafe($"run_{suffix}", $"walk_{suffix}", "run", "walk"); // fallback chain
                    break;
                case "AttackState":
                    animPlayed = PlaySafe($"attack_{suffix}", "attack");
                    break;
                case "HurtState":
                    animPlayed = PlaySafe($"hurt_{suffix}", "hurt");
                    break;
                case "DeathState":
                    animPlayed = PlaySafe($"death_{suffix}", "death");
                    break;
            }

            // If a generic animation was played (no direct suffix match), flip the sprite manually
            if (!string.IsNullOrEmpty(animPlayed) && !animPlayed.EndsWith($"_{suffix}"))
            {
                _animatedSprite.FlipH = !_facingRight;
            }
        }

        private string PlaySafe(params string[] animNames)
        {
            if (_animatedSprite.SpriteFrames == null) return string.Empty;

            foreach (var animName in animNames)
            {
                if (animName != null && _animatedSprite.SpriteFrames.HasAnimation(animName))
                {
                    _animatedSprite.Play(animName);
                    return animName;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Properties
        public Vector2 StartPosition => _startPosition;
        public bool CanAttack => _attackCooldownTimer <= 0;
        public Node2D Target => _target;
        public bool IsDead => _isDead;
        public float DistanceToTarget => _target != null ? GlobalPosition.DistanceTo(_target.GlobalPosition) : float.MaxValue;
        public bool IsNearTarget => _target != null && DistanceToTarget <= AttackRange;
        public bool CanSeeTarget => _target != null && DistanceToTarget <= DetectionRange;
        #endregion
    }

    #region Enemy State Machine
    public class EnemyStateMachine
    {
        private DemontekAI _enemy;
        private IEnemyState _currentState;

        public EnemyStateMachine(DemontekAI enemy)
        {
            _enemy = enemy;
        }

        public void Update(float delta)
        {
            if (_currentState != null)
            {
                IEnemyState nextState = _currentState.Update(_enemy, delta);

                if (nextState != null && nextState != _currentState)
                {
                    ChangeState(nextState);
                }
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            _currentState?.Exit(_enemy);
            _currentState = newState;
            _currentState.Enter(_enemy);
        }

        public IEnemyState CurrentState => _currentState;
    }

    public interface IEnemyState
    {
        void Enter(DemontekAI enemy);
        IEnemyState Update(DemontekAI enemy, float delta);
        void Exit(DemontekAI enemy);
    }

    public class IdleState : IEnemyState
    {
        public void Enter(DemontekAI enemy)
        {
            enemy.StopMoving();
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            if (enemy.CanSeeTarget)
                return new ChaseState();

            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class PatrolState : IEnemyState
    {
        public void Enter(DemontekAI enemy) { }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            if (enemy.CanSeeTarget)
                return new ChaseState();

            enemy.Patrol(delta);
            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class ChaseState : IEnemyState
    {
        public void Enter(DemontekAI enemy)
        {
            GD.Print($"{enemy.Type} Demontek started chasing!");
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            if (enemy.IsDead)
                return new DeathState();

            // Don't give up easily - keep chasing even if slightly out of detection range
            float distanceToTarget = enemy.DistanceToTarget;
            if (distanceToTarget > enemy.DetectionRange * 1.5f)
                return new ReturnState();

            // Attack aggressively when close
            if (enemy.IsNearTarget)
                return new AttackState();

            enemy.ChaseTarget(delta);
            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class AttackState : IEnemyState
    {
        private float _attackDuration = 0.5f;
        private float _timer = 0f;
        private bool _hasAttacked = false;

        public void Enter(DemontekAI enemy)
        {
            _timer = 0f;
            _hasAttacked = false;
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            _timer += delta;

            // Keep moving toward player during attack
            if (enemy.Target != null)
            {
                float direction = Mathf.Sign(enemy.Target.GlobalPosition.X - enemy.GlobalPosition.X);
                enemy.Velocity = new Vector2(direction * enemy.ChaseSpeed * 0.5f, enemy.Velocity.Y);
            }

            // Execute attack quickly
            if (!_hasAttacked && _timer >= _attackDuration * 0.3f)
            {
                enemy.Attack();
                _hasAttacked = true;
            }

            // Return to chase after attack completes
            if (_timer >= _attackDuration)
            {
                // Continue attacking if still in range
                if (enemy.IsNearTarget)
                    return new AttackState();
                else
                    return new ChaseState();
            }

            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class ReturnState : IEnemyState
    {
        public void Enter(DemontekAI enemy)
        {
            GD.Print($"{enemy.Type} Demontek returning to patrol.");
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            if (enemy.CanSeeTarget)
                return new ChaseState();

            float distanceToStart = enemy.GlobalPosition.DistanceTo(enemy.StartPosition);

            if (distanceToStart < 10f)
            {
                return new PatrolState();
            }

            enemy.MoveTowards(enemy.StartPosition, enemy.PatrolSpeed);
            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class HurtState : IEnemyState
    {
        private float _hurtDuration = 0.3f;
        private float _timer = 0f;

        public void Enter(DemontekAI enemy)
        {
            enemy.StopMoving();
            _timer = 0f;
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            _timer += delta;

            if (_timer >= _hurtDuration)
            {
                if (enemy.CanSeeTarget)
                    return new ChaseState();
                else
                    return new PatrolState();
            }

            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class DeathState : IEnemyState
    {
        public void Enter(DemontekAI enemy)
        {
            enemy.StopMoving();
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            // Stay in death state
            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }
    #endregion

    #region Enemy Types
    public enum EnemyType
    {
        Normal,    // Standard Demontek
        Guard,     // Demontek with uniform
        Mutant,    // Muscular Demontek
        Goliath,   // Boss: Demontek-Monster fusion
        King       // Final Boss: King Demontek
    }
    #endregion
}
