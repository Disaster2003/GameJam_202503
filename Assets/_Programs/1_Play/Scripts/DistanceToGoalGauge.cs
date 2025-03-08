using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoalGauge : MonoBehaviour
{
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private Image playerPosBar;
    [SerializeField, Header("アイコンの位置調整用")] private float offsetStartPosY;
    [SerializeField] private float offsetEndPosY;
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
        UpdatePlayerIcon(distance);
    }


    void UpdatePlayerIcon(float distance)
    {
        float maxDistance = distanceToGoal.GetMaxDistance();
        Debug.Log(maxDistance);
        sliderValue = Mathf.InverseLerp(maxDistance, 0, distance);

        float playerIconPosY = Mathf.Lerp(startPosY+offsetStartPosY, endPosY+offsetEndPosY, sliderValue);

        // anchoredPosition でUIの座標を変更
        playerIcon.anchoredPosition = new Vector2(startPosX, playerIconPosY);
    }

    void SetPlayerIcon()
    {
        RectTransform barRect = playerPosBar.GetComponent<RectTransform>();
        startPosX = (barRect.localPosition.x + (playerIcon.rect.width/2));
        startPosY = (barRect.localPosition.y - (barRect.rect.height / 2));
        endPosY = (startPosY + (barRect.rect.height));

        playerIcon.anchoredPosition = new Vector2(startPosX, startPosY);
    }
}
