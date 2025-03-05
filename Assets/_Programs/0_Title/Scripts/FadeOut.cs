using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image imgPanel;
    private float timerColor;
    [SerializeField, Header("�Ó]�Ԋu")]
    private float INTERVAL_FADEOUT;

    private bool isFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        imgPanel = GetComponent<Image>();

        // �^�C�}�[�̏�����
        timerColor = 0f;

        // ��Ԃ̏�����
        isFadeOut = false;

        NullCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            if (timerColor >= INTERVAL_FADEOUT)
            {
                // �v���C��ʂ�
                GameManager.GetInstance.ChangeScene = GameManager.STATE_SCENE.PLAY;
            }

            // �t�F�[�h�A�E�g
            timerColor += Time.deltaTime;
            imgPanel.color = Color.Lerp(Color.clear, Color.black, timerColor / INTERVAL_FADEOUT);
            return;
        }

        // �t�F�[�h�A�E�g�J�n
        if (Input.GetKeyDown(KeyCode.Space)) isFadeOut = true;
    }

    private void NullCheck()
    {
        if(imgPanel == null)
        {
            Debug.LogError("Image�R���|�[�l���g���擾�ł��܂���");
        }
    }
}
