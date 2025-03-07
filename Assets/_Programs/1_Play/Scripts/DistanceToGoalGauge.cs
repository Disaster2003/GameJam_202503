using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoalGauge : MonoBehaviour
{
    [SerializeField] private Slider distanceSlider;
    [SerializeField] private Image playerIcon;
    [SerializeField] private float extraMargin = 10f;
    private DistanceToGoal distanceToGoal;
    private float playerIconPosX;

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
        float sliderValue = Mathf.InverseLerp(maxDistance, 0, distance);
        distanceSlider.value = sliderValue;
    }

    void UpdatePlayerIcon()
    {
        float gaugeHeight = distanceSlider.GetComponent<RectTransform>().rect.height;
        float playerIconPosY = gaugeHeight * distanceSlider.value;

        playerIcon.rectTransform.anchoredPosition = new Vector2(playerIconPosX, playerIconPosY);
    }

    void SetPlayerIcon()
    {
        //ゲージの幅を取得
        float gaugeWidth = distanceSlider.GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log(gaugeWidth);
        Debug.Log(playerIcon.rectTransform.anchoredPosition.y);
        
        playerIconPosX = gaugeWidth + extraMargin;
        //プレイヤーアイコン（矢印）をゲージ右端に設置
        playerIcon.rectTransform.anchoredPosition = new Vector2(playerIconPosX, playerIcon.rectTransform.anchoredPosition.y);
    }
}
