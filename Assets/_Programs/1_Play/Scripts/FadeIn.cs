using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image imgPanel;
    private float timerColor;
    [SerializeField, Header("�Ó]�Ԋu")]
    private float INTERVAL_FADEOUT;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        imgPanel = GetComponent<Image>();

        // �^�C�}�[�̏�����
        timerColor = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerColor >= INTERVAL_FADEOUT)
        {
            // ���g�̔j��
            Destroy(gameObject);
        }

        // �t�F�[�h�A�E�g
        timerColor += Time.deltaTime;
        imgPanel.color = Color.Lerp(Color.black, Color.clear, timerColor / INTERVAL_FADEOUT);
    }
}
