using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines
{
    public class PlatformMovementStateMachine : IStateMachine<PlatformMovementState>
    {
        private readonly PlatformMovement _movement;
        private readonly IInputProvider _inputProvider;
        private readonly Rigidbody2D _rigidbody;
        private PlatformMovementState _state;

        public PlatformMovementStateMachine(PlatformMovement movement, IInputProvider inputProvider, Rigidbody2D rigidbody)
        {
            _state = PlatformMovementState.Idle;
            _movement = movement;
            _inputProvider = inputProvider;
            _rigidbody = rigidbody;
        }


        public void RecalculateState()
        {
            var inputX = _inputProvider.GetAxisInput(Axis.X);
            var jumpRequested = _inputProvider.GetAction(InputAction.Jump);
             if (_state == PlatformMovementState.Walk)
            {
                if (inputX == 0)
                {
                    _state = PlatformMovementState.Idle;
                }
                else if (jumpRequested)
                {
                    _state = PlatformMovementState.Jumping;
                }
                else if (!_movement.IsGrounded())
                {
                    _state = PlatformMovementState.Falling;
                }
            } else if (_state == PlatformMovementState.Jumping)
            {
                if (!_movement.IsGrounded() && _rigidbody.velocity.y < 0)
                {
                    _state = PlatformMovementState.Falling;
                }
            } else if (_state == PlatformMovementState.Falling)
            {
                if (_movement.IsGrounded())
                {
                    if (inputX == 0)
                    {
                        _state = PlatformMovementState.Idle;
                    }
                    else
                    {
                        _state = PlatformMovementState.Walk;
                    }
                }
                else if (_movement.IsPushingAgainstTheWall())
                {
                    _state = PlatformMovementState.WallSlide;
                }
            }
            else if (_state == PlatformMovementState.WallSlide)
            {
                if (jumpRequested)
                {
                    _state = PlatformMovementState.WallJump;
                }
                else if (_movement.IsGrounded())
                {
                    if (inputX == 0)
                    {
                        _state = PlatformMovementState.Idle;
                    }
                    else
                    {
                        _state = PlatformMovementState.Walk;
                    }
                }
                else if (!_movement.IsPushingAgainstTheWall())
                {
                    _state = PlatformMovementState.Falling;
                }
            }
            else if (_state == PlatformMovementState.WallJump)
            {
                if (!_movement.IsGrounded() && _rigidbody.velocity.y < 0)
                {
                    _state = PlatformMovementState.Falling;
                }
            }
        }

        public PlatformMovementState GetCurrentState()
        {
            return _state;
        }
    }
}
