namespace TransitionEffects
{
    public interface ITransitionEffect
    {
        bool IsAvailable { get; }

        void Clear();

        void Play(string animationKey);

        void PlayImmediate(string animationKey);

        bool IsPlaying { get; }
    }
}
