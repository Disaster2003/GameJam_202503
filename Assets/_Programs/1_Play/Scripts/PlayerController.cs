using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
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

        //移動速度の初期化
        moveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //連続ジャンプの時間内なら
        if(groundTime < jumpComboTime)
        {
            float gageRatio = groundTime / jumpComboTime;
            //JumpGageSpriteRender.transform.localScale = new Vector3(1.0f - gageRatio, 0.1f, 1.0f);  // ゲージを伸縮 (X軸の大きさを変更)
            JumpGageImage.fillAmount = 1.0f - gageRatio;
        }
        else if(groundTime > jumpComboTime)
        {
           //JumpGageSpriteRender.enabled = false;   //ゲージを非表示
            JumpGageImage.enabled = false;   //ゲージを非表示
        }

        //ジャンプキーを押したとき
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            return;
        }

        float input = Input.GetAxisRaw("Horizontal");   //プレイヤーの方向を読み取る
        //もしプレイヤーが止まっていなければ
        if (input != 0)
        {
            if (RayWidth < 0) RayWidth *= -1;   //レイの幅を元に戻す
            RayWidth *= input;  //方向の向きの判定が行くようにする
        }

            Vector2 StartPositionL = transform.position; //衝突判定の開始位置(プレイヤーの中心)
        Vector2 StartPositionR = transform.position + new Vector3(RayWidth, 0, 0); //衝突判定の開始位置
        int LayerObject = LayerMask.GetMask(GroundLayer);   //衝突判定をするレイヤー
        UnityEngine.Color color = UnityEngine.Color.red;
        Debug.DrawRay(StartPositionL, Vector2.down, color, RayDistance);
        Debug.DrawRay(StartPositionR, Vector2.down, color, RayDistance);
        RaycastHit2D HitObjectL = Physics2D.Raycast(StartPositionL, Vector2.down, RayDistance, LayerObject);    //下向きにして地面と接しているかどうか
        RaycastHit2D HitObjectR = Physics2D.Raycast(StartPositionR, Vector2.down, RayDistance, LayerObject);    //下向きにして地面と接しているかどうか

        //地面に接しているとき
        if (HitObjectL || HitObjectR)
        {
            moveSpeed = defaultMoveSpeed;   //移動速度を通常に戻す
            if (input == 0)
            {
                state = State.STOP; //アニメーションを待機にする
            }
            else
            {
                state = State.WALK; //アニメーションを歩きにする
            }

            //上下に動いていないとき
            if (rbody2D.velocity.y == 0)
            {
                isGround = true;
                groundTime += Time.deltaTime;   //地面との接触時間の加算
            }

            //if(groundTime < jumpComboTime) JumpGageSpriteRender.enabled = true;    //ゲージを表示
            if (groundTime < jumpComboTime) JumpGageImage.enabled = true;   //ゲージを表示
        }
        //地面に接していないとき
        else
        {
            if (rbody2D.velocity.y >= 0)
            {
                state = State.JUMP; //アニメーションをジャンプにする
            }
            else if(rbody2D.velocity.y <= 0)
            {
                state = State.FALL; //アニメーションを落下にする
            }

            isGround = false;
            groundTime = 0; //地面と離れたら初期化
            //JumpGageSpriteRender.enabled = false;   //ゲージを非表示
            JumpGageImage.enabled = false;   //ゲージを非表示
        }

        // 進行方向へ向きを変える
        if (input > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (input < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //アニメーション差分
        switch (state)
        {
            case State.STOP:
                Animation(spritesStop);
                break;
            case State.WALK:
                Animation(spritesWalk);
                break;
            case State.RUN:
                Animation(spritesRun);
                break;
            case State.JUMP:
                Animation(spritesJump);
                break;
            case State.FALL:
                Animation(spritesFall);
                break;

        }
        
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
            if(jumpCount == 0) moveSpeed = jumpMoveSpeed;  //移動速度をジャンプ中の速度に変更
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

    //プレイヤーの行動
    enum State
    {
        STOP = 0,   //待機
        WALK = 1,   //歩き
        RUN = 2,    //走り
        JUMP = 3,   //ジャンプ
        FALL = 4,   //落下
    }


    Rigidbody2D rbody2D;  //rigidbody2dを取得
    private State state = State.STOP;   //プレイヤーの現在の動き   
    private float moveSpeed = 0;
    [Header("移動速度")]
    public float defaultMoveSpeed = 0.1f; //移動速度
    public float jumpMoveSpeed = 0.3f;  //ジャンプ中の移動速度
    [Header("ジャンプ")]
    public float jumpPower = 300f;    //ジャンプ力
    public float jumpMultiplier = 1.2f;    //連続ジャンプ時のジャンプ力の倍率
    public float jumpComboTime = 600;   //連続ジャンプの猶予時間
    private int jumpCount = 0;  //連続ジャンプの回数
    private static int maxJumpCombo = 3;    //連続ジャンプの回数
    private bool isJump = false;    //ジャンプキー入力
    [Header("地面の判定")]
    public string GroundLayer = "Block"; //地面の判定をするレイヤー
    public float RayDistance = 0.5f;    //レイの長さ
    public float RayWidth = 0.3f;   //レイの幅
    private float groundTime = 0;   //地面と接触している時間
    private bool isGround  = false; //地面と接触しているかどうか

    [Header("ジャンプゲージ")]
    //public SpriteRenderer JumpGageSpriteRender = null;
    public UnityEngine.UI.Image JumpGageImage = null;
    private SpriteRenderer spriteRenderer = null;
    [Header("アニメーション画像")]
    [SerializeField] private Sprite[] spritesStop;
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    [SerializeField] private Sprite[] spritesFall;
    private float timerAnimation = 0f;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION = 0f;
}
