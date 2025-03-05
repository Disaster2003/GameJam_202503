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
        startY = player.position.y; //�X�^�[�g����Y���W���L�^
    }

    void Update()
    {
        if(player != null)
        {
            int altitude = Mathf.FloorToInt(player.position.y - startY); //�v���C���[��Y���W�𐮐��ɕϊ�
            altitudeText.text = "�W��:" + altitude + "m";
        }
    }
}
