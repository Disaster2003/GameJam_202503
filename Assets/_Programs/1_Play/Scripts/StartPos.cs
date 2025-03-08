using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Awake()
    {
        //�I�u�W�F�N�g�̈ʒu���v���C���[�̊J�n�n�_�ɐݒ�
        player.transform.position = transform.position;
    }

    public Vector3 GetStartPos()
    {
        return transform.position;
    }
}
