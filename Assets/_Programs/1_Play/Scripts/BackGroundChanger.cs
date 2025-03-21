using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private FollowCamera followCamera;

    [System.Serializable]
    public class LayerImage
    {
        public int layer;          // 対応するtargetLayer
        public GameObject image;   // レイヤーで使用する背景
    }

    [SerializeField] private List<LayerImage> layerImageList;

    private int currentLayer = -1; //初期値を-1にして、未設定にする
    private GameObject currentImage = null; //表示中の背景

    private void Start()
    {
        if (followCamera == null)
        {
            followCamera = FindFirstObjectByType<FollowCamera>();
        }

        foreach (var entry in layerImageList)
        {
            //全背景を非表示
            if (entry.image != null)
                entry.image.SetActive(false);
        }
    }

    void Update()
    {
        int targetLayer = Mathf.RoundToInt(followCamera.GetTargetLayer());

        if (targetLayer != currentLayer)
        {
            //階層が変わったら表示背景を変更
            ChangeBackground(targetLayer);
            currentLayer = targetLayer;
        }
    }

    private void ChangeBackground(int layer)
    {
        if (currentImage != null)
        {
            // 以前の画像を非表示
            currentImage.SetActive(false);
        }

        LayerImage targetImage = layerImageList.Find(entry => entry.layer == layer); //対応する背景を検索

        if (targetImage != null && targetImage.image != null)
        {
            targetImage.image.SetActive(true);
            currentImage = targetImage.image;
        }
        else
        {
            currentImage = null;    //該当画像がなければ表示しない
        }
    }
}