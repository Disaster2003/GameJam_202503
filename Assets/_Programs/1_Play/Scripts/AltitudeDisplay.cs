using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AltitudeDisplay : MonoBehaviour
{
    [SerializeField] private Transform player;
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
            int altitude = Mathf.FloorToInt(player.position.y - startY); //プレイヤーのY座標を整数に変換
            altitudeText.text = "標高:" + altitude + "m";
        }
    }
}
