using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoalGauge : MonoBehaviour
{
    [SerializeField] private Slider distanceSlider;
    [SerializeField] private RectTransform playerIcon;
    private DistanceToGoal distanceToGoal;
    private float startPosX;
    private float startPosY;
    private float endPosY;
    private float sliderValue;

    void Start()
    {
        distanceToGoal = FindFirstObjectByType<DistanceToGoal>();
        SetPlayerIcon();
    }

    void Update()
    {
        //プレイヤーの位置からゴールまでの距離を取得
        float distance = distanceToGoal.GetDistance();
        UpdateGauge(distance);
        UpdatePlayerIcon();
    }

    void UpdateGauge(float distance)
    {
        float maxDistance = distanceToGoal.GetMaxDistance();
        sliderValue = Mathf.InverseLerp(maxDistance, 0, distance);
        distanceSlider.value = sliderValue;
    }

    void UpdatePlayerIcon()
    {
        RectTransform gaugeRect = distanceSlider.GetComponent<RectTransform>();
        float gaugeHeight = gaugeRect.rect.height;

        float playerIconPosY = startPosY + (endPosY * (sliderValue*2));

        // anchoredPosition でUIの座標を変更
        playerIcon.anchoredPosition = new Vector2(startPosX, playerIconPosY);
    }

    void SetPlayerIcon()
    {
        //ゲージの幅を取得
        RectTransform gaugeRect = distanceSlider.GetComponent<RectTransform>();
        startPosX = (gaugeRect.localPosition.x - (gaugeRect.rect.height / 2) + playerIcon.rect.width);
        startPosY = (gaugeRect.localPosition.y - (gaugeRect.rect.width/2));
        endPosY = (startPosY + gaugeRect.rect.width);

        playerIcon.anchoredPosition = new Vector2(startPosX, startPosY);
    }
}
