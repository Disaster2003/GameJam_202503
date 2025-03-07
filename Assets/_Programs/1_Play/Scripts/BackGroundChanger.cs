using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private FollowCamera followCamera; 
    [SerializeField] private List<GameObject> images;   

    private int currentLayer = 0;

    private void Start()
    {
        if(followCamera == null)
        {
            followCamera = FindFirstObjectByType<FollowCamera>();
        }
    }

    void Update()
    {
        int targetLayer = Mathf.RoundToInt(followCamera.GetTargetLayer());

        if (targetLayer != currentLayer)
        {
            ChangeBackground(targetLayer);
            currentLayer = targetLayer;
        }
    }

    private void ChangeBackground(int layer)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].SetActive(i == layer); 
        }
    }
}
