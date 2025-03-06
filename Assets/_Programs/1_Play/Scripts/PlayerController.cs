using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static UnityEngine.UI.Image;
using System.Drawing;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // タイマーの初期化
        timerAnimation = 0f;

        NullCheck();

        // 画像の初期化
        spriteRenderer.sprite = spritesWalk[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            return;
        }

        Vector2 StartPositionL = transform.position; //衝突判定の開始位置(プレイヤーの中心)
        Vector2 StartPositionR = transform.position + new Vector3(0.3f, 0, 0); //衝突判定の開始位置(プレイヤーの中心)
        int LayerObject = LayerMask.GetMask(GroundLayer);   //衝突判定をするレイヤー
        UnityEngine.Color color = UnityEngine.Color.red;
        Debug.DrawRay(StartPositionL, Vector2.down, color, RayDistance);
        Debug.DrawRay(StartPositionR, Vector2.down, color, RayDistance);
        RaycastHit2D HitObjectL = Physics2D.Raycast(StartPositionL, Vector2.down, RayDistance, LayerObject);    //下向きにして地面と接しているかどうか
        RaycastHit2D HitObjectR = Physics2D.Raycast(StartPositionR, Vector2.down, RayDistance, LayerObject);    //下向きにして地面と接しているかどうか

        if (rbody2D.velocity.y <= 0)
        {
            if (HitObjectL || HitObjectR)
            {
                isGround = true;
                groundTime += Time.deltaTime;   //地面との接触時間の加算
            }
        }
        else
        {
            isGround = false;
            groundTime = 0; //地面と離れたら初期化
        }

        Animation(spritesWalk);
    }

    /// <summary>
    /// nullチェック
    /// </summary>
    private void NullCheck()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererコンポーネントが取得できません");
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
    
    void FixedUpdate()
    {
        //ジャンプ
        if (isJump && isGround)
        {
            if (groundTime > jumpComboTime) jumpCount = 0;  //連続ジャンプではない場合は0
            float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //ジャンプ力の倍率をジャンプの回数によって変える
            rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //ジャンプ

            jumpCount++;    //ジャンプ回数を増やす
            jumpCount %= maxJumpCombo; //最大連続ジャンプの回数で割ったあまりを取ることで連続ジャンプが最大以上に発生しないようにする
            isJump = false;
        }
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed, 0, 0);
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed, 0, 0);
        }
    }

    Rigidbody2D rbody2D;  //rigidbody2dを取得
    public float moveSpeed = 0.1f; //移動速度
    public float jumpPower = 300f;    //ジャンプ力
    private int jumpCount = 0;  //連続ジャンプの回数
    private float groundTime = 0;   //地面と接触している時間
    private bool isGround  = false; //地面と接触しているかどうか
    private bool isJump = false;    //ジャンプキー入力
    private static int maxJumpCombo = 3;    //連続ジャンプの回数
    public float jumpMultiplier = 1.2f;    //連続ジャンプ時のジャンプ力の倍率
    public float jumpComboTime = 600;   //連続ジャンプの猶予時間
    public string GroundLayer = "Block"; //地面の判定をするレイヤー
    public float RayDistance = 0.5f;    //レイの長さ

    private SpriteRenderer spriteRenderer = null;
    [Header("アニメーション画像")]
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    private float timerAnimation = 0f;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION = 0f;
}
