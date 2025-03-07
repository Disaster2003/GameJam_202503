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
            
            SceneManager.sceneLoaded += ResultSceneLoaded;  //�C�x���g�ɓo�^

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

        setRanking.SetScore = finalTime; //�^�C����n������

        SceneManager.sceneLoaded -= ResultSceneLoaded;  //�C�x���g����
    }

    public Vector3 GetGoalPos()
    {
        return transform.position;
    }
}
