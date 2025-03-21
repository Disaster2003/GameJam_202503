using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetNextScene : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField, Header("次に遷移するシーン")]
    private GameManager.STATE_SCENE state_scene;

    [SerializeField] private AudioClip decide;
    [SerializeField] private AudioClip select;
    private GameObject goButton;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameManager.GetInstance.PlaySE(decide);
            GameManager.GetInstance.ChangeScene = state_scene;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (goButton != EventSystem.current.currentSelectedGameObject)
        {
            goButton = EventSystem.current.currentSelectedGameObject;

            // 選択音の再生
            if (goButton == gameObject) GameManager.GetInstance.PlaySE(select);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GetInstance.PlaySE(select);
    }
}
