using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Meta.Features.GameModes
{
    public class GameModeSelectorService
    {
        private readonly IReadOnlyList<GameMode> _availableModes;

        private GameMode _selectedMode;

        public GameModeSelectorService(IReadOnlyList<GameMode> availableModes)
        {
            _availableModes = availableModes;
        }

        public event Action<bool> SelectionChanged;

        public event Action<GameMode> SelectionConfirmed;

        public IReadOnlyList<GameMode> AvailableModes => _availableModes;

        public GameMode SelectedMode => _selectedMode;

        public bool HasSelection => _selectedMode != GameMode.None;

        public void Select(GameMode mode)
        {
            if (_availableModes.Contains(mode) == false)
                throw new InvalidOperationException($"{mode} is not available for selection");

            if (_selectedMode == mode)
                return;

            _selectedMode = mode;

            SelectionChanged?.Invoke(HasSelection);
        }

        public void ClearSelection()
        {
            if (HasSelection == false)
                return;

            _selectedMode = GameMode.None;

            SelectionChanged?.Invoke(HasSelection);
        }

        public void SelectNext() => Select(GetNeighbourMode(1));

        public void SelectPrevious() => Select(GetNeighbourMode(-1));

        public void ConfirmSelection()
        {
            if (HasSelection == false)
                return;

            SelectionConfirmed?.Invoke(_selectedMode);
        }

        private GameMode GetNeighbourMode(int direction)
        {
            int modesCount = _availableModes.Count;

            if (modesCount == 0)
                throw new InvalidOperationException("There are no available modes to select");

            int currentIndex = _availableModes.IndexOf(_selectedMode);

            int nextIndex = currentIndex < 0
                ? (direction > 0 ? 0 : modesCount - 1)
                : (currentIndex + direction + modesCount) % modesCount;

            return _availableModes[nextIndex];
        }
    }

    internal static class ReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> list, T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], value))
                    return i;
            }

            return -1;
        }

        public static bool Contains<T>(this IReadOnlyList<T> list, T value) => list.IndexOf(value) >= 0;
    }
}
