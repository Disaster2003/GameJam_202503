using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class MoveGround : MonoBehaviour
{
    /// <summary>
    /// 足場のタイプ
    /// </summary>
    enum Obstacles
    {
        normal,
        LateralMovement,
        VerticalMovement,
        target,
        rotate,
        disappear
    }

    [SerializeField, Header("キャラクターのポジション")] private GameObject character;
    [SerializeField, Header("カメラの中央からの縦幅半分")] private float widthY;
    [SerializeField, Header("階層")] private float floor;

    [SerializeField, Header("障害物の種類")] private Obstacles obstacles1;
    [SerializeField, Header("障害物の種類")] private Obstacles obstacles2;

    [SerializeField, Header("移動の速さ")] private float moveSpeed;
    [SerializeField, Header("可動域　横(LateralMovement)")] private float lateraRange;
    [SerializeField, Header("可動域　縦(VerticalMovement)")] private float verticalRange;
    [SerializeField, Header("回転の速さ")] private float rotateSpeed;
    [SerializeField, Header("足場の消える時間")] private float timer;
    [SerializeField, Header("所定の座標")] private Transform[] targets;

    private int currentTargetIndex = 0; // 現在のターゲットインデックス

    Vector3 startPosition;

    /// <summary>
    /// タイマー
    /// </summary>
    float time;


    /// <summary>
    /// 消えるフラグ
    /// </summary>
    private bool isTach;

    ///// <summary>
    ///// 実効値
    ///// </summary>
    //float index;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        isTach = false;
        //index = 0;
        startPosition = transform.position;
        Debug.Log(2 * ((floor - 1) * widthY));
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        SetObstacles();      
    }

    
    void SetObstacles()
    {
        switch (obstacles1)
        {
            case Obstacles.LateralMovement:
                LateralMove();
                break;
            case Obstacles.VerticalMovement:
                VerticalMove();
                break;
            case Obstacles.target:
                TargetMove();
                break;
            case Obstacles.rotate:
                RotateMove();
                break;
            case Obstacles.disappear:
                if (isTach)
                {
                    if (time < 0)
                    {
                        gameObject.SetActive(false);
                        isTach = false;
                    }
                }
                if (character.transform.position.x > widthY + (2 * (floor * widthY))
                    || character.transform.position.x < (2 * ((floor - 1) * widthY)) + widthY)
                {
                    gameObject.SetActive(true);
                }
                break;
        }

        switch (obstacles2)
        {
            case Obstacles.LateralMovement:
                LateralMove();
                break;
            case Obstacles.VerticalMovement:
                VerticalMove();
                break;
            case Obstacles.target:
                TargetMove();
                break;
            case Obstacles.rotate:
                RotateMove();
                break;
            case Obstacles.disappear:
                if (isTach)
                {
                    if (time < 0)
                    {
                        gameObject.SetActive(false);
                        isTach =false;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 横の移動
    /// </summary>
    void LateralMove()
    {
        float newX = Mathf.PingPong(Time.time * moveSpeed, lateraRange * 2) - lateraRange;
        transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 縦の移動
    /// </summary>
    void VerticalMove()
    {
        float newY = Mathf.PingPong(Time.time * moveSpeed, lateraRange * 2);
        transform.position = new Vector3(transform.position.x, newY + startPosition.y, transform.position.z);
    }

    /// <summary>
    /// 所定の位置
    /// </summary>
    void TargetMove()
    {
        if (targets.Length == 0) return;

        // 現在のターゲット位置に向かって移動
        Vector3 targetPosition = (currentTargetIndex < targets.Length) ? targets[currentTargetIndex].position : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

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

    void RotateMove()
    {
        transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("aa");
        time = timer;
        isTach = true;
    }
}
