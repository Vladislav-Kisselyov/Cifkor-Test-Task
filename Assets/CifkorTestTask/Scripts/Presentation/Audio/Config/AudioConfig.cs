using System;
using CifkorTestTask.Infrastructure.Collections;
using UnityEngine;

namespace CifkorTestTask.Presentation.Audio.Config
{
    [Serializable]
    public class AudioConfig
    {
        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private UDictionary<string, AudioClip> _sounds;

        public AudioSource AudioSourcePrefab => _audioSourcePrefab;

        public bool TryGet(string soundId, out AudioClip clip)
        {
            return _sounds.TryGetValue(soundId, out clip);
        }
    }
}
