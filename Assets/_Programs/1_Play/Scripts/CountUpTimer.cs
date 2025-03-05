using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
    private float timer;    //タイマー
    private bool isGameRunning; //ゲーム中かどうか

    void Start()
    {
        
    }

    void Update()
    {
        if(isGameRunning)
        {
            timer += Time.deltaTime;    //ゲーム中は時間をカウント
        }
    }

    void StartGame()
    {
        timer = 0f;
        isGameRunning = true;
    }

    public void EndGame()
    {
        isGameRunning = false;
    }

    public void RestartGame()
    {
        StartGame();
    }
}
