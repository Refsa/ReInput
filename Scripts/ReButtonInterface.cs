using System;

namespace ReInput
{
    public interface IReButtonInput
    {
        event Action onButtonDown;
        event Action onButtonUp;
        event Action<float> onButtonHeld;
    }

    public interface IReInputAxis
    {
        event Action<float> onGetAxis;
    }
}
