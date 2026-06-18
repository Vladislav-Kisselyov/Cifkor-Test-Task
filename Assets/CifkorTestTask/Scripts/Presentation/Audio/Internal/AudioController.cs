using System.Collections.Generic;
using CifkorTestTask.Presentation.Audio.Config;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Audio.Internal
{
    internal class AudioController : IAudioController, IInitializable
    {
        private readonly AudioConfig _config;
        private readonly List<AudioSource> _audioSources = new();

        private Transform _audioSourcesRoot;

        public AudioController(AudioConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            var go = new GameObject("AudioRoot");
            _audioSourcesRoot = go.transform;
            Object.DontDestroyOnLoad(go);
        }

        public void Play(string soundId, bool isLoop = false)
        {
            if (!_config.TryGet(soundId, out var audioClip))
            {
                return;
            }

            var audioSourceVacant = GetAudioSource();

            SetAndPlayClip();

            void SetAndPlayClip()
            {
                audioSourceVacant.name = $"{audioClip.name}";
                audioSourceVacant.clip = audioClip;
                audioSourceVacant.volume = 1;
                audioSourceVacant.loop = isLoop;
                audioSourceVacant.Play();
            }
        }

        private AudioSource GetAudioSource()
        {
            if (_audioSources.Count == 0)
            {
                return CreateAudioSource();
            }

            foreach (var audioSource in _audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }
            }

            var audioSourceNew = CreateAudioSource();
            _audioSources.Add(audioSourceNew);
            return audioSourceNew;
        }

        private AudioSource CreateAudioSource()
        {
            var source = GameObject.Instantiate(_config.AudioSourcePrefab, _audioSourcesRoot);

            source.playOnAwake = false;
            source.mute = false;
            source.loop = false;

            return source;
        }
    }
}
