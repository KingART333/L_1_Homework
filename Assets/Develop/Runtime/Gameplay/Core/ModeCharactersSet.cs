using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    [Serializable]
    public class ModeCharactersSet
    {
        [field: SerializeField] public GameMode Mode {  get; private set; }

        [field: SerializeField] public string Characters { get; private set; }
    }
}
