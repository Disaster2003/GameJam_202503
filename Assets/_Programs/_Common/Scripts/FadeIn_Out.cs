using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn_Out : MonoBehaviour
{
    private Image imgFadePanel;
    private bool isFadeOut;

    private float timerColor;
    [SerializeField, Header("�Ó]�Ԋu")]
    private float INTERVAL_FADE;

    [SerializeField, Header("���ɑJ�ڂ���V�[��")]
    private GameManager.STATE_SCENE state_scene;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        imgFadePanel = GetComponent<Image>();

        // ��Ԃ̏�����
        isFadeOut = false;

        // �^�C�}�[�̏�����
        timerColor = 0f;

        NullCheck();

        // �F�̏�����
        imgFadePanel.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            if (timerColor >= INTERVAL_FADE)
            {
                // ���̃V�[����
                GameManager.GetInstance.ChangeScene = state_scene;
                return;
            }

            // �t�F�[�h�A�E�g
            imgFadePanel.color = Color.Lerp(Color.clear, Color.black, timerColor / INTERVAL_FADE);
        }
        else
        {
            if (timerColor >= INTERVAL_FADE)
            {
                // �摜�̔�\��
                imgFadePanel.enabled = false;
                return;
            }

            // �t�F�[�h�C��
            imgFadePanel.color = Color.Lerp(Color.black, Color.clear, timerColor / INTERVAL_FADE);
        }

        // ���Ԍv��
        timerColor += Time.deltaTime;
    }

    /// <summary>
    /// null�`�F�b�N
    /// </summary>
    private void NullCheck()
    {
        if (imgFadePanel == null)
        {
            Debug.Log("Image�R���|�[�l���g���擾�ł��܂���");
        }
    }

    /// <summary>
    /// �t�F�[�h�A�E�g���J�n����
    /// </summary>
    public void StartFadeOut()
    {
        isFadeOut = true;
        imgFadePanel.enabled = true;
    }
}
