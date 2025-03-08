using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class MoveGround : MonoBehaviour
{
    /// <summary>
    /// 足場のタイプ
    /// </summary>
    private enum Obstacles
    {
        Normal,
        LateralMovementRight,
        LateralMovementLeft,
        VerticalMovement,
        Target,
        RotateX,
        RotateY,
        RotateZ,
        Time_disappear,
        Exit_disappear,
        Exit_disappear_Cnt,
        Attack,
        Blinking,
        Penetration,
    }

    //[SerializeField, Header("プレイヤーのポジション")] private GameObject player;
    [SerializeField, Header("階層")] private float floor;

    [SerializeField, Header("障害物の種類")] private Obstacles obstacles1;
    [SerializeField, Header("障害物の種類")] private Obstacles obstacles2;

    [SerializeField, Header("壊れる音(disappear)")] private AudioClip breakClip;

    [SerializeField, Header("横移動の速さ(LateralMovement)")] private float lateraSpeed;
    [SerializeField, Header("可動域　横(LateralMovementRight)")] private float lateraRange;
    [SerializeField, Header("縦移動の速さ(VerticalMovement)")] private float verticalSpeed;
    [SerializeField, Header("可動域　縦(VerticalMovement)")] private float verticalRange;
    [SerializeField, Header("回転の速さ(Rotate)")] private float rotateSpeed;
    [SerializeField, Header("足場の壊れる時間(disappear)")] private float breakTime;
    [SerializeField, Header("足場の消える時間(disappear)")] private float disappearTime;
    [SerializeField, Header("足場の離れる回数(Exit_disappear_Cnt)")] private int exitCnt;
    [SerializeField, Header("画像差し替え(disappear)")] private Sprite newSprite;
    [SerializeField, Header("所定の座標(Target)")] private Transform[] targets;
    [SerializeField, Header("速さ(Attack)")] private float attackSpeed;
    [SerializeField, Header("プレイヤーの補正値(Attack)")] private float offset;
    [SerializeField, Header("画面端のx座標(Attack)")] private float attackX;
    [SerializeField, Header("間隔(Attack.Blinking)")] private float interval;

    private int currentTargetIndex = 0; // 現在のターゲットインデックス

    /// <summary>
    /// 元の画像保存
    /// </summary>
    private Sprite saveSprite;

    /// <summary>
    /// 音源出す
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// プレイヤーのオブジェクト
    /// </summary>
    private GameObject player;

    /// <summary>
    /// playerの画像の縦幅
    /// </summary>
    float playerWidth;

    /// <summary>
    /// カメラ
    /// </summary>
    private Camera cam;

    /// <summary>
    /// カメラの縦幅
    /// </summary>
    private float width;

    /// <summary>
    /// 物体の初期位置
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// 発射の速度
    /// </summary>
    private Vector3 moveDirection;

    private Collider2D collision2D;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// タイマー
    /// </summary>
    private float time;

    /// <summary>
    /// Blinkingのフラグ
    /// </summary>
    private bool isDiscover;


    /// <summary>
    /// 触れて時間差で消えるフラグ
    /// </summary>
    private bool isTime;

    /// <summary>
    /// 動いて消えるフラグ
    /// </summary>
    private bool isExit;

    /// <summary>
    /// 乗るフラグ
    /// </summary>
    private bool isTach;

    /// <summary>
    /// 妨害フラグ
    /// </summary>
    private bool isAttack;

    /// <summary>
    /// 貫通フラグ
    /// </summary>
    private bool isPenetration;

    /// <summary>
    /// 実効値
    /// </summary>
    private float index;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;

        if (cam != null)
        {
            width = GetCameraWidth(cam);
        }

        playerWidth = player.GetComponent<Collider2D>().bounds.size.y / 2;

        time = 0;
        isTime = false;
        isExit = false;
        isTach = false;
        isAttack = false;
        isDiscover = false;
        isPenetration = false;
        index = 0;
        startPosition = transform.position;
        moveDirection = new Vector3(-1, 0, 0);
        collision2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        saveSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        SetObstacles(obstacles1);
        SetObstacles(obstacles2);

        if (transform.position.y <= player.transform.position.y - playerWidth)
        {
            isPenetration = true;
        }
        else if (transform.position.y >= player.transform.position.y)
        {
            isPenetration = false;
        }

        if (player.transform.position.y > width + (2 * (floor * width))
           || player.transform.position.y < (2 * ((floor - 1) * width)) + width)
        {
            collision2D.enabled = true;
            spriteRenderer.sprite = saveSprite;
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// カメラの縦幅
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    private float GetCameraWidth(Camera cam)
    {
        if (cam.orthographic)
        {
            // 正射影カメラ (Orthographic) の場合
            return cam.orthographicSize;
        }
        else
        {
            // 透視投影カメラ (Perspective) の場合
            float height = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * cam.transform.position.z;
            return height;
        }
    }


    /// <summary>
    /// Obstaclesの選択結果
    /// </summary>
    /// <param name="obstacles"></param>
    private void SetObstacles(Obstacles obstacles)
    {
        switch (obstacles)
        {
            case Obstacles.LateralMovementRight:
            case Obstacles.LateralMovementLeft:
                isTach = true;
                LateralMove(obstacles);
                break;
            case Obstacles.VerticalMovement:
                isTach = true;
                VerticalMove();
                break;
            case Obstacles.Target:
                isTach = true;
                TargetMove();
                break;
            case Obstacles.RotateX:
            case Obstacles.RotateY:
            case Obstacles.RotateZ:
                RotateMove(obstacles);
                break;
            case Obstacles.Time_disappear:
                DisappearMove(ref isTime);
                break;
            case Obstacles.Exit_disappear:
            case Obstacles.Exit_disappear_Cnt:
                DisappearMove(ref isExit);
                break;
            case Obstacles.Attack:
                AttackMove();
                break;
            case Obstacles.Blinking:
                BlinkingMove();
                break;
            case Obstacles.Penetration:
                PenetrationMove();
                break;
        }
    }

    /// <summary>
    /// 横の移動
    /// </summary>
    private void LateralMove(Obstacles obstacles)
    {
        float newX;
        if (obstacles == Obstacles.LateralMovementRight)
        {
            newX = Mathf.PingPong(Time.time * lateraSpeed, lateraRange * 2) - lateraRange;
            transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
        }
        else
        {
             newX = Mathf.PingPong(Time.time * lateraSpeed, lateraRange * 2) - lateraRange;
            transform.position = new Vector3(startPosition.x - newX, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// 縦の移動
    /// </summary>
    private void VerticalMove()
    {
        float newY = Mathf.PingPong(Time.time * verticalSpeed, verticalRange * 2);
        transform.position = new Vector3(transform.position.x, newY + startPosition.y, transform.position.z);
    }

    /// <summary>
    /// 所定の位置
    /// </summary>
    private void TargetMove()
    {
        if (targets.Length == 0) return;

        // 現在のターゲット位置に向かって移動
        Vector3 targetPosition = (currentTargetIndex < targets.Length) ? targets[currentTargetIndex].position : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, lateraSpeed * Time.deltaTime);

        // ターゲットまたは初期位置に近づいたら次のターゲットへ
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (currentTargetIndex < targets.Length)
            {
                currentTargetIndex++; // 次のターゲットへ
            }
            else
            {
                currentTargetIndex = 0; // すべてのターゲットを回ったら初期位置に戻る
            }
        }
    }

    /// <summary>
    /// 回転(Rotate)の関数
    /// </summary>
    /// <param name="obstacles">回転軸</param>
    private void RotateMove(Obstacles obstacles)
    {
        if(obstacles == Obstacles.RotateX)
        {
            transform.Rotate(Vector3.right * -rotateSpeed * Time.deltaTime);
        }
        else if(obstacles == Obstacles.RotateY)
        {
            transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 妨害(敵)
    /// </summary>
    private void AttackMove()
    {
        if (time < 0)
        {
            if (transform.position.y >= player.transform.position.y - offset
                && transform.position.y <= player.transform.position.y + offset)
            {
                isAttack = true;
            }
        }

        if(isAttack)
        {
            //左に流れる場合
            if (startPosition.x > attackX)
            {
                transform.position += moveDirection * attackSpeed * Time.deltaTime;

                //画面端に行ったとき
                if (transform.position.x < attackX)
                {
                    time = interval;
                    transform.position = startPosition;
                    isAttack = false;
                }
            }
            //右に流れる場合
            else
            {
                transform.position -= moveDirection * attackSpeed * Time.deltaTime;

                //画面端に行ったとき
                if (transform.position.x > attackX)
                {
                    time = interval;
                    transform.position = startPosition;
                    isAttack = false;
                }
            }
        }
    }

    /// <summary>
    /// 時間差で現れたり消えたり
    /// </summary>
    private void BlinkingMove()
    {
        if(time < 0)
        {
            if(isDiscover)
            {
                collision2D.enabled = false;
                spriteRenderer.enabled = false;
            }
            else
            {
                collision2D.enabled = true;
                spriteRenderer.enabled = true;
            }
            isDiscover = !isDiscover;
            time = interval;
        }
    }
    private void PenetrationMove()
    {
        if(isPenetration)
        {
            collision2D.enabled = true;
        }
        else
        {
            collision2D.enabled= false;
        }
    }

    /// <summary>
    /// disappear型の処理
    /// </summary>
    /// <param name="flag">対応するフラグ</param>
    private void DisappearMove(ref bool flag)
    {
        
        if(newSprite == null)
        {
            Debug.LogError("newSpriteはない");
        }
        if (flag && time < 0 && spriteRenderer.sprite == newSprite)
        {
            spriteRenderer.enabled = false;
            spriteRenderer.sprite = saveSprite;
            flag = false;
        }
        else if (flag && time < 0 && index >= exitCnt)
        {
            spriteRenderer.sprite = newSprite;
            collision2D.enabled = false;
            time = breakTime;
            index = 0;
            audioSource.PlayOneShot(breakClip);
            
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPenetration)
        {
            time = disappearTime;
            isTime = true;
            isExit = false;
            if (collision.gameObject.CompareTag("Player") && isTach)
            {
                // 触れたobjの親を移動床にする
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPenetration)
        {
            time = 0;
            isExit = true;
            index++;
            if (collision.gameObject.CompareTag("Player") && isTach)
            {
                // 触れたobjの親を移動床にする
                collision.transform.SetParent(null);
            }
        }
    }
}
