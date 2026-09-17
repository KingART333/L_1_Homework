using Assets.Develop.Runtime.Gameplay.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Develop.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameplayConfig", fileName = "GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField] private List<ModeCharactersSet> _modeCharactersSets;
        [SerializeField] private int _sequenceLength;

        public int SequenceLength => _sequenceLength;

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
