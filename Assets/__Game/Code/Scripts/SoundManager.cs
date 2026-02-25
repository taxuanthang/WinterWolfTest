using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {

        public static SoundManager instance;
        public AudioSource musicSource;
        public AudioSource sfxSource;

        public AudioMixer mixer;

        [SerializeField] SimpleAudioEvent _bloop_Sound;

        [SerializeField] SimpleAudioEvent _explosion_Sound;

        [SerializeField] SimpleAudioEvent _win_Sound;

        [SerializeField] SimpleAudioEvent _background_Music;

        private void Awake()
        {
            if(musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }

            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void TestSFX()
        {
            ToggleMuteSFX(true);
        }

        public void TestMusic()
        {
            ToggleMuteMusic(true);
        }

        public void ToggleMuteMusic(bool isMuted)
        {
            if (isMuted)
            {
                mixer.SetFloat("MusicVol", -80f); // Tắt sạch
            }
            else
            {
                mixer.SetFloat("MusicVol", 0f);   // Bật về mức tối đa
            }
        }

        public void ToggleMuteSFX(bool isMuted)
        {
            if (isMuted)
            {
                mixer.SetFloat("SFXVol", -80f); // Tắt sạch
            }
            else
            {
                mixer.SetFloat("SFXVol", 0f);   // Bật về mức tối đa
            }
        }

        public void PlayBloopSound()
        {
            _explosion_Sound.Play(sfxSource);
        }

        public void PlayExplosionSound()
        {
            _explosion_Sound.Play(sfxSource);
        }

        public async Task PlayWinSound()
        {
            _win_Sound.Play(sfxSource);
            await Task.Delay((int)_win_Sound.clips[0].length * 1000);
        }

        public void PlayBackgroundSound()
        {
            _background_Music.Play(musicSource);
        }
    }
}
