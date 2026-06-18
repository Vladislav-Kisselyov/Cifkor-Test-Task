using System;
using System.Collections.Generic;
using CifkorTestTask.Infrastructure.Pooling;
using CifkorTestTask.Presentation.Cookie.View;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Cookie.Presenter
{
    public class ParticlesPanelPresenter : IInitializable, IDisposable
    {
        private const int MinParticlesCount = 1;
        private const int MaxParticlesCount = 10;
        private const float PositionRandomDistance = 50f;

        private readonly IPoolableFactory _poolableFactory;
        private readonly IParticlesPanelView _view;

        private readonly List<ParticleView> _spawnedParticles = new();

        public ParticlesPanelPresenter(
            IPoolableFactory poolableFactory,
            IParticlesPanelView view)
        {
            _poolableFactory = poolableFactory;
            _view = view;
        }

        public void Initialize()
        {
            _view.ParticlePrefab.gameObject.SetActive(false);
            _view.Show();
        }

        public void Dispose()
        {
            foreach (var spawnedParticle in _spawnedParticles)
            {
                if (!spawnedParticle)
                    continue;

                _poolableFactory.Despawn(spawnedParticle);
            }

            _spawnedParticles.Clear();
            _view.Hide();
        }

        public void ShowParticle(
            int particlesCount,
            Sprite particlesSprite,
            Transform from,
            Transform to,
            Action onLastParticleFinished)
        {
            var particlesToSpawn = Math.Clamp(particlesCount, MinParticlesCount, MaxParticlesCount);

            var particle = _poolableFactory.Spawn(_view.ParticlePrefab, _view.ParticleParentTransform);
            particle.SetText(particlesCount);
            particle.transform.position = RandomizePosition(from.position);
            particle.ParticleImage.rateOverLifetime = particlesToSpawn;
            particle.ParticleImage.sprite = particlesSprite;
            particle.ParticleImage.attractorTarget = to;
            particle.ParticleImage.onLastParticleFinished.AddListener(() =>
            {
                onLastParticleFinished?.Invoke();
                DisposeParticle(particle);
            });

            _spawnedParticles.Add(particle);
            particle.ParticleImage.Play();
        }

        private Vector3 RandomizePosition(Vector3 position)
        {
            position.x += UnityEngine.Random.Range(-PositionRandomDistance, PositionRandomDistance);
            position.y += UnityEngine.Random.Range(-PositionRandomDistance, PositionRandomDistance);

            return position;
        }

        private void DisposeParticle(ParticleView particle)
        {
            _spawnedParticles.Remove(particle);

            if (!particle)
                return;

            _poolableFactory.Despawn(particle);
        }
    }
}
