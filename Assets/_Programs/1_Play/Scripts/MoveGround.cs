using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 障害物の種類
/// </summary>
enum Obstacles
{
    normal,
    LateralMovement,
    VerticalMovement,
    rotate,
}


public class MoveGround : MonoBehaviour
{
    enum Obstacles
    {
        normal,
        LateralMovement,
        VerticalMovement,
        target,
        rotate,
    }

    [SerializeField, Header("障害物の種類")] private Obstacles obstacles1;
    [SerializeField, Header("障害物の種類")] private Obstacles obstacles2;

    [SerializeField, Header("移動の速さ")] private float moveSpeed;
    [SerializeField, Header("可動域　横(LateralMovement)")] private float lateraRange;
    [SerializeField, Header("可動域　縦(VerticalMovement)")] private float verticalRange;
    [SerializeField, Header("回転の速さ")] private float rotateSpeed;
    [SerializeField, Header("所定の座標")] private Transform[] targets;

    private int currentTargetIndex = 0; // 現在のターゲットインデックス

    Vector3 startPosition;

    ///// <summary>
    ///// タイマー
    ///// </summary>
    //float time;

    ///// <summary>
    ///// 実効値
    ///// </summary>
    //float index;

    // Start is called before the first frame update
    void Start()
    {
        //time = 0;
        //index = 0;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //time -= Time.deltaTime;
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
}
