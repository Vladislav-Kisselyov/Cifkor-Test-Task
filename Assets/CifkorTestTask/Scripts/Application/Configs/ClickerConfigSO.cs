using CifkorTestTask.Domain.Cookie.Config;
using CifkorTestTask.Infrastructure.Configs;
using UnityEngine;

namespace CifkorTestTask.Application.Configs
{
    [CreateAssetMenu(fileName = nameof(ClickerConfigSO),
        menuName = "Cifkor Test Task/Configs SO/" + nameof(ClickerConfigSO),
        order = 0)]
    public class ClickerConfigSO: BaseScriptableObjectConfig<ClickerConfig>
    {

    }
}
