using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBGMVolume : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // BGM�̉��ʐݒ�
        audioSource.volume = GameManager.GetInstance.GetBGMVolume;
    }
}
