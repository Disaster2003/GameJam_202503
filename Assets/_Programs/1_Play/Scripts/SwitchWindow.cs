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
            Time.timeScale = (isSwitch) ? 0 : 1;
            window.SetActive(isSwitch);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
