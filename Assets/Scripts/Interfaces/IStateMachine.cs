using System;

namespace Interfaces
{
    public interface IStateMachine<TState> where TState : Enum
    {
        public void RecalculateState();
        public TState GetCurrentState();
    }
}
