using System.Linq;
using Behaviours.Configs;
using Behaviours.StateMachines;
using Behaviours.StateMachines.PlatformMovementStates;
using Enums;
using Extensions;
using Interfaces;
using UnityEngine;
using Utils;

namespace Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IInputProvider))]
    public class PlatformMovement : MonoBehaviour
    {
        private BehaviourConfig<PlatformMovementFeature> _config;

        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private IInputProvider _inputProvider;
        private CircleCheck _groundCheck;
        private CircleCheck _frontCheck;
        private StateRunner _stateRunner;


        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float wallJumpForce;

        [SerializeField]
        private float jumpForce;

        [SerializeField]
        private float wallJumpSpeedAcceleration;

        [SerializeField]
        private float wallSlideSpeed;

        [SerializeField]
        private PlatformMovementFeature[] initialFeatures;

        private const int GroundCheckId = 0;
        private const int FrontCheckId = 1;

        public Animator GetAnimator()
        {
            return _animator;
        }

        public IInputProvider GetInputProvider()
        {
            return _inputProvider;
        }

        public Rigidbody2D GetRigidbody()
        {
            return _rigidbody;
        }

        public BehaviourConfig<PlatformMovementFeature> GetConfig()
        {
            return _config;
        }

        public float GetWalkSpeed()
        {
            return walkSpeed;
        }

        public float GetWallSlideSpeed()
        {
            return wallSlideSpeed;
        }

        public float GetWallJumpForce()
        {
            return wallJumpForce;
        }

        public float GetWallJumpSpeedAcceleration()
        {
            return wallJumpSpeedAcceleration;
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector2.up * jumpForce);
        }

        public void DirectionalJump(Vector2 dir, float force)
        {
            _rigidbody.AddForce(dir * force);
        }

        public void ApplyHorizontalMovement(float speed, bool additive = false)
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);
            speed *= inputX;
            var additiveSpeed = _rigidbody.velocity.x + speed;
            _rigidbody.SetVelocity(Axis.X, additive ? additiveSpeed : speed);
            _rigidbody.SetVelocity(Axis.X, Mathf.Clamp(
                _rigidbody.velocity.x,
                -walkSpeed,
                walkSpeed
            ));
        }

        private void AdjustAnimationDirection()
        {
            if (_inputProvider.GetAxisInput(Axis.X) == 0) return;

            var scale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Sign(_inputProvider.GetAxisInput(Axis.X)), scale.y, scale.y);
        }

        public bool IsPushingAgainstTheWall()
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);
            return inputX != 0 &&
                   Mathf.Sign(inputX) == Mathf.Sign(transform.localScale.x) &&
                   _frontCheck.Check();
        }

        public bool IsGrounded()
        {
            return _groundCheck.Check();
        }

        private void Start()
        {
            _config = new BehaviourConfig<PlatformMovementFeature>(initialFeatures);
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _inputProvider = GetComponent<IInputProvider>();
            _stateRunner = new StateRunner(new IdleState(this));

            var circleChecks = GetComponentsInChildren<CircleCheck>();
            _groundCheck = circleChecks.First(c => c.GetId() == GroundCheckId);
            _frontCheck = circleChecks.First(c => c.GetId() == FrontCheckId);
        }

        private void FixedUpdate()
        {
            _stateRunner.FixedUpdate();
        }

        private void Update()
        {
            _stateRunner.Update();
            AdjustAnimationDirection();
        }
    }
}
