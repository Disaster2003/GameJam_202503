using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V���O���g��
/// 
/// �W�F�l���b�N�Ŕėp������
/// where T : new()�̓C���X�^���X���̕ۏ�
/// </summary>
public abstract class SingletonSample<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    /// <summary>
    /// �C���X�^���X���擾����
    /// </summary>
    public static T GetInstance { get { return instance; } }

    public virtual void Awake()
    {
        if (instance == null)
        {
            // ����̂݃C���X�^���X��
            instance = (T)FindAnyObjectByType(typeof(T));
        }
        else
        {
            // �����֎~
            Destroy(gameObject);
        }
    }
}