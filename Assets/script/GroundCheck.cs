using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�n�ʂƐڐG���Ă��邩�ǂ���
    public bool GronudCheck()
    {
        //������ɏ�Q���������Ă���Ȃ�true
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;   //�����Ă��Ȃ��Ȃ�false
        }

        //�S�Ă̔�������Z�b�g
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    //������ɏ�Q������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGroundEnter = true;
    }

    //������ɏ�Q���������Ă��鎞
    private void OnTriggerStay2D(Collider2D collision)
    {
      �@isGroundStay = true;
    }

    //������̏�Q�����o����
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGroundExit = true;
    }

    private bool isGround = false;  //�n�ʂɐG��Ă��邩�ǂ���
    private bool isGroundEnter = false, isGroundStay = false, isGroundExit = false; //�R���W�����̂��ꂼ��̔���
}
