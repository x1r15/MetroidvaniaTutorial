﻿using Behaviours.Configs;
using Enums;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class JumpState : StateBase, IState
    {
        private static readonly int Jump = Animator.StringToHash("Jump");

        private bool _shallowJumpInitialized;
        private BehaviourConfig<PlatformMovementFeature> _config;
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        public JumpState(PlatformMovement movement) : base(movement)
        {
        }
        public void Init()
        {
            _config = _movement.GetConfig();
            _animator.SetTrigger(Jump);
            _movement.Jump();
            _shallowJumpInitialized = false;
        }

        public void Clean() {}

        public void Update() {}

        public void FixedUpdate()
        {
            ApplyShallowJump();
            _movement.ApplyHorizontalMovement(_movement.GetWalkSpeed());
        }

        public IState MonitorForChange()
        {
            if (!_movement.IsGrounded() && _rigidbody.velocity.y < 0)
            {
                return new FallingState(_movement);
            }

            return this;
        }

        public void ApplyAnimation()
        {
            _animator.SetBool(IsGrounded, _movement.IsGrounded());
            _animator.SetBool(IsGrounded, _movement.IsGrounded());
        }

        private void ApplyShallowJump()
        {
            if (!_movement.IsGrounded() &&
                !_inputProvider.GetAction(InputAction.Jump) &&
                _rigidbody.velocity.y > 0 &&
                !_shallowJumpInitialized &&
                _config.IsEnabled(PlatformMovementFeature.ShallowJump))
            {
                _rigidbody.SetVelocity(Axis.Y, _rigidbody.velocity.y / 2);
                _shallowJumpInitialized = true;
            }
        }

    }
}
