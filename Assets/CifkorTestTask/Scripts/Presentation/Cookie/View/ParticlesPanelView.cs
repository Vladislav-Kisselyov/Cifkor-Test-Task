using CifkorTestTask.Presentation.Screens;
using UnityEngine;

namespace CifkorTestTask.Presentation.Cookie.View
{
    public interface IParticlesPanelView : IViewBase
    {
        public ParticleView ParticlePrefab { get; }
        public Transform ParticleParentTransform { get; }
    }

    public class ParticlesPanelView : ViewBase, IParticlesPanelView
    {
        [SerializeField] private ParticleView _particlePrefab;
        [SerializeField] private Transform _particleParentTransform;

        public ParticleView ParticlePrefab => _particlePrefab;
        public Transform ParticleParentTransform => _particleParentTransform;
    }
}
