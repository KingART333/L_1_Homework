using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/GameEconomyConfig", fileName = "GameEconomyConfig")]
    public class GameEconomyConfig : ScriptableObject
    {
        [field: SerializeField] public int WinGoldReward { get; private set; }
        [field: SerializeField] public int LoseGoldPenalty { get; private set; }
        [field: SerializeField] public int ResetProgressCost { get; private set; }
    }
}