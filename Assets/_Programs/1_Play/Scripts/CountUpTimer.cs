using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
    private float timer;    //�^�C�}�[
    private bool isGameRunning; //�Q�[�������ǂ���

    void Start()
    {
        
    }

    void Update()
    {
        if(isGameRunning)
        {
            timer += Time.deltaTime;    //�Q�[�����͎��Ԃ��J�E���g
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
