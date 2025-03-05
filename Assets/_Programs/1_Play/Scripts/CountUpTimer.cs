using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float timer;    //�^�C�}�[
    private bool isGameRunning; //�Q�[�������ǂ���

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if(isGameRunning)
        {
            timer += Time.deltaTime;    //�Q�[�����͎��Ԃ��J�E���g
            UpdateTimerDisplay();   //�^�C�}�[�̕\�����X�V
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
        //���ƕb���v�Z
        int minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = timer % 60f;

        //�b����2���\���ɂ���
        timerText.text = string.Format("{0:D2}:{1:00.00}", minutes, seconds);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = timer % 60f;

        return string.Format("{0:D2}:{1:00.00}", minutes, seconds);
    }
}
