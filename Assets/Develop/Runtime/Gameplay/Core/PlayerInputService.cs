using UnityEngine;

namespace Assets.Develop.Runtime.Gameplay.Core
{
    public class PlayerInputService
    {
        public bool TryGetSymbol(out char symbol)
        {
            symbol = default;

            string inputString = Input.inputString;

            if (inputString.Length == 0)
                return false;

            char rawSymbol = inputString[0];

            if (rawSymbol == '\b' || rawSymbol == '\n' || rawSymbol == '\r')
                return false;

            symbol = char.ToLower(rawSymbol);

            return true;
        }
    }
}
