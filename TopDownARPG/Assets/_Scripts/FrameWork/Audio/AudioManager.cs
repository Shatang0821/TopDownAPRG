using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Audio
{
    public class AudioManager : PersistentUnitySingleton<AudioManager>
    {
        [SerializeField] AudioSource BgmPlayer;
        [SerializeField] AudioSource GsePlayer;
        

        private const float MIN_PITCH = 0.9f;

        private const float MAX_PITCH = 1.1f;

        private float bgmValue = 1.0f; // 默认音量为1
        private float gseValue = 1.0f; // 默认音量为1


        public float BgmValue
        {
            get { return bgmValue; }
            set
            {
                bgmValue = Mathf.Clamp01(value); // 确保在0到1之间
                UpdateVolume();
            }
        }

        public float GseValue
        {
            get { return gseValue; }
            set
            {
                gseValue = Mathf.Clamp01(value); // 确保在0到1之间
                UpdateVolume();
            }
        }

        private void UpdateVolume()
        {
            BgmPlayer.volume = bgmValue; // 设置音效音量
            GsePlayer.volume = gseValue;
            // 如果有其他音源，也可以在这里进行设置
        }
        /// <summary>
        /// 音を出す
        /// </summary>
        /// <param name="audioData">音データ</param>
        public void PlaySfx(AudioData audioData)
        {
            BgmPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
        }   

        /// <summary>
        /// Pitchをランダムに変更して音を出す
        /// </summary>
        /// <param name="audioData">音データ</param>
        public void PlayRandomSfx(AudioData audioData)
        {
            BgmPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
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