using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FrameWork.Utils;

namespace FrameWork.SceneLoader
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        private Image _transitionImage;
        private const float FADE_TIME = 3.5f;

        private Color color = new Color(0, 0, 0, 0);

        /*
            private const string GAMEPLAY = "Gameplay";
            private const string MAIN_MENU = "MainMenu";
            private const string SCORING = "Scoring";
         */

        private void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // private async UniTask LoadingCoroutine(string sceneName)
        // {
        //     //シーンの非同期ロードを開始
        //     var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        //     loadingOperation.allowSceneActivation = false;
        //
        //     _transitionImage.gameObject.SetActive(true);
        //
        //     // Fade out
        //     while (color.a < 1f)
        //     {
        //         color.a = Mathf.Clamp01(color.a += Time.unscaledDeltaTime / FADE_TIME);
        //         _transitionImage.color = color;
        //
        //         await UniTask.Yield(PlayerLoopTiming.Update);
        //     }
        //
        //     // シーンのロードが完全に終わるまで待つ
        //     await UniTask.WaitUntil(() => loadingOperation.progress >= 0.9f);
        //
        //     loadingOperation.allowSceneActivation = true;
        //
        //     // Fade in
        //     while (color.a > 0f)
        //     {
        //         color.a = Mathf.Clamp01(color.a -= Time.unscaledDeltaTime / FADE_TIME);
        //         _transitionImage.color = color;
        //
        //         await UniTask.Yield(PlayerLoopTiming.Update);
        //     }
        //
        //     _transitionImage.gameObject.SetActive(false);
        // }

        // public void LoadGamePlayScene()
        // {
        //     LoadingCoroutine(GAMEPLAY).Forget();
        // }
        //
        // public void LoadMainMenuScene()
        // {
        //     LoadingCoroutine(MAIN_MENU).Forget();
        // }
        //
        // public void LoadScoringScene()
        // {
        //     LoadingCoroutine(SCORING).Forget();
        // }
    }

}