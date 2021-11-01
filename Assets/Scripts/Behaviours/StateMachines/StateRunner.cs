using Interfaces;

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

            _requestedState = _currentState.MonitorForChange();
            if (_requestedState != _currentState && _requestedState.IsEnabled())
            {
                _currentState.Clean();
                _currentState = _requestedState;
                _requestedState.Init();
            }
        }

        public void Update()
        {

            _currentState.Update();
        }
    }

}
