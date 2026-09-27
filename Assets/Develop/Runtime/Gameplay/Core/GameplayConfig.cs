using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    [CreateAssetMenu(menuName = "Configs/GameplayConfig", fileName = "GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField] private List<ModeCharactersSet> _modeCharactersSets;
        [SerializeField] private int _sequenceLength;

        public int SequenceLength => _sequenceLength;

        public GameMode[] GetAvailableModes()
        {
            List<GameMode> modes = new();

            foreach (ModeCharactersSet set in _modeCharactersSets)
            {
                if (modes.Contains(set.Mode) == false)
                    modes.Add(set.Mode);
            }

            return modes.ToArray();
        }

        public string GetCharactersFor(GameMode mode)
        {
            foreach (ModeCharactersSet set in _modeCharactersSets)
            {
                if (set.Mode == mode)
                    return set.Characters;
            }

            throw new InvalidOperationException($"Not found characters set for mode {mode}");
        }
    }
}
