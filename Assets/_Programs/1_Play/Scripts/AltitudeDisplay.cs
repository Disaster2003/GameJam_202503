using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AltitudeDisplay : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private StartPos startPos;
    [Header("y���W��1�����邲�Ƃɉ�m�ɂ��邩")]
    [SerializeField] private int scale = 1;
    public TextMeshProUGUI altitudeText;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = startPos.GetStartPos(); //�X�^�[�g���̍��W���L�^
    }

    void Update()
    {
        if(player != null)
        {
            int altitude = Mathf.FloorToInt((player.position.y - startPosition.y)*scale); //�v���C���[��Y���W�𐮐��ɕϊ�
            altitudeText.text = "�W��:" + altitude + "m";
        }
    }

    public int GetScale()
    {
        return scale;
    }
}
