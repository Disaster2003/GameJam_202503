using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour, IPointerEnterHandler
{
    [Header("切り替えるウィンドウとそのON/OFF")]
    [SerializeField] private GameObject window;
    [SerializeField] private bool isSwitch;

    [SerializeField] private Image imgFadePanel;

    [SerializeField] private AudioClip decide;
    [SerializeField] private AudioClip select;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            WindowSwitch();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) WindowSwitch();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GetInstance.PlaySE(select);
    }

    /// <summary>
    /// ウィンドウの切り替え
    /// </summary>
    private void WindowSwitch()
    {
        if (imgFadePanel.enabled) return;
        if (window.activeSelf == isSwitch) return;

        GameManager.GetInstance.PlaySE(decide);
        Time.timeScale = (isSwitch) ? 0 : 1;
        window.SetActive(isSwitch);
    }
}
