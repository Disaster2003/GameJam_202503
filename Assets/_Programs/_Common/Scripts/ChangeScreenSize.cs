using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChangeScreenSize : MonoBehaviour
{
    private TextMeshProUGUI txtScreenSize;
    [Header("解像度")]
    [SerializeField] private int[] screenWidth;
    [SerializeField] private int[] screenHeight;
    private enum STATE_SCREEN
    {
        FULL, // フルスクリーン
        FHD,  // 1920 * 1080
        HDTV, // 1280 * 720
    }
    private static STATE_SCREEN state_screen;
    private const int indexMaxSTATE_SCREEN = (int)STATE_SCREEN.HDTV;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        txtScreenSize = GetComponent<TextMeshProUGUI>();

        NullCheck();

        // フルスクリーン化
        Screen.fullScreen = true;

        // テキストの初期化
        txtScreenSize.text = "フルスクリーン";

        // 状態の初期化
        state_screen = STATE_SCREEN.FULL;
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
        if (txtScreenSize == null)
        {
            Debug.LogError("TextMeshProUGUIコンポーネントが取得できません");
        }
        if (screenWidth.Length == 0 || screenHeight.Length == 0
            || screenWidth.Length != screenHeight.Length)
        {
            Debug.LogError("解像度が未設定です");
        }
    }

    /// <summary>
    /// スクリーンを拡大する
    /// </summary>
    public void OnClickLeft()
    {
        if (state_screen == 0)
        {
            // 最小サイズに
            state_screen = (STATE_SCREEN)indexMaxSTATE_SCREEN;
        }
        else state_screen--;
        ChangeTextAndScreen();
    }

    /// <summary>
    /// スクリーンを縮小する
    /// </summary>
    public void OnClickRight()
    {
        if (state_screen == (STATE_SCREEN)indexMaxSTATE_SCREEN)
        {
            // フルスクリーンに
            state_screen = 0;
        }
        else state_screen++;
        ChangeTextAndScreen();
    }

    /// <summary>
    /// テキストと解像度を変更する
    /// </summary>
    private void ChangeTextAndScreen()
    {
        if (screenWidth[(int)state_screen] == 0)
        {
            // フルスクリーン
            Screen.fullScreen = true;
            txtScreenSize.text = "フルスクリーン";
        }
        else
        {
            // フルスクリーン以外
            Screen.fullScreen = false;
            txtScreenSize.text = $"{screenWidth[(int)state_screen]} * {screenHeight[(int)state_screen]}";
            Screen.SetResolution(screenWidth[(int)state_screen], screenHeight[(int)state_screen], Screen.fullScreen);
        }
    }
}
