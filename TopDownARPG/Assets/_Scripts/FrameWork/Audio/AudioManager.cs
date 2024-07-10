using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Audio
{
    public class AudioManager : PersistentUnitySingleton<AudioManager>
    {
        [SerializeField] AudioSource BgmPlayer;        //普通BGM
        [SerializeField] AudioSource WinBgm;　　　　 　//勝つBGM   
        [SerializeField] AudioSource LoseBgm;        　//負けるBGM
        [SerializeField] AudioSource GameBgmPlayer;    //戦闘BGM

        [SerializeField] AudioSource GsePlayer;　　　　//効果音
        [SerializeField] AudioSource GseChangeSound;   //効果音変える時提示音 
        [SerializeField] AudioSource Attack;　　　　　 //攻撃
        [SerializeField] AudioSource Attack_E;　　　 　//攻撃無効
        [SerializeField] AudioSource CRE_Attack;       //近距離
        [SerializeField] AudioSource CRE_Hit;          //近距離攻撃された
        [SerializeField] AudioSource LRE_Attack;       //遠距離
        [SerializeField] AudioSource LRE_Hit;          //遠距離攻撃された
        [SerializeField] AudioSource Dash;             //ダッシュ   

        private const float MIN_PITCH = 0.9f;
        private const float MAX_PITCH = 1.1f;

        private float bgmValue = 1.0f;
        private float gseValue = 1.0f;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            // Disable play on awake for all audio sources
            BgmPlayer.playOnAwake = false;
            WinBgm.playOnAwake = false;
            LoseBgm.playOnAwake = false;
            GameBgmPlayer.playOnAwake = false;
            GsePlayer.playOnAwake = false;
            GseChangeSound.playOnAwake = false;
            Attack.playOnAwake = false;
            Attack_E.playOnAwake = false;
            CRE_Attack.playOnAwake = false;
            CRE_Hit.playOnAwake = false;
            LRE_Attack.playOnAwake = false;
            LRE_Hit.playOnAwake = false;
            Dash.playOnAwake = false;
        }

        public bool IsBgmPlaying()
        {
            return BgmPlayer.isPlaying;
        }

        public void StopGameBgmPlayer()
        {
            // 这里添加停止游戏背景音乐的代码
            if (GameBgmPlayer.isPlaying)
                GameBgmPlayer.Stop();
        }

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
                Debug.Log($"Playing sound: {source.clip.name} at volume: {volume}");
                source.volume = volume;
                source.Play();
            }
            else
            {
                //Debug.LogError("AudioSource is null. Cannot play sound.");
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

        #region GBM停止
        public void StopAllNonBgmPlayers()
        {
            if (WinBgm.isPlaying)
                WinBgm.Stop();

            if (LoseBgm.isPlaying)
                LoseBgm.Stop();

            if (GameBgmPlayer.isPlaying)
                GameBgmPlayer.Stop();
        }

        public void StopAllNonWinBgms()
        {
            if (BgmPlayer.isPlaying)
                BgmPlayer.Stop();

            if (LoseBgm.isPlaying)
                LoseBgm.Stop();

            if (GameBgmPlayer.isPlaying)
                GameBgmPlayer.Stop();
        }

        public void StopAllNonLoseBgms()
        {
            if (BgmPlayer.isPlaying)
                BgmPlayer.Stop();

            if (WinBgm.isPlaying)
                WinBgm.Stop();

            if (GameBgmPlayer.isPlaying)
                GameBgmPlayer.Stop();
        }

        public void StopAllNonGameBgmPlayers()
        {
            if (BgmPlayer.isPlaying)
                BgmPlayer.Stop();

            if (WinBgm.isPlaying)
                WinBgm.Stop();

            if (LoseBgm.isPlaying)
                LoseBgm.Stop();
        }
        #endregion

        #region 音效
        public void PlayBgmPlayer()
        {
            PlaySfx(BgmPlayer, BgmValue);
        }

        public void PlayWinBgm()
        {
            PlaySfx(WinBgm, BgmValue);
        }

        public void PlayLoseBgm()
        {
            PlaySfx(LoseBgm, BgmValue);
        }

        public void PlayGameBgmPlayer()
        {
            PlaySfx(GameBgmPlayer, BgmValue);
        }

        public void PlayGsePlayer()
        {
            PlaySfx(GsePlayer, GseValue);
        }

        public void PlayGseChangeSound()
        {
            PlaySfx(GseChangeSound, GseValue);
        }

        public void PlayAttack()
        {
            PlaySfx(Attack, GseValue);
        }

        public void PlayAttack_E()
        {
            PlaySfx(Attack_E, GseValue);
        }

        public void PlayCRE_Attack()
        {
            PlaySfx(CRE_Attack, GseValue);
        }

        public void PlayCRE_Hit()
        {
            PlaySfx(CRE_Hit, GseValue);
        }

        public void PlayLRE_Attack()
        {
            PlaySfx(LRE_Attack, GseValue);
        }

        public void PlayLRE_Hit()
        {
            PlaySfx(LRE_Hit, GseValue);
        }

        public void PlayDash()
        {
            PlaySfx(Dash, GseValue);
        }
        #endregion 
    }
}
