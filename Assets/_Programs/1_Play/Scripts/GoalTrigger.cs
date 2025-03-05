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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
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
}
