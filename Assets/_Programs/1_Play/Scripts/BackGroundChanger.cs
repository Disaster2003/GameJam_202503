using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private FollowCamera followCamera;

    [System.Serializable]
    public class LayerImage
    {
        public int layer;          // ‘Î‰‚·‚étargetLayer
        public GameObject image;   // ƒŒƒCƒ„[‚Åg—p‚·‚é”wŒi
    }

    [SerializeField] private List<LayerImage> layerImageList;

    private int currentLayer = -1; //‰Šú’l‚ğ-1‚É‚µ‚ÄA–¢İ’è‚É‚·‚é
    private GameObject currentImage = null; //•\¦’†‚Ì”wŒi

    private void Start()
    {
        if (followCamera == null)
        {
            followCamera = FindFirstObjectByType<FollowCamera>();
        }

        foreach (var entry in layerImageList)
        {
            //‘S”wŒi‚ğ”ñ•\¦
            if (entry.image != null)
                entry.image.SetActive(false);
        }
    }

    void Update()
    {
        int targetLayer = Mathf.RoundToInt(followCamera.GetTargetLayer());

        if (targetLayer != currentLayer)
        {
            //ŠK‘w‚ª•Ï‚í‚Á‚½‚ç•\¦”wŒi‚ğ•ÏX
            ChangeBackground(targetLayer);
            currentLayer = targetLayer;
        }
    }

    private void ChangeBackground(int layer)
    {
        if (currentImage != null)
        {
            // ˆÈ‘O‚Ì‰æ‘œ‚ğ”ñ•\¦
            currentImage.SetActive(false);
        }

        LayerImage targetImage = layerImageList.Find(entry => entry.layer == layer); //‘Î‰‚·‚é”wŒi‚ğŒŸõ

        if (targetImage != null && targetImage.image != null)
        {
            targetImage.image.SetActive(true);
            currentImage = targetImage.image;
        }
        else
        {
            currentImage = null;    //ŠY“–‰æ‘œ‚ª‚È‚¯‚ê‚Î•\¦‚µ‚È‚¢
        }
    }
}