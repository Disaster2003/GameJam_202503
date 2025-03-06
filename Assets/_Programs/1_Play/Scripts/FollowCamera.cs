using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;    // 追従するターゲット（プレイヤー）
    [SerializeField] private float cameraSize = 5f; //カメラサイズ
    private float offsetY;  // 各階層の高さ
    private float targetLayer = 0f; //今いる階層

    private void Start()
    {
        offsetY = cameraSize * 2;
    }

    void Update()
    {
        Vector3 cameraPos = transform.position;
        float targetY = target.transform.position.y;

        // カメラの現在のY位置を基準に、ターゲットのY位置と比較して階層を決める
        float cameraY = cameraPos.y;

        // プレイヤーがカメラより上に移動したら階層を上げる
        if (targetY > cameraY+cameraSize)
        {
            targetLayer++;
            cameraPos.y = targetLayer * offsetY;
        }
        // プレイヤーがカメラより下に移動したら階層を下げる
        else if (targetY < cameraY-cameraSize)
        {
            targetLayer--;
            cameraPos.y = targetLayer * offsetY;
        }

        // X, Z座標は変更せず、固定
        cameraPos.x = 0;
        cameraPos.z = -10;

        // カメラ位置を更新
        transform.position = cameraPos;
    }

    public float GetTargetLayer()
    {
        return targetLayer;
    }
}
