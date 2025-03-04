using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSample : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [Header("アニメーション画像")]
    [SerializeField] private Sprite[] spritesSample;
    private float timerAnimation = 0f;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // タイマーの初期化
        timerAnimation = 0f;

        NullCheck();

        // 画像の初期化
        spriteRenderer.sprite = spritesSample[0];
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
        if (spriteRenderer = null)
        {
            Debug.LogError("SpriteRendererコンポーネントが取得できません");
        }
        if (spritesSample.Length == 0)
        {
            Debug.LogError("画像群が未設定です");
        }
        if (INTERVAL_ANIMATION == 0f)
        {
            Debug.LogError("アニメーション間隔が未設定です");
        }
    }

    /// <summary>
    /// アニメーション
    /// </summary>
    /// <param name="_sprites">アニメーション用画像</param>
    private void Animation(Sprite[] _sprites)
    {
        if (timerAnimation < INTERVAL_ANIMATION)
        {
            // インターバル中
            timerAnimation += Time.deltaTime;
            return;
        }

        // アニメーション
        timerAnimation = 0;
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (spriteRenderer.sprite == _sprites[i])
            {
                if (i == _sprites.Length - 1)
                {
                    // 最初の画像に戻す
                    spriteRenderer.sprite = _sprites[0];
                    return;
                }
                else
                {
                    // 次の画像へ
                    spriteRenderer.sprite = _sprites[i + 1];
                    return;
                }
            }
            else if (i == _sprites.Length - 1)
            {
                // 画像を変更する
                spriteRenderer.sprite = _sprites[0];
                return;
            }
        }
    }
}
