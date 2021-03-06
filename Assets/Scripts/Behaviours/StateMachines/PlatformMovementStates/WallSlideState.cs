using Enums;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class WallSlideState : StateBase, IState
    {
        private static readonly int IsWallSliding = Animator.StringToHash("IsWallSliding");

        public WallSlideState(PlatformMovement movement) : base(movement) {}

        public void Init()
        {
            _animator.SetBool(IsWallSliding, true);
        }

        public void Clean()
        {
            _animator.SetBool(IsWallSliding, false);
        }

        public void Update()
        {
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                RequestAction(InputAction.Jump);
            }
        }

        public void FixedUpdate()
        {
            _rigidbody.SetVelocity(Axis.Y, Mathf.Clamp(
                    _rigidbody.velocity.y,
                    -_movement.GetWallSlideSpeed(),
                    float.MaxValue
                ));
        }

        public IState MonitorForChange()
        {
            if (IsRequested(InputAction.Jump))
            {
                ClearRequest(InputAction.Jump);
                return new WallJumpState(_movement);
            }

            if (_movement.IsGrounded())
            {
                var inputX = _inputProvider.GetAxisInput(Axis.X);
                if (inputX == 0)
                {
                    return new IdleState(_movement);
                }

                return new WalkState(_movement);
            }

            if(!_movement.IsPushingAgainstTheWall())
            {
                return new FallingState(_movement);
            }

            return this;
        }

        public bool IsEnabled()
        {
            return _movement.GetConfig().IsEnabled(PlatformMovementFeature.WallSlide);
        }
    }
}
