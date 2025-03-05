using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] private string nextSceneName;  //���s���Ɏg�p����V�[�����i��\���j

#if UNITY_EDITOR
    [Header("�J�ڐ�V�[���I��")]
    [SerializeField] private UnityEditor.SceneAsset nextSceneNameAsset; //�G�f�B�^�p
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
