using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Image;
using System.Drawing;
using static UnityEngine.RuleTile.TilingRuleOutput;
using NUnit.Framework.Constraints;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        // コンポーネントの取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // タイマーの初期化
        timerAnimation = 0f;

        NullCheck();

        // 画像の初期化
        spriteRenderer.sprite = spritesWalk[0];

        //ジャンプカウント初期化
        JumpCountImage[0].enabled = true;
        for (int i = 1; i < JumpCountImage.Length; i++)
        {
            JumpCountImage[i].enabled = false;  //全て非表示にする
        }

        //サウンドの初期化
        audioSource.clip = moveSE;

        //プレイヤーコライダーの初期化
        playerCollider2D = GetComponent<Collider2D>();

        //移動速度の初期化
        moveSpeed = defaultMoveSpeed;

        //方向初期化
        input = Input.GetAxisRaw("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        //連続ジャンプの時間内なら
        if(groundTime < jumpComboTime)
        {
            float gageRatio = groundTime / jumpComboTime;
            JumpGageImage.fillAmount = 1.0f - gageRatio;
        }
        //連続ジャンプの時間外か一回目のジャンプなら
        if(groundTime > jumpComboTime || jumpCount == 0)
        {
            JumpGageImage.enabled = false;   //ゲージを非表示
        }

        //ジャンプキーを押したとき(地面についている場合)
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            return;
        }

        //もしプレイヤーが止まっていなければ
        if (input != 0)
        {
            if (RayWidth < 0) RayWidth *= -1;   //レイの幅を元に戻す
            RayWidth *= input;  //方向の向きの判定が行くようにする
        }

       
        //地面に接しているとき
        if (isColliding)
        {
            if (!isGround && state == State.FALL)
            {
                state = State.LAND; //もし地面に接して1回目ならアニメーションを着地にする
                audioSource.PlayOneShot(landSE); //着地SE再生
            }
            //止まっているとき
            if (input == 0)
            {
                if (state != State.LAND)  //着地アニメーションではなく、前と同じ位置なら
                {
                    state = State.STOP; //アニメーションを待機にする
                }
            }
            //動いているとき
            else
            {
                state = State.WALK; //アニメーションを歩きにする
                audioSource.clip = moveSE;  //SEを歩きにする
            }
            isGround = true; //上下に動いていない場合
            if(isGround && rbody2D.velocity.y <= 0) moveSpeed = defaultMoveSpeed;   //移動速度を通常に戻す
            groundTime += Time.deltaTime;   //地面との接触時間の加算
            if (groundTime < jumpComboTime && jumpCount != 0) JumpGageImage.enabled = true;   //ゲージを表示
        }
        //地面に接していないとき
        else
        {
            isGround = false;
            //上昇
            if (rbody2D.velocity.y > 0)
            {
                state = State.JUMP; //アニメーションをジャンプにする
                groundTime = 0; //地面と離れたら初期化
                JumpGageImage.enabled = false;   //ゲージを非表示
            }
            //下降
            else if (rbody2D.velocity.y < 0)
            {
                state = State.FALL; //アニメーションを落下にする
                groundTime = 0; //地面と離れたら初期化
                JumpGageImage.enabled = false;   //ゲージを非表示
            }
        }

        //地面についていて、3回ジャンプしていたら
        if (isGround && rbody2D.velocity.y <= 0 && (jumpCount == 0 || groundTime > jumpComboTime))
        {
            for(int i = 0; i < JumpCountImage.Length; i++)
            {
                JumpCountImage[i].enabled = false;
            }
        }

        //アニメーション・SE差分
        switch (state)
        {
            case State.STOP:
                Animation(spritesStop);
                if(timerAudio >= intervalAudio) audioSource.Stop(); //音を止める
                break;
            case State.WALK:
                Animation(spritesWalk);
                PlaySE(moveSE); //歩きSE再生
                break;
            case State.RUN:
                Animation(spritesRun);
                break;
            case State.JUMP:
                Animation(spritesJump);
                break;
            case State.FALL:
                Animation(spritesFall);
                audioSource.Stop(); //音を止める
                break;
            case State.LAND:
                Animation(spritesLanded);
                break;
        }

        if (timerAudio < intervalAudio)
        {
            // インターバル中
            timerAudio += Time.deltaTime;
        }
        if (groundTime > jumpComboTime) jumpCount = 0;  //連続ジャンプではない場合は0
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //障害物のレイヤー以外なら終了
        ContactPoint2D maxPoint = collision.contacts[0];    //一番高い衝突点
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //一番小さい衝突点を取得
            if (maxPoint.point.y < contact.point.y)
            {
                maxPoint = contact;
            }
        }

        float angle = Vector2.Angle(maxPoint.normal, Vector2.up); // 上方向との角度
        //水平じゃないなら
        if (angle != 90.0f && angle != 270.0f)
        {
            if (maxPoint.point.y <= transform.position.y)
            {
                isColliding = true;
                return;
            }

        }
        //ぶつかったコリジョンがプレイヤーよりも低い位置にいたら
        if (collision.collider.bounds.max.y <= playerCollider2D.bounds.min.y)
        {
            isColliding = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //障害物のレイヤー以外なら終了
        ContactPoint2D maxPoint = collision.contacts[0];    //一番高い衝突点
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //一番小さい衝突点を取得
            if (maxPoint.point.y < contact.point.y)
            {
                maxPoint = contact;
            }
        }

        float angle = Vector2.Angle(maxPoint.normal, Vector2.up); // 上方向との角度
        //水平じゃないなら
        if (angle != 90.0f && angle != 270.0f)
        {
            if (maxPoint.point.y <= transform.position.y)
            {
                isColliding = true;
                return;
            }

        }
        //ぶつかったコリジョンがプレイヤーよりも低い位置にいたら
        if (collision.collider.bounds.max.y <= playerCollider2D.bounds.min.y)
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //障害物のレイヤー以外なら終了
        isColliding = false;
    }

    void FixedUpdate()
    {
        //ジャンプ
        if (isJump && isGround)
        {
            rbody2D.velocity = new Vector2(rbody2D.velocity.x, 0);  //y軸の力を0にする
            float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //ジャンプ力の倍率をジャンプの回数によって変える
            rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //ジャンプ
            if(jumpCount == 2) moveSpeed = jumpMoveSpeed;  //移動速度をジャンプ中の速度に変更

            AudioSource tempAudioSource = gameObject.AddComponent<AudioSource>();   // 一時的な AudioSource を作成
            tempAudioSource.pitch = defaultPitch + jumpCount * pitchMultiplier;  // ピッチを変更
            tempAudioSource.PlayOneShot(jumpSE);    // 音を再生
            Destroy(tempAudioSource, jumpSE.length);  // 音の長さ分後に削除


            //ジャンプカウントの表示
            //1つ目
            if (jumpCount >= 0) JumpCountImage[0].enabled = true;
            else JumpCountImage[0].enabled = false;
            //2つ目
            if (jumpCount >= 1) JumpCountImage[1].enabled = true;
            else JumpCountImage[1].enabled = false;
            //3つ目
            if (jumpCount >= 2) JumpCountImage[2].enabled = true;
            else JumpCountImage[2].enabled = false; 

            jumpCount++;    //ジャンプ回数を増やす
            jumpCount %= maxJumpCombo; //最大連続ジャンプの回数で割ったあまりを取ることで連続ジャンプが最大以上に発生しないようにする
            isJump = false;
        }
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed, 0, 0);
            transform.eulerAngles = new Vector3(0, 180, 0);
            //プレイヤー以外の回転を直す

            timerAudio = 0f;
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
            timerAudio = 0f;
        }
        input = Input.GetAxisRaw("Horizontal"); //方向を取得
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
                    if (state == State.LAND)
                    {
                        state = State.STOP;   //もし着地のアニメーションが終わったら待機に変更
                        return; //画像が戻らないようにする
                    }
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

    private void PlaySE(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;   //同じ音で再生中なら終了
        }
        audioSource.Stop(); //音を止める
        audioSource.clip = clip;    //音を変更
        audioSource.Play(); //音を再生
    }


    //プレイヤーの行動
    private enum State
    {
        STOP = 0,   //待機
        WALK = 1,   //歩き
        RUN = 2,    //走り
        JUMP = 3,   //ジャンプ
        FALL = 4,   //落下
        LAND = 5, //着地
    }


    private bool isColliding = false;   //コリジョンにぶつかった時
    Collider2D playerCollider2D = null; //プレイヤーのコライダー
    Rigidbody2D rbody2D;  //rigidbody2dを取得
    private float input;    //プレイヤーの方向
    private State state = State.STOP;   //プレイヤーの現在の動き 
    private float moveSpeed = 0;
    [Header("移動速度")]
    public float defaultMoveSpeed = 0.08f; //移動速度
    public float jumpMoveSpeed = 0.15f;  //ジャンプ中の移動速度
    [Header("ジャンプ")]
    public float jumpPower = 350f;    //ジャンプ力
    public float jumpMultiplier = 1.2f;    //連続ジャンプ時のジャンプ力の倍率
    public float jumpComboTime = 1;   //連続ジャンプの猶予時間
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
    public UnityEngine.UI.Image JumpGageImage = null;
    [SerializeField] public UnityEngine.UI.Image[] JumpCountImage;
    private SpriteRenderer spriteRenderer = null;
    [Header("アニメーション画像")]
    [SerializeField] private Sprite[] spritesStop;
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    [SerializeField] private Sprite[] spritesFall;
    [SerializeField] private Sprite[] spritesLanded;
    private float timerAnimation = 0f;
    [SerializeField, Header("アニメーション間隔")]
    private float INTERVAL_ANIMATION = 0f;

    private AudioSource audioSource;
    private float intervalAudio = 0.5f;    //SEが必ず再生される時間
    private float timerAudio = 0f;
    [Header("SE")]
    public AudioClip moveSE;
    public AudioClip jumpSE;
    public AudioClip landSE;
    public float defaultPitch = 1.0f; //ジャンプの初期ピッチ
    public float pitchMultiplier = 0f;  //ピッチの上昇幅
}
