using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoalGauge : MonoBehaviour
{
    [SerializeField, Header("�Q�[�W���g�p���邩")] private bool UseSlider = true;
    [SerializeField] private Slider distanceSlider;
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private float startPosX;
    [SerializeField] private float startPosY;
    [SerializeField] private float endPosY;
    private DistanceToGoal distanceToGoal;
    private float sliderValue;

    void Start()
    {
        distanceToGoal = FindFirstObjectByType<DistanceToGoal>();
        SetPlayerIcon();
    }

    void Update()
    {
        //�v���C���[�̈ʒu����S�[���܂ł̋������擾
        float distance = distanceToGoal.GetDistance();
        UpdateGauge(distance);
        UpdatePlayerIcon();
    }

    void UpdateGauge(float distance)
    {
        float maxDistance = distanceToGoal.GetMaxDistance();
        sliderValue = Mathf.InverseLerp(maxDistance, 0, distance);

        if(UseSlider)
        {
            distanceSlider.value = sliderValue;
        }
    }

    void UpdatePlayerIcon()
    {
        RectTransform gaugeRect = distanceSlider.GetComponent<RectTransform>();
        float gaugeHeight = gaugeRect.rect.height;

        float playerIconPosY = startPosY + (endPosY * (sliderValue*2));

        // anchoredPosition ��UI�̍��W��ύX
        playerIcon.anchoredPosition = new Vector2(startPosX, playerIconPosY);
    }

    void SetPlayerIcon()
    {
        //�Q�[�W�̕����擾
        if(UseSlider) 
        {
            RectTransform gaugeRect = distanceSlider.GetComponent<RectTransform>();
            startPosX = (gaugeRect.localPosition.x - (gaugeRect.rect.height / 2) + playerIcon.rect.width);
            startPosY = (gaugeRect.localPosition.y - (gaugeRect.rect.width / 2));
            endPosY = (startPosY + gaugeRect.rect.width);
        }

        playerIcon.anchoredPosition = new Vector2(startPosX, startPosY);
    }
}
