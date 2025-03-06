using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBGMVolume : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // BGMの音量設定
        audioSource.volume = GameManager.GetInstance.GetBGMVolume;
    }
}
