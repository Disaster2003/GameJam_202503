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

    // Start is called before the first frame update
    void Start()
    {
        // スコアの初期化
        score = score;
        txtPlayerScore.text = score.ToString("f2");

        NullCheck();

        GetRanking();
        UpdateRanking();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {

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
        int newRank = 0; // まず今回のスコアを0位と仮定する
        for (int idx = 5; idx > 0; idx--)
        {
            if (ranks[idx] > score || ranks[idx] == 0f)
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
        for(int idx = 0; idx < 5; idx++)
        {
            if (ranks[idx] == 0f)
            {
                txtRanks[idx].text = "___.__";
                continue;
            }

            txtRanks[idx].text = ranks[idx + 1].ToString("f2"); 
        }
    }
}
