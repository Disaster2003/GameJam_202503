using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    [Header("切り替えるウィンドウとそのON/OFF")]
    [SerializeField] private GameObject window;
    [SerializeField] private bool isSwitch;

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
<<<<<<< HEAD
=======
        if (window.activeSelf == isSwitch) return;

>>>>>>> 36e198edbf218ea7783cfa60e50f7efe739efb66
        Time.timeScale = (isSwitch) ? 0 : 1;
        window.SetActive(isSwitch);
    }
}
