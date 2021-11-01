using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines.PlatformMovementStates
{
    public class StateBase
    {
        protected readonly PlatformMovement _movement;
        protected readonly IInputProvider _inputProvider;
        protected readonly Rigidbody2D _rigidbody;
        protected readonly Animator _animator;

        private readonly HashSet<InputAction> _registeredActions = new HashSet<InputAction>();

        public StateBase(PlatformMovement movement)
        {
            _movement = movement;
            _inputProvider = movement.GetInputProvider();
            _rigidbody = movement.GetRigidbody();
            _animator = movement.GetAnimator();
        }

        public void RequestAction(InputAction action)
        {
            _registeredActions.Add(action);
        }

        public bool IsRequested(InputAction action)
        {
            return _registeredActions.Contains(action);
        }

        public void ClearRequest(InputAction action)
        {
            _registeredActions.Remove(action);
        }
    }
}
