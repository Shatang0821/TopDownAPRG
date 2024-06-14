using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Audio
{
    public class AudioManager : PersistentUnitySingleton<AudioManager>
    {
        [SerializeField] AudioSource BgmPlayer;
        [SerializeField] AudioSource GsePlayer;
        [SerializeField] AudioSource GseChangeSound;  // Changed to AudioSource

        private const float MIN_PITCH = 0.9f;
        private const float MAX_PITCH = 1.1f;

        private float bgmValue = 1.0f;
        private float gseValue = 1.0f;

        public float BgmValue
        {
            get { return bgmValue; }
            set
            {
                bgmValue = Mathf.Clamp01(value);
                UpdateVolume();
            }
        }

        public float GseValue
        {
            get { return gseValue; }
            set
            {
                gseValue = Mathf.Clamp01(value);
                UpdateVolume();
            }
        }

        private void UpdateVolume()
        {
            BgmPlayer.volume = bgmValue;
            GsePlayer.volume = gseValue;
        }

        public void PlaySfx(AudioSource source, float volume)
        {
            if (source != null)
            {
                source.volume = volume;
                source.Play();
            }
            else
            {
                Debug.LogError("AudioSource is null. Cannot play sound.");
            }
        }

        public void PlaySfx(AudioData audioData)
        {
            BgmPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
        }

        public void PlayRandomSfx(AudioData audioData)
        {
            BgmPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            PlaySfx(audioData);
        }

        public void PlayRandomSfx(AudioData[] audioData)
        {
            PlayRandomSfx(audioData[Random.Range(0, audioData.Length)]);
        }

        public void PlayGseChangeSound()
        {
            PlaySfx(GseChangeSound, GseValue);
        }
    }
}
