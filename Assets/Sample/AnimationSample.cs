using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSample : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [Header("�A�j���[�V�����摜")]
    [SerializeField] private Sprite[] spritesSample;
    private float timerAnimation = 0f;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �^�C�}�[�̏�����
        timerAnimation = 0f;

        NullCheck();

        // �摜�̏�����
        spriteRenderer.sprite = spritesSample[0];
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
        if (spriteRenderer = null)
        {
            Debug.LogError("SpriteRenderer�R���|�[�l���g���擾�ł��܂���");
        }
        if (spritesSample.Length == 0)
        {
            Debug.LogError("�摜�Q�����ݒ�ł�");
        }
        if (INTERVAL_ANIMATION == 0f)
        {
            Debug.LogError("�A�j���[�V�����Ԋu�����ݒ�ł�");
        }
    }

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    /// <param name="_sprites">�A�j���[�V�����p�摜</param>
    private void Animation(Sprite[] _sprites)
    {
        if (timerAnimation < INTERVAL_ANIMATION)
        {
            // �C���^�[�o����
            timerAnimation += Time.deltaTime;
            return;
        }

        // �A�j���[�V����
        timerAnimation = 0;
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (spriteRenderer.sprite == _sprites[i])
            {
                if (i == _sprites.Length - 1)
                {
                    // �ŏ��̉摜�ɖ߂�
                    spriteRenderer.sprite = _sprites[0];
                    return;
                }
                else
                {
                    // ���̉摜��
                    spriteRenderer.sprite = _sprites[i + 1];
                    return;
                }
            }
            else if (i == _sprites.Length - 1)
            {
                // �摜��ύX����
                spriteRenderer.sprite = _sprites[0];
                return;
            }
        }
    }
}
