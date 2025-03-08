using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Awake()
    {
        //オブジェクトの位置をプレイヤーの開始地点に設定
        player.transform.position = transform.position;
    }

    public Vector3 GetStartPos()
    {
        return transform.position;
    }
}
