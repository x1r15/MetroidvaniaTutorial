using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class IdleState : StateBase, IState
    {
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");

        public IdleState(PlatformMovement movement) :
            base(movement) {}
        public void Init() {}

        public void Clean() {}

        public void Update()
        {
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                RequestAction(InputAction.Jump);
            }
        }

        public void FixedUpdate()
        {
            _movement.ApplyHorizontalMovement(0);
        }

        public IState MonitorForChange()
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);
            if (inputX != 0)
            {
                return new WalkState(_movement);
            }

            if (IsRequested(InputAction.Jump))
            {
                return new JumpState(_movement);
            }
            return this;
        }

        public void ApplyAnimation()
        {
            _animator.SetBool(IsGrounded, _movement.IsGrounded());
            _animator.SetBool(IsFalling, false);
            _animator.SetBool(IsWalking, false);
        }
    }
}
