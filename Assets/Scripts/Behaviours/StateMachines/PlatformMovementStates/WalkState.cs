using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class WalkState : StateBase, IState
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public WalkState(PlatformMovement movement) : base(movement) {}

        public void Init()
        {
            _animator.SetBool(IsGrounded, true);
            _animator.SetBool(IsWalking, true);
        }

        public void Clean()
        {
            _animator.SetBool(IsWalking, false);
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
            _movement.ApplyHorizontalMovement(_movement.GetWalkSpeed());
        }

        public IState MonitorForChange()
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);


            if (inputX == 0)
            {
                return new IdleState(_movement);
            }

            if (IsRequested(InputAction.Jump))
            {
                ClearRequest(InputAction.Jump);
                return new JumpState(_movement);
            }

            if (!_movement.IsGrounded())
            {
                return new FallingState(_movement);
            }

            return this;
        }

        public bool IsEnabled()
        {
            return true;
        }
    }
}
