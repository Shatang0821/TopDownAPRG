using UnityEngine;

namespace FrameWork.Audio
{
    /// <summary>
    /// AudioClipとvoluemeをまとめるクラス
    /// </summary>
    [System.Serializable]
    public class AudioData
    {
        /// <summary>
        /// 音源
        /// </summary>
        public AudioClip audioClip;

        /// <summary>
        /// 音量
        /// </summary>
        public float volume;
    }
}