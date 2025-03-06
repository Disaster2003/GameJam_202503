using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 startPosition;

    void Start()
    {
        //オブジェクトの位置をプレイヤーの開始地点に設定
        startPosition = transform.position;
        player.transform.position = startPosition;
    }

    public Vector3 GetStartPos()
    {
        return startPosition;
    }
}
