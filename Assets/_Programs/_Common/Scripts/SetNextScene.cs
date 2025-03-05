using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNextScene : MonoBehaviour
{
    [SerializeField] GameManager.STATE_SCENE state_scene;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameManager.GetInstance.ChangeScene = state_scene;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
