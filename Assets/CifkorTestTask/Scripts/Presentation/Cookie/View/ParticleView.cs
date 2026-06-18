using AssetKits.ParticleImage;
using CifkorTestTask.Infrastructure.Pooling;
using TMPro;
using UnityEngine;

namespace CifkorTestTask.Presentation.Cookie.View
{
    public class ParticleView : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleImage _particleImage;
        [SerializeField] private TMP_Text _text;

        public ParticleImage ParticleImage => _particleImage;

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            _particleImage.onLastParticleFinished.RemoveAllListeners();
            gameObject.SetActive(false);
        }

        public void SetText(int particlesCount)
        {
            _text.SetText($"+{particlesCount}");
        }
    }
}
