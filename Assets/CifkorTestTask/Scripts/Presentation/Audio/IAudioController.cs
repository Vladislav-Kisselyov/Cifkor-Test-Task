namespace CifkorTestTask.Presentation.Audio
{
    public interface IAudioController
    {
        public void Play(string soundId, bool isLoop = false);
    }
}
