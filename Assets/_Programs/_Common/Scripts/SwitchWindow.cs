using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    [Header("�؂�ւ���E�B���h�E�Ƃ���ON/OFF")]
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
    /// �E�B���h�E�̐؂�ւ�
    /// </summary>
    private void WindowSwitch()
    {
        if (!imgFadePanel.enabled) return;
        if (window.activeSelf == isSwitch) return;

        Time.timeScale = (isSwitch) ? 0 : 1;
        window.SetActive(isSwitch);
    }
}
