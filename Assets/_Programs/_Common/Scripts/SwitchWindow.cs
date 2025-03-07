using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    [Header("切り替えるウィンドウとそのON/OFF")]
    [SerializeField] private GameObject window;
    [SerializeField] private bool isSwitch;

    [SerializeField] private Image imgFadePanel;

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

    /// <summary>
    /// ウィンドウの切り替え
    /// </summary>
    private void WindowSwitch()
    {
        if (!imgFadePanel.enabled) return;
        if (window.activeSelf == isSwitch) return;

        Time.timeScale = (isSwitch) ? 0 : 1;
        window.SetActive(isSwitch);
    }
}
