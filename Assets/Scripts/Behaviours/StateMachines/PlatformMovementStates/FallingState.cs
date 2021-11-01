using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class FallingState : StateBase, IState
    {
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public FallingState(PlatformMovement movement) : base(movement) {}

        public void Init()
        {
            _animator.SetBool(IsFalling, true);
            _animator.SetBool(IsGrounded, false);
        }

        public void Clean()
        {
            _animator.SetBool(IsFalling, false);
        }

        public void Update() {}

        public void FixedUpdate()
        {
            _movement.ApplyHorizontalMovement(_movement.GetWalkSpeed());
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
            return true;
        }
    }
}
