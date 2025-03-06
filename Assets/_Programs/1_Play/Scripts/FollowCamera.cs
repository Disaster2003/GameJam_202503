using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;    // �Ǐ]����^�[�Q�b�g�i�v���C���[�j
    [SerializeField] private float cameraSize = 5f; //�J�����T�C�Y
    [Header("�J�����T�C�Y��2�{")]
    [SerializeField] private float offsetY = 10f;  // �e�K�w�̍���
    private float currentLayer = 0f;// ���݂̃J�����K�w
    private float targetLayer = 0f; //������K�w

    void Update()
    {
        Vector3 cameraPos = transform.position;
        float targetY = target.transform.position.y;

        // �J�����̌��݂�Y�ʒu����ɁA�^�[�Q�b�g��Y�ʒu�Ɣ�r���ĊK�w�����߂�
        float cameraY = cameraPos.y;

        // �v���C���[���J��������Ɉړ�������K�w���グ��
        if (targetY > cameraY+cameraSize)
        {
            targetLayer++;
            cameraPos.y = targetLayer * offsetY;
            currentLayer = targetLayer;
        }
        // �v���C���[���J������艺�Ɉړ�������K�w��������
        else if (targetY < cameraY-cameraSize)
        {
            targetLayer--;
            cameraPos.y = targetLayer * offsetY;
            currentLayer = targetLayer;
        }

        // X, Z���W�͕ύX�����A�Œ�
        cameraPos.x = 0;
        cameraPos.z = -10;

        // �J�����ʒu���X�V
        transform.position = cameraPos;
    }
}
