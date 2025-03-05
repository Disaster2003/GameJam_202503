using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonSample<GameManager>
{
    public enum STATE_SCENE
    {
        TITLE,  // タイトル画面
        PLAY,   // プレイ画面
        RESULT, // 結果画面
    }
    private STATE_SCENE state_scene;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // シーン遷移による破壊の防止
        DontDestroyOnLoad(gameObject);

        // 状態の初期化
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;

        // コンポーネントの取得
        audioSource = GetComponent<AudioSource>();

        NullCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// nullチェック
    /// </summary>
    private void NullCheck()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceコンポーネントが取得できません");
        }
    }

    /// <summary>
    /// シーンを変更する
    /// </summary>
    public STATE_SCENE ChangeScene
    {
        set
        {
            Time.timeScale = 1f;
            state_scene = value;
            SceneManager.LoadSceneAsync((int)state_scene);
        }
    }

    /// <summary>
    /// 効果音を鳴らす
    /// </summary>
    /// <param name="SE">鳴らす効果音</param>
    public void PlaySE(AudioClip SE)
    {
        audioSource.PlayOneShot(SE);
    }
}
