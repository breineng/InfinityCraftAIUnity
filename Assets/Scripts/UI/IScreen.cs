using InfinityCraft.Managers;

namespace InfinityCraft.UI
{
    public interface IScreen
    {
        bool? ShowCursor { get; }
        bool CanDeactivate { get; }
        void Init(ScreenManager screenManager);
        void Activate();
        void Deactivate();
    }
}