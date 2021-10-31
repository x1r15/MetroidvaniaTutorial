namespace Interfaces
{
    public interface IState
    {
        public void Init();
        public void Clean();
        public void Update();
        public void FixedUpdate();
        public IState MonitorForChange();
        public void ApplyAnimation();
    }
}
