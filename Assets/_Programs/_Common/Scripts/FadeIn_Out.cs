using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn_Out : MonoBehaviour
{
    private Image imgFadePanel;
    private bool isFadeOut;

    private float timerColor;
    [SerializeField, Header("暗転間隔")]
    private float INTERVAL_FADE;

    [SerializeField, Header("次に遷移するシーン")]
    private GameManager.STATE_SCENE state_scene;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        imgFadePanel = GetComponent<Image>();

        // 状態の初期化
        isFadeOut = false;

        // タイマーの初期化
        timerColor = 0f;

        NullCheck();

        // 色の初期化
        imgFadePanel.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            if (timerColor >= INTERVAL_FADE)
            {
                // 次のシーンへ
                GameManager.GetInstance.ChangeScene = state_scene;
                return;
            }

            // フェードアウト
            imgFadePanel.color = Color.Lerp(Color.clear, Color.black, timerColor / INTERVAL_FADE);
        }
        else
        {
            if (timerColor >= INTERVAL_FADE)
            {
                // 画像の非表示
                imgFadePanel.enabled = false;
                return;
            }

            // フェードイン
            imgFadePanel.color = Color.Lerp(Color.black, Color.clear, timerColor / INTERVAL_FADE);
        }

        // 時間計測
        timerColor += Time.deltaTime;
    }

    /// <summary>
    /// nullチェック
    /// </summary>
    private void NullCheck()
    {
        if (imgFadePanel == null)
        {
            Debug.Log("Imageコンポーネントを取得できません");
        }
    }

    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void StartFadeOut()
    {
        isFadeOut = true;
        imgFadePanel.enabled = true;
    }
}
