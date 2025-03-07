using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetNextScene : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField, Header("éüÇ…ëJà⁄Ç∑ÇÈÉVÅ[Éì")]
    private GameManager.STATE_SCENE state_scene;

    [SerializeField] private AudioClip decide;
    [SerializeField] private AudioClip select;

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

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GetInstance.PlaySE(select);
    }
}
