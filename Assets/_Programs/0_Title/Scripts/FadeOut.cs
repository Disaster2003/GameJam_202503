using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image imgPanel;
    private float timerColor;
    [SerializeField, Header("暗転間隔")]
    private float INTERVAL_FADEOUT;

    private bool isFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        imgPanel = GetComponent<Image>();

        // タイマーの初期化
        timerColor = 0f;

        // 状態の初期化
        isFadeOut = false;

        NullCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            if (timerColor >= INTERVAL_FADEOUT)
            {
                // プレイ画面へ
                GameManager.GetInstance.ChangeScene = GameManager.STATE_SCENE.PLAY;
            }

            // フェードアウト
            timerColor += Time.deltaTime;
            imgPanel.color = Color.Lerp(Color.clear, Color.black, timerColor / INTERVAL_FADEOUT);
            return;
        }

        // フェードアウト開始
        if (Input.GetKeyDown(KeyCode.Space)) isFadeOut = true;
    }

    private void NullCheck()
    {
        if(imgPanel == null)
        {
            Debug.LogError("Imageコンポーネントが取得できません");
        }
    }
}
