using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KheloJeeto
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        [SerializeField] private AudioSource audiouSrc, musicSource, buttonAudioSrc, wheelAudioSource;
        //variable use sound
        [SerializeField] private AudioClip[] sounds;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void PlaySound(AudioClip clip, float delayTime = 0)
        {
            audiouSrc.clip = clip;
            audiouSrc.PlayDelayed(delayTime);
        }
        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        public void PlayButtonSound()
        {
            if (JeetoJokerManager.Instance.isMusicSoundEnabled)
                buttonAudioSrc.Play();
        }
        public void PlaySfx(int index, float delayTime = 0)
        {
            if (JeetoJokerManager.Instance.isMusicSoundEnabled)
                PlaySound(sounds[index], delayTime);
        }

        public void PlaySound(AudioClip clip)
        {
            if (JeetoJokerManager.Instance.isMusicSoundEnabled)
            {
                Debug.Log("called");
                audiouSrc.clip = clip;
                audiouSrc.Play();
            }
        }

        public void PlayOneShotSound(AudioClip clip)
        {
            if (JeetoJokerManager.Instance.isMusicSoundEnabled)
            {
                audiouSrc.PlayOneShot(clip);
            }
        }

        public void PlaySpinAudio(AudioClip clip)
        {
            if (JeetoJokerManager.Instance.isMusicSoundEnabled)
            {
                Debug.Log("called");
                wheelAudioSource.clip = clip;
                wheelAudioSource.Play(1);
            }
        }
        public void StopSpinAudio()
        {
            wheelAudioSource.Stop();
        }
    }
}