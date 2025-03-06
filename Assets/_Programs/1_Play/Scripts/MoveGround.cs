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
    enum Obstacles
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
    }

    [SerializeField, Header("プレイヤーのポジション")] private GameObject player;
    [SerializeField, Header("カメラの中央からの縦幅半分")] private float widthY;
    [SerializeField, Header("階層")] private float floor;

    [SerializeField, Header("障害物の種類")] private Obstacles obstacles1;
    [SerializeField, Header("障害物の種類")] private Obstacles obstacles2;

    [SerializeField, Header("横移動の速さ(LateralMovement)")] private float lateraSpeed;
    [SerializeField, Header("可動域　横(LateralMovementRight)")] private float lateraRange;
    [SerializeField, Header("縦移動の速さ(VerticalMovement)")] private float verticalSpeed;
    [SerializeField, Header("可動域　縦(VerticalMovement)")] private float verticalRange;
    [SerializeField, Header("回転の速さ(Rotate)")] private float rotateSpeed;
    [SerializeField, Header("足場の消える時間(Time_disappear)")] private float timer;
    [SerializeField, Header("足場の離れる回数(Exit_disappear_Cnt)")] private int exitCnt;
    [SerializeField, Header("所定の座標(Target)")] private Transform[] targets;
    [SerializeField, Header("速さ(Attack)")] private float attackSpeed;
    [SerializeField, Header("プレイヤーの補正値(Attack)")] private float offset;
    [SerializeField, Header("画面端のx座標(Attack)")] private float minX;
    [SerializeField, Header("間隔(Attack)")] private float interval;

    private int currentTargetIndex = 0; // 現在のターゲットインデックス

    /// <summary>
    /// 物体の初期位置
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// 発射の速度
    /// </summary>
    private Vector3 moveDirection;

    private BoxCollider2D collision2D;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// タイマー
    /// </summary>
    float time;


    /// <summary>
    /// 触れて消えるフラグ
    /// </summary>
    private bool isTach;

    /// <summary>
    /// 動いて消えるフラグ
    /// </summary>
    private bool isExit;

    /// <summary>
    /// 実効値
    /// </summary>
    float index;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        isTach = false;
        isExit = false;
        index = 0;
        startPosition = transform.position;
        moveDirection = new Vector3(-1, 0, 0);
        collision2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        SetObstacles(obstacles1);
        SetObstacles(obstacles2);

        if (player.transform.position.y > widthY + (2 * (floor * widthY))
           || player.transform.position.y < (2 * ((floor - 1) * widthY)) + widthY)
        {
            collision2D.enabled = true;
            spriteRenderer.enabled = true;
        }
    }


    private void SetObstacles(Obstacles obstacles)
    {
        switch (obstacles)
        {
            case Obstacles.LateralMovementRight:
            case Obstacles.LateralMovementLeft:
                LateralMove(obstacles);
                break;
            case Obstacles.VerticalMovement:
                VerticalMove();
                break;
            case Obstacles.Target:
                TargetMove();
                break;
            case Obstacles.RotateX:
            case Obstacles.RotateY:
            case Obstacles.RotateZ:
                RotateMove(obstacles);
                break;
            case Obstacles.Time_disappear:
                if (isTach)
                {
                    if (time < 0)
                    {
                        collision2D.enabled = false;
                        spriteRenderer.enabled = false;
                        isTach = false;
                    }
                }
                break;
            case Obstacles.Exit_disappear:
                if (isExit)
                {
                    collision2D.enabled = false;
                    spriteRenderer.enabled = false;
                    isExit = false;
                }
                break;
            case Obstacles.Exit_disappear_Cnt:
                if (isExit && index == exitCnt)
                {
                    collision2D.enabled = false;
                    spriteRenderer.enabled = false;
                    isExit = false;
                    index = 0;
                }
                break;
            case Obstacles.Attack:
                AttackMove();
                break;
        }
    }

    /// <summary>
    /// 横の移動
    /// </summary>
    private void LateralMove(Obstacles obstacles)
    {
        if (obstacles == Obstacles.LateralMovementRight)
        {
            float newX = Mathf.PingPong(Time.time * lateraSpeed, lateraRange * 2) - lateraRange;
            transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
        }
        else
        {
            float newX = -Mathf.PingPong(Time.time * lateraSpeed, lateraRange * 2) - lateraRange;
            transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
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
                || transform.position.y <= player.transform.position.y + offset)
            {
                transform.position += moveDirection * attackSpeed * Time.deltaTime;
            }
        }

        if (transform.position.x < minX - offset)
        {
            time = interval;
            transform.position = startPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("aa");
        time = timer;
        isTach = true;
        isExit = false;
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親を移動床にする
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isExit = true;
        index++;
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親を移動床にする
            collision.transform.SetParent(null);
        }
    }
}
