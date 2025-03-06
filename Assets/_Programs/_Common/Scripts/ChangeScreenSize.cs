using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChangeScreenSize : MonoBehaviour
{
    private TextMeshProUGUI txtScreenSize;
    [Header("�𑜓x")]
    [SerializeField] private int[] screenWidth;
    [SerializeField] private int[] screenHeight;
    private enum STATE_SCREEN
    {
        FULL, // �t���X�N���[��
        FHD,  // 1920 * 1080
        HDTV, // 1280 * 720
    }
    private static STATE_SCREEN state_screen;
    private const int indexMaxSTATE_SCREEN = (int)STATE_SCREEN.HDTV;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        txtScreenSize = GetComponent<TextMeshProUGUI>();

        NullCheck();

        // �t���X�N���[����
        Screen.fullScreen = true;

        // �e�L�X�g�̏�����
        txtScreenSize.text = "�t���X�N���[��";

        // ��Ԃ̏�����
        state_screen = STATE_SCREEN.FULL;
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
        if (txtScreenSize == null)
        {
            Debug.LogError("TextMeshProUGUI�R���|�[�l���g���擾�ł��܂���");
        }
        if (screenWidth.Length == 0 || screenHeight.Length == 0
            || screenWidth.Length != screenHeight.Length)
        {
            Debug.LogError("�𑜓x�����ݒ�ł�");
        }
    }

    /// <summary>
    /// �X�N���[�����g�傷��
    /// </summary>
    public void OnClickLeft()
    {
        if (state_screen == 0)
        {
            // �ŏ��T�C�Y��
            state_screen = (STATE_SCREEN)indexMaxSTATE_SCREEN;
        }
        else state_screen--;
        ChangeTextAndScreen();
    }

    /// <summary>
    /// �X�N���[�����k������
    /// </summary>
    public void OnClickRight()
    {
        if (state_screen == (STATE_SCREEN)indexMaxSTATE_SCREEN)
        {
            // �t���X�N���[����
            state_screen = 0;
        }
        else state_screen++;
        ChangeTextAndScreen();
    }

    /// <summary>
    /// �e�L�X�g�Ɖ𑜓x��ύX����
    /// </summary>
    private void ChangeTextAndScreen()
    {
        if (screenWidth[(int)state_screen] == 0)
        {
            // �t���X�N���[��
            Screen.fullScreen = true;
            txtScreenSize.text = "�t���X�N���[��";
        }
        else
        {
            // �t���X�N���[���ȊO
            Screen.fullScreen = false;
            txtScreenSize.text = $"{screenWidth[(int)state_screen]} * {screenHeight[(int)state_screen]}";
            Screen.SetResolution(screenWidth[(int)state_screen], screenHeight[(int)state_screen], Screen.fullScreen);
        }
    }
}
