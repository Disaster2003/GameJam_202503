using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetRanking : MonoBehaviour
{
    [SerializeField, Header("デバッグ用")] private float score;
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
    /// nullチェック
    /// </summary>
    private void NullCheck()
    {
        if(txtPlayerScore == null)
        {
            Debug.LogError("プレイヤーのスコアテキストが未設定です");
        }
        for (int i = 0; i < txtRanks.Length; i++)
        {
            if (txtRanks[i] == null)
            {
                Debug.LogError("ランキングのテキスト群が未設定です");
            }
        }
    }

    /// <summary>
    /// データ領域を初期化、読み込みする
    /// </summary>
    private void GetRanking()
    {
        if (PlayerPrefs.HasKey("Rank1"))
        {
            // データ領域の読み込み
            for (int idx = 1; idx <= 5; idx++)
            {
                ranks[idx] = PlayerPrefs.GetFloat("Rank" + idx);
            }
        }
        else
        {
            // データ領域の初期化
            for (int idx = 1; idx <= 5; idx++)
            {
                ranks[idx] = 0;
                PlayerPrefs.SetFloat("Rank" + idx, ranks[idx]);
            }
        }
    }

    /// <summary>
    /// ランキングを更新する
    /// </summary>
    private void UpdateRanking()
    {
        newRank = 0; // まず今回のスコアを0位と仮定する
        for (int idx = 5; idx > 0; idx--)
        {
            if (ranks[idx] == 0f || ranks[idx] > score)
            {
                // 新しいランクとして判定する
                newRank = idx;
            }
        }

        // 0位のままでならランクイン外
        if (newRank == 0) return;

        for (int idx = 5; idx > newRank; idx--)
        {
            // 繰り下げ処理
            ranks[idx] = ranks[idx - 1];
        }
        ranks[newRank] = score; // 新ランクに登録
        for (int idx = 1; idx <= 5; idx++)
        {
            // データ領域に保存
            PlayerPrefs.SetFloat("Rank" + idx, ranks[idx]);
        }
    }

    /// <summary>
    /// テキストを設定する
    /// </summary>
    private void SetText()
    {
        // スコアの初期化
        txtPlayerScore.text = FormatTime(score);

        for (int idx = 0; idx < 5; idx++)
        {
            if (ranks[idx + 1] == 0f)
            {
                txtRanks[idx].text = "__:__.__";
                continue;
            }

            txtRanks[idx].text = FormatTime(ranks[idx + 1]);

            // ランクインしたら、その箇所だけ色変更
            if (idx + 1 == newRank) txtRanks[idx].color = Color.yellow;
        }
    }

    /// <summary>
    /// ランキングを削除する
    /// </summary>
    private void RemoveRanking()
    {
        txtRanks[newRank - 1].color = Color.white;
        newRank = 0;

        // データ領域の初期化
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
