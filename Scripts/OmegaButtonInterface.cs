using System;

namespace Refsa.OmegaInput
{
    public interface IOmegaButtonInput
    {
        event Action onButtonDown;
        event Action onButtonUp;
        event Action<float> onButtonHeld;
    }

    public interface IOmegaInputAxis
    {
        event Action<float> onGetAxis;
    }
}
