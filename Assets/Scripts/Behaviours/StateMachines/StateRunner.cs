using Interfaces;
using UnityEngine;

namespace Behaviours.StateMachines
{
    public class StateRunner
    {
        private IState _currentState;
        private IState _requestedState;

        public StateRunner(IState currentState)
        {
            _currentState = currentState;
            _currentState.Init();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
            _currentState.ApplyAnimation();

            _requestedState = _currentState.MonitorForChange();
            if (_requestedState != _currentState)
            {
                _currentState.Clean();
                _currentState = _requestedState;
                _requestedState.Init();
            }

            Debug.Log(_currentState.GetType());
        }

        public void Update()
        {

            _currentState.Update();
        }
    }

}
