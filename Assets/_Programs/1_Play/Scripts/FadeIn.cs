using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image imgPanel;
    private float timerColor;
    [SerializeField, Header("暗転間隔")]
    private float INTERVAL_FADEOUT;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        imgPanel = GetComponent<Image>();

        // タイマーの初期化
        timerColor = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerColor >= INTERVAL_FADEOUT)
        {
            // 自身の破棄
            Destroy(gameObject);
        }

        // フェードアウト
        timerColor += Time.deltaTime;
        imgPanel.color = Color.Lerp(Color.black, Color.clear, timerColor / INTERVAL_FADEOUT);
    }
}
