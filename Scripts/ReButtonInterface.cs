using System;

namespace Refsa.ReInput
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
