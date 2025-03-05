using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// ��Q���̎��
/// </summary>
enum Obstacles
{
    normal,
    LateralMovement,
    VerticalMovement,
    rotate,
}


public class MoveGround : MonoBehaviour
{
    enum Obstacles
    {
        normal,
        LateralMovement,
        VerticalMovement,
        target,
        rotate,
    }

    [SerializeField, Header("��Q���̎��")] private Obstacles obstacles1;
    [SerializeField, Header("��Q���̎��")] private Obstacles obstacles2;

    [SerializeField, Header("�ړ��̑���")] private float moveSpeed;
    [SerializeField, Header("����@��(LateralMovement)")] private float lateraRange;
    [SerializeField, Header("����@�c(VerticalMovement)")] private float verticalRange;
    [SerializeField, Header("��]�̑���")] private float rotateSpeed;
    [SerializeField, Header("����̍��W")] private Transform[] targets;

    private int currentTargetIndex = 0; // ���݂̃^�[�Q�b�g�C���f�b�N�X

    Vector3 startPosition;

    ///// <summary>
    ///// �^�C�}�[
    ///// </summary>
    //float time;

    ///// <summary>
    ///// �����l
    ///// </summary>
    //float index;

    // Start is called before the first frame update
    void Start()
    {
        //time = 0;
        //index = 0;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //time -= Time.deltaTime;
        SetObstacles();
    }

    
    void SetObstacles()
    {
        switch (obstacles1)
        {
            case Obstacles.LateralMovement:
                LateralMove();
                break;
            case Obstacles.VerticalMovement:
                VerticalMove();
                break;
            case Obstacles.target:
                TargetMove();
                break;
            case Obstacles.rotate:
                RotateMove();
                break;
        }

        switch (obstacles2)
        {
            case Obstacles.LateralMovement:
                LateralMove();
                break;
            case Obstacles.VerticalMovement:
                VerticalMove();
                break;
            case Obstacles.target:
                TargetMove();
                break;
            case Obstacles.rotate:
                RotateMove();
                break;
        }
    }

    /// <summary>
    /// ���̈ړ�
    /// </summary>
    void LateralMove()
    {
        float newX = Mathf.PingPong(Time.time * moveSpeed, lateraRange * 2) - lateraRange;
        transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// �c�̈ړ�
    /// </summary>
    void VerticalMove()
    {
        float newY = Mathf.PingPong(Time.time * moveSpeed, lateraRange * 2);
        transform.position = new Vector3(transform.position.x, newY + startPosition.y, transform.position.z);
    }

    /// <summary>
    /// ����̈ʒu
    /// </summary>
    void TargetMove()
    {
        if (targets.Length == 0) return;

        // ���݂̃^�[�Q�b�g�ʒu�Ɍ������Ĉړ�
        Vector3 targetPosition = (currentTargetIndex < targets.Length) ? targets[currentTargetIndex].position : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // �^�[�Q�b�g�܂��͏����ʒu�ɋ߂Â����玟�̃^�[�Q�b�g��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (currentTargetIndex < targets.Length)
            {
                currentTargetIndex++; // ���̃^�[�Q�b�g��
            }
            else
            {
                currentTargetIndex = 0; // ���ׂẴ^�[�Q�b�g��������珉���ʒu�ɖ߂�
            }
        }
    }

    void RotateMove()
    {
        transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
    }
}
