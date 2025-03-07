using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] private string nextSceneName;  //実行時に使用するシーン名（非表示）

#if UNITY_EDITOR
    [Header("遷移先シーン選択")]
    [SerializeField] private UnityEditor.SceneAsset nextSceneNameAsset; //エディタ用
#endif

    private float finalTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            CountUpTimer timerScript = GameObject.FindFirstObjectByType<CountUpTimer>();
            if(timerScript != null)
            {
                timerScript.EndGame();
                finalTime = timerScript.GetTimer();
            }
            
            SceneManager.sceneLoaded += ResultSceneLoaded;  //イベントに登録

            SceneManager.LoadScene(nextSceneName);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(nextSceneNameAsset != null) 
        {
            nextSceneName = nextSceneNameAsset.name;
        }
    }
#endif

    private void ResultSceneLoaded(Scene next, LoadSceneMode mode)
    {
        SetRanking setRanking = GameObject.FindFirstObjectByType<SetRanking>();

        setRanking.SetScore = finalTime; //タイムを渡す処理

        SceneManager.sceneLoaded -= ResultSceneLoaded;  //イベント解除
    }

    public Vector3 GetGoalPos()
    {
        return transform.position;
    }
}
