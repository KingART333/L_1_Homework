using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using UnityEngine;
using System.Text;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class SequenceGeneratorService
    {
        private readonly ConfigsProviderService _configsProviderService;

        public SequenceGeneratorService(ConfigsProviderService configsProviderService)
        {
            _configsProviderService = configsProviderService;
        }

        public string Generate(GameMode mode)
        {
            GameplayConfig config = _configsProviderService.GetConfig<GameplayConfig>();

            return Generate(config.GetCharactersFor(mode), config.SequenceLength);
        }

        private string Generate(string characters, int length)
        {
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
