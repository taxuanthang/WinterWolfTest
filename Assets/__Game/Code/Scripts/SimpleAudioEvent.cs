using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Audio Events/Simple")]
    public class SimpleAudioEvent : AudioEvent
    {
        public AudioClip[] clips; // Danh sách clip để phát ngẫu nhiên
        public float volume = 1f;
        [Range(0, 2)] public float pitch = 1f;

        public override void Play(AudioSource source)
        {
            if (clips.Length == 0) return;

            source.clip = clips[Random.Range(0, clips.Length)];
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
    }
}
