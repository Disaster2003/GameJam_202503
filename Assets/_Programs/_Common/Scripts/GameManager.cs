using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonSample<GameManager>
{
    public enum STATE_SCENE
    {
        TITLE,  // �^�C�g�����
        PLAY,   // �v���C���
        RESULT, // ���ʉ��
    }
    private STATE_SCENE state_scene;

    private AudioSource audioSource;
    private float volumeBGM = 0.5f;
    private float volumeSE = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // �V�[���J�ڂɂ��j��̖h�~
        DontDestroyOnLoad(gameObject);

        // ��Ԃ̏�����
        state_scene = (STATE_SCENE)SceneManager.GetActiveScene().buildIndex;

        // �R���|�[�l���g�̎擾
        audioSource = GetComponent<AudioSource>();

        NullCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// null�`�F�b�N
    /// </summary>
    private void NullCheck()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource�R���|�[�l���g���擾�ł��܂���");
        }
    }

    /// <summary>
    /// �V�[����ύX����
    /// </summary>
    public STATE_SCENE ChangeScene
    {
        set
        {
            state_scene = value;
            SceneManager.LoadSceneAsync((int)state_scene);
        }
    }

    /// <summary>
    /// ���ʉ���炷
    /// </summary>
    /// <param name="SE">�炷���ʉ�</param>
    public void PlaySE(AudioClip SE)
    {
        audioSource.volume = volumeSE;
        audioSource.PlayOneShot(SE);
    }
   
    /// <summary>
    /// BGM�̉��ʂ��擾����
    /// </summary>
    public float GetBGMVolume { get { return volumeBGM; } }
    /// <summary>
    /// BGM�̉��ʂ�ݒ肷��
    /// </summary>
    public float SetBGMVolume { set { volumeBGM = value; } }

    /// <summary>
    /// SE�̉��ʂ��擾����
    /// </summary>
    public float GetSEVolume { get { return volumeSE; } }
    /// <summary>
    /// SE�̉��ʂ�ݒ肷��
    /// </summary>
    public float SetSEVolume { set { volumeSE = value; } }
}
