using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets.Develop.Runtime.Gameplay.Configs;
using UnityEngine;
using System.Text;

namespace Assets.Develop.Runtime.Gameplay.Core
{
    public class SequenceGeneratorService
    {
        private readonly GameplayConfig _config;
        private readonly GameMode _mode;

        public SequenceGeneratorService(ConfigsProviderService configsProviderService, GameMode mode)
        {
            _config = configsProviderService.GetConfig<GameplayConfig>();
            _mode = mode;
        }

        public string Generate()
        {
            string characters = _config.GetCharactersFor(_mode);
            int length = _config.SequenceLength;

            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = Random.Range(0, characters.Length);
                builder.Append(characters[randomIndex]);
            }

            return builder.ToString();
        }
    }
}
