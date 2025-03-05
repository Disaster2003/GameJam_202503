using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
        // �E�Ɉړ�
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }

        //�n�ʂƐڐG���Ă��鎞
        if(ground.GronudCheck())
        {
            groundTime++;   //�n�ʂƂ̐ڐG���Ԃ̉��Z
            //�W�����v
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D rbody2D = GetComponent<Rigidbody2D>();  //rigidbody2d���擾
                if (groundTime > comboJumpTime) jumpCount = 0;  //�A���W�����v�ł͂Ȃ��ꍇ��0
                float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //�W�����v�͂̔{�����W�����v�̉񐔂ɂ���ĕς���
                rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //�W�����v

                jumpCount++;    //�W�����v�񐔂𑝂₷
                jumpCount %= 3; //3�Ŋ��������܂����邱�ƂłR��ȏ�̘A���W�����v���������Ȃ��悤�ɂ���
            }
        }
        else
        {
            groundTime = 0; //�n�ʂƗ��ꂽ�珉����
        }
    }


    private float speed = 0.003f; //�ړ����x
    private float jumpPower = 300f;    //�W�����v��
    private static float jumpMultiplier = 1.2f;    //�A���W�����v���̃W�����v�͂̔{��
    private int jumpCount = 0;  //�A���W�����v�̉�
    private int groundTime = 0;   //�n�ʂƐڐG���Ă��鎞��
    private static int comboJumpTime = 600;   //�A���W�����v�̗P�\����
    public GroundCheck ground;  //�n�ʂƂ̓����蔻��̃N���X��ݒ�
}
