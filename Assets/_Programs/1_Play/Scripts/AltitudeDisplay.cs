using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AltitudeDisplay : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Header("y座標が1増えるごとに何mにするか")]
    [SerializeField] private int times = 1;
    public TextMeshProUGUI altitudeText;
    float startY;

    void Start()
    {
        startY = player.position.y; //スタート時のY座標を記録
    }

    void Update()
    {
        if(player != null)
        {
            int altitude = Mathf.FloorToInt(player.position.y - startY)*times; //プレイヤーのY座標を整数に変換
            altitudeText.text = "標高:" + altitude + "m";
        }
    }
}
