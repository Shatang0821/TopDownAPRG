using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Audio
{
    public class AudioManager : PersistentUnitySingleton<AudioManager>
    {
        [SerializeField] AudioSource sFXPlayer;

        private const float MIN_PITCH = 0.9f;

        private const float MAX_PITCH = 1.1f;

        public float BgmValue;
        public float GseValue;

        /// <summary>
        /// 音を出す
        /// </summary>
        /// <param name="audioData">音データ</param>
        public void PlaySfx(AudioData audioData)
        {
            sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
        }

        /// <summary>
        /// Pitchをランダムに変更して音を出す
        /// </summary>
        /// <param name="audioData">音データ</param>
        public void PlayRandomSfx(AudioData audioData)
        {
            sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            PlaySfx(audioData);
        }

        /// <summary>
        /// いくつかの音源をランダムに流す
        /// </summary>
        /// <param name="audioData">音データ配列</param>
        public void PlayRandomSfx(AudioData[] audioData)
        {
            PlayRandomSfx(audioData[Random.Range(0, audioData.Length)]);
        }
    }
}