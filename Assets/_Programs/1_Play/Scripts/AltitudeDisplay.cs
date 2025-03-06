using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AltitudeDisplay : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private StartPos startPos;
    [Header("y座標が1増えるごとに何mにするか")]
    [SerializeField] private int scale = 1;
    public TextMeshProUGUI altitudeText;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = startPos.GetStartPos(); //スタート時の座標を記録
    }

    void Update()
    {
        if(player != null)
        {
            int altitude = Mathf.FloorToInt((player.position.y - startPosition.y)*scale); //プレイヤーのY座標を整数に変換
            altitudeText.text = "標高:" + altitude + "m";
        }
    }

    public int GetScale()
    {
        return scale;
    }
}
