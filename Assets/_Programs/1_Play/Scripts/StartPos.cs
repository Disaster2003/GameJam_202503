using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 startPosition;

    void Start()
    {
        //�I�u�W�F�N�g�̈ʒu���v���C���[�̊J�n�n�_�ɐݒ�
        startPosition = transform.position;
        player.transform.position = startPosition;
    }

    public Vector3 GetStartPos()
    {
        return startPosition;
    }
}
