using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class WallJumpState : StateBase, IState
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        public WallJumpState(PlatformMovement movement) : base(movement) {}

        public void Init()
        {
            var dir = new Vector2(-Mathf.Sign(_inputProvider.GetAxisInput(Axis.X)), 0);
            _movement.DirectionalJump(dir.normalized, _movement.GetWallJumpForce());
            _movement.Jump();
            _animator.SetTrigger(Jump);
        }

        public void Clean()
        {
            _animator.ResetTrigger(Jump);
        }

        public void Update() {}

        public void FixedUpdate()
        {
            var speed = Time.fixedDeltaTime * _movement.GetWallJumpSpeedAcceleration();
            _movement.ApplyHorizontalMovement(speed, true);
        }

        public IState MonitorForChange()
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);
            if (_movement.IsGrounded())
            {
                if (inputX == 0)
                {
                    return new IdleState(_movement);
                }

                return new WalkState(_movement);
            }

            if (_movement.IsPushingAgainstTheWall())
            {
                return new WallSlideState(_movement);
            }

            return this;
        }

        public bool IsEnabled()
        {
            return _movement.GetConfig().IsEnabled(PlatformMovementFeature.WallJump);
        }
    }
}
