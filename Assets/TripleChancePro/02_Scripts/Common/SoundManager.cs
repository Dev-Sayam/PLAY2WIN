using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        [SerializeField] private AudioSource audiouSrc, musicSource, buttonAudioSrc;
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
        public void PlaySound(AudioClip clip, float delayTime = 0)
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
            buttonAudioSrc.Play();
        }
        public void PlaySfx(int index, float delayTime = 0)
        {
            PlaySound(sounds[index], delayTime);
        }
    }
}
