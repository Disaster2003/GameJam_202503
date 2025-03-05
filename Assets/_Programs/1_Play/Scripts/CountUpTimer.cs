using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float timer;    //タイマー
    private bool isGameRunning; //ゲーム中かどうか

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if(isGameRunning)
        {
            timer += Time.deltaTime;    //ゲーム中は時間をカウント
            UpdateTimerDisplay();   //タイマーの表示を更新
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

    void UpdateTimerDisplay()
    {
        //分と秒を計算
        int minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = timer % 60f;

        //秒数を2桁表示にする
        timerText.text = string.Format("{0:D2}:{1:00.00}", minutes, seconds);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = timer % 60f;

        return string.Format("{0:D2}:{1:00.00}", minutes, seconds);
    }
}
