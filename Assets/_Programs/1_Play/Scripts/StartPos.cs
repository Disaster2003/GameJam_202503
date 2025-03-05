using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Start()
    {
        //オブジェクトの位置をプレイヤーの開始地点に設定
        player.transform.position = transform.position;
    }
}
