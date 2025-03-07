using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetRanking : MonoBehaviour
{
    [SerializeField, Header("�f�o�b�O�p")] private float score;
    [SerializeField] TextMeshProUGUI txtPlayerScore;
    [SerializeField] TextMeshProUGUI[] txtRanks = new TextMeshProUGUI[5];
    private float[] ranks = new float[6];
    int newRank;

    // Start is called before the first frame update
    void Start()
    {
        NullCheck();

        GetRanking();
        UpdateRanking();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            RemoveRanking();
        }
    }

    /// <summary>
    /// null�`�F�b�N
    /// </summary>
    private void NullCheck()
    {
        if(txtPlayerScore == null)
        {
            Debug.LogError("�v���C���[�̃X�R�A�e�L�X�g�����ݒ�ł�");
        }
        for (int i = 0; i < txtRanks.Length; i++)
        {
            if (txtRanks[i] == null)
            {
                Debug.LogError("�����L���O�̃e�L�X�g�Q�����ݒ�ł�");
            }
        }
    }

    /// <summary>
    /// �f�[�^�̈���������A�ǂݍ��݂���
    /// </summary>
    private void GetRanking()
    {
        if (PlayerPrefs.HasKey("Rank1"))
        {
            // �f�[�^�̈�̓ǂݍ���
            for (int idx = 1; idx <= 5; idx++)
            {
                ranks[idx] = PlayerPrefs.GetFloat("Rank" + idx);
            }
        }
        else
        {
            // �f�[�^�̈�̏�����
            for (int idx = 1; idx <= 5; idx++)
            {
                ranks[idx] = 0;
                PlayerPrefs.SetFloat("Rank" + idx, ranks[idx]);
            }
        }
    }

    /// <summary>
    /// �����L���O���X�V����
    /// </summary>
    private void UpdateRanking()
    {
        newRank = 0; // �܂�����̃X�R�A��0�ʂƉ��肷��
        for (int idx = 5; idx > 0; idx--)
        {
            if (ranks[idx] == 0f || ranks[idx] > score)
            {
                // �V���������N�Ƃ��Ĕ��肷��
                newRank = idx;
            }
        }

        // 0�ʂ̂܂܂łȂ烉���N�C���O
        if (newRank == 0) return;

        for (int idx = 5; idx > newRank; idx--)
        {
            // �J�艺������
            ranks[idx] = ranks[idx - 1];
        }
        ranks[newRank] = score; // �V�����N�ɓo�^
        for (int idx = 1; idx <= 5; idx++)
        {
            // �f�[�^�̈�ɕۑ�
            PlayerPrefs.SetFloat("Rank" + idx, ranks[idx]);
        }
    }

    /// <summary>
    /// �e�L�X�g��ݒ肷��
    /// </summary>
    private void SetText()
    {
        // �X�R�A�̏�����
        txtPlayerScore.text = FormatTime(score);

        for (int idx = 0; idx < 5; idx++)
        {
            if (ranks[idx + 1] == 0f)
            {
                txtRanks[idx].text = "__:__.__";
                continue;
            }

            txtRanks[idx].text = FormatTime(ranks[idx + 1]);

            // �����N�C��������A���̉ӏ������F�ύX
            if (idx + 1 == newRank) txtRanks[idx].color = Color.yellow;
        }
    }

    /// <summary>
    /// �����L���O���폜����
    /// </summary>
    private void RemoveRanking()
    {
        txtRanks[newRank - 1].color = Color.white;
        newRank = 0;

        // �f�[�^�̈�̏�����
        PlayerPrefs.DeleteAll();

        GetRanking();
        SetText();
    }

    private string FormatTime(float score)
    {
        int minutes = (int)(score / 60);
        float seconds = score % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }

    public void SetPlayerTime(float time)
    {
        score = time;
    }

}
