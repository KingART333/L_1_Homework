using Assets.Develop.Runtime.Gameplay.Core;
using System;
using UnityEngine;

namespace Assets.Develop.Runtime.Gameplay.Configs
{
    [Serializable]
    public class ModeCharactersSet
    {
        [field: SerializeField] public GameMode Mode {  get; private set; }

        [field: SerializeField] public string Characters { get; private set; }
    }
}
