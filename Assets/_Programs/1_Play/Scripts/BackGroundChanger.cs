using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private FollowCamera followCamera;

    [System.Serializable]
    public class LayerImage
    {
        public int layer;          // �Ή�����targetLayer
        public GameObject image;   // ���C���[�Ŏg�p����w�i
    }

    [SerializeField] private List<LayerImage> layerImageList;

    private int currentLayer = -1; //�����l��-1�ɂ��āA���ݒ�ɂ���
    private GameObject currentImage = null; //�\�����̔w�i

    private void Start()
    {
        if (followCamera == null)
        {
            followCamera = FindFirstObjectByType<FollowCamera>();
        }

        foreach (var entry in layerImageList)
        {
            //�S�w�i���\��
            if (entry.image != null)
                entry.image.SetActive(false);
        }
    }

    void Update()
    {
        int targetLayer = Mathf.RoundToInt(followCamera.GetTargetLayer());

        if (targetLayer != currentLayer)
        {
            //�K�w���ς������\���w�i��ύX
            ChangeBackground(targetLayer);
            currentLayer = targetLayer;
        }
    }

    private void ChangeBackground(int layer)
    {
        if (currentImage != null)
        {
            // �ȑO�̉摜���\��
            currentImage.SetActive(false);
        }

        LayerImage targetImage = layerImageList.Find(entry => entry.layer == layer); //�Ή�����w�i������

        if (targetImage != null && targetImage.image != null)
        {
            targetImage.image.SetActive(true);
            currentImage = targetImage.image;
        }
        else
        {
            currentImage = null;    //�Y���摜���Ȃ���Ε\�����Ȃ�
        }
    }
}