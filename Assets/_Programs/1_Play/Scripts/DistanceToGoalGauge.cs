using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoalGauge : MonoBehaviour
{
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
        //プレイヤーの位置からゴールまでの距離を取得
        float distance = distanceToGoal.GetDistance();
        UpdatePlayerIcon(distance);
    }


    void UpdatePlayerIcon(float distance)
    {
        float maxDistance = distanceToGoal.GetMaxDistance();
        sliderValue = Mathf.InverseLerp(maxDistance, 0, distance);

        float playerIconPosY = startPosY + (endPosY * (sliderValue*2));

        // anchoredPosition でUIの座標を変更
        playerIcon.anchoredPosition = new Vector2(startPosX, playerIconPosY);
    }

    void SetPlayerIcon()
    {
        playerIcon.anchoredPosition = new Vector2(startPosX, startPosY);
    }
}
