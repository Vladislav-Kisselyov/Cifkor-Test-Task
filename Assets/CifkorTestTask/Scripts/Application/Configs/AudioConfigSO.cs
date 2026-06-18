using CifkorTestTask.Infrastructure.Configs;
using CifkorTestTask.Presentation.Audio.Config;
using UnityEngine;

namespace CifkorTestTask.Application.Configs
{
    [CreateAssetMenu(fileName = nameof(AudioConfigSO),
        menuName = "Cifkor Test Task/Configs SO/" + nameof(AudioConfigSO),
        order = 0)]
    public class AudioConfigSO: BaseScriptableObjectConfig<AudioConfig>
    {

    }
}
