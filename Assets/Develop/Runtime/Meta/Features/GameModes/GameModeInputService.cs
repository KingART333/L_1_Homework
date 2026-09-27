using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.GameModes
{
    public class GameModeInputService
    {
        public const KeyCode NextModeKey = KeyCode.RightArrow;
        public const KeyCode PreviousModeKey = KeyCode.LeftArrow;
        public const KeyCode ConfirmSelectionKey = KeyCode.Space;

        private readonly GameModeSelectorService _selector;

        private string _lastHint;

        public GameModeInputService(GameModeSelectorService selector)
        {
            _selector = selector;
        }

        public void Update()
        {
            if (Input.GetKeyDown(NextModeKey))
                _selector.SelectNext();
            else if (Input.GetKeyDown(PreviousModeKey))
                _selector.SelectPrevious();

            if (Input.GetKeyDown(ConfirmSelectionKey))
                _selector.ConfirmSelection();
        }

        public void PrintHintIfChanged()
        {
            string hint = GetHint();

            if (hint == _lastHint)
                return;

            _lastHint = hint;

            Debug.Log(hint);
        }

        public string GetHint()
        {
            string modes = string.Join(" / ", _selector.AvailableModes);

            if (_selector.HasSelection == false)
                return $"<{PreviousModeKey}> <{NextModeKey}> — choose mode ({modes})";

            return $"mode: {_selector.SelectedMode} | <{PreviousModeKey}> <{NextModeKey}> — change | <{ConfirmSelectionKey}> — start";
        }
    }
}
