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
        [Export] public float ChaseSpeed { get; set; } = 100f;
        [Export] public float DetectionRange { get; set; } = 150f;
        [Export] public float AttackRange { get; set; } = 40f;
        [Export] public float PatrolDistance { get; set; } = 200f;

        [ExportCategory("Combat")]
        [Export] public int MaxHealth { get; set; } = 20;
        [Export] public int Damage { get; set; } = 5;
        [Export] public float AttackCooldown { get; set; } = 1.5f;
        [Export] public float Gravity { get; set; } = 980f;

        [ExportCategory("References")]
        [Export] public NodePath AnimatedSpritePath { get; set; }
        [Export] public NodePath DetectionAreaPath { get; set; }
        [Export] public NodePath AttackAreaPath { get; set; }
        #endregion

        #region Private Variables
        private AnimatedSprite2D _animatedSprite;
        private Area2D _detectionArea;
        private Area2D _attackArea;

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

            // Update state machine
            _stateMachine.Update(deltaF);

            // Apply movement
            Velocity = _velocity;
            MoveAndSlide();

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
                    Damage = 5;
                    ChaseSpeed = 100f;
                    DetectionRange = 150f;
                    break;

                case EnemyType.Guard:
                    MaxHealth = 25;
                    Damage = 5;
                    ChaseSpeed = 80f;
                    DetectionRange = 200f;
                    PatrolDistance = 150f;
                    break;

                case EnemyType.Mutant:
                    MaxHealth = 50;
                    Damage = 10;
                    ChaseSpeed = 120f;
                    DetectionRange = 180f;
                    AttackCooldown = 1.2f;
                    break;

                case EnemyType.Goliath:
                    MaxHealth = 200;
                    Damage = 20;
                    ChaseSpeed = 70f;
                    DetectionRange = 250f;
                    AttackCooldown = 2.0f;
                    break;

                case EnemyType.King:
                    MaxHealth = 500;
                    Damage = 30;
                    ChaseSpeed = 90f;
                    DetectionRange = 300f;
                    AttackCooldown = 1.5f;
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

            // Turn around at patrol distance or edges
            if (_patrolDistanceTraveled >= PatrolDistance || !IsOnFloor())
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
            if (_velocity.X > 0 && !_facingRight)
            {
                _facingRight = true;
                _animatedSprite.FlipH = false;
            }
            else if (_velocity.X < 0 && _facingRight)
            {
                _facingRight = false;
                _animatedSprite.FlipH = true;
            }
        }
        #endregion

        #region Combat
        public void Attack()
        {
            if (_attackCooldownTimer > 0 || _target == null) return;

            _attackCooldownTimer = AttackCooldown;
            EmitSignal(SignalName.AttackedPlayer, Damage);

            // Apply damage to player
            if (_target.HasMethod("TakeDamage"))
            {
                _target.Call("TakeDamage", Damage);
            }

            GD.Print($"{Type} Demontek attacked for {Damage} damage!");
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;

            _currentHealth -= damage;
            EmitSignal(SignalName.HealthChanged, _currentHealth, MaxHealth);

            GD.Print($"{Type} Demontek took {damage} damage. Health: {_currentHealth}/{MaxHealth}");

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

            // Remove collision
            SetCollisionLayerValue(1, false);
            SetCollisionMaskValue(1, false);

            // Queue free after death animation
            GetTree().CreateTimer(2.0f).Timeout += QueueFree;

            GD.Print($"{Type} Demontek died!");
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

            switch (currentState)
            {
                case "IdleState":
                    PlaySafe("idle");
                    break;
                case "PatrolState":
                    PlaySafe("walk");
                    break;
                case "ChaseState":
                    PlaySafe("run", "walk"); // fallback to walk if no run
                    break;
                case "AttackState":
                    PlaySafe("attack");
                    break;
                case "HurtState":
                    PlaySafe("hurt");
                    break;
                case "DeathState":
                    PlaySafe("death");
                    break;
            }
        }

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

            if (!enemy.CanSeeTarget)
                return new ReturnState();

            if (enemy.IsNearTarget)
                return new AttackState();

            enemy.ChaseTarget(delta);
            return this;
        }

        public void Exit(DemontekAI enemy) { }
    }

    public class AttackState : IEnemyState
    {
        private float _attackDuration = 0.8f;
        private float _timer = 0f;
        private bool _hasAttacked = false;

        public void Enter(DemontekAI enemy)
        {
            enemy.StopMoving();
            _timer = 0f;
            _hasAttacked = false;
        }

        public IEnemyState Update(DemontekAI enemy, float delta)
        {
            _timer += delta;

            // Execute attack at midpoint
            if (!_hasAttacked && _timer >= _attackDuration / 2)
            {
                enemy.Attack();
                _hasAttacked = true;
            }

            // Return to chase after attack completes
            if (_timer >= _attackDuration)
            {
                if (enemy.IsNearTarget)
                    return this; // Continue attacking
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
