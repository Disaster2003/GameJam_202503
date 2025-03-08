using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceToGoal : MonoBehaviour
{
    [SerializeField] private GoalTrigger goalTrigger;
    [SerializeField] private Transform player;
    //[SerializeField] private TextMeshProUGUI GoalDistance;
    [SerializeField, Header("�S�[���܂ł̋�����������������")] private bool isHeightOnly = true;
    private Vector3 goalPos;
    private AltitudeDisplay altitudeDisplay;
    private float maxDistance;

    void Start()
    {
        goalPos = goalTrigger.GetGoalPos();
        altitudeDisplay = FindFirstObjectByType<AltitudeDisplay>();
        maxDistance = CalculationMaxDistance();
    }

    void Update()
    {
        CalculationDistance();
        //GoalDistance.text = "�S�[���܂�:" + CalculationDistance() + "m";
    }

    int CalculationDistance()
    {
        //�v���C���[�̈ʒu����S�[���܂ł̋����i��Βl�j
        if (isHeightOnly)
        {
            //���������iY���W�j
            return Mathf.Abs(Mathf.FloorToInt((goalPos.y - player.position.y) * altitudeDisplay.GetScale()));
        }
        else
        {
            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.y),
            new Vector2(goalPos.x, goalPos.y));

            return Mathf.Abs(Mathf.FloorToInt(distance * altitudeDisplay.GetScale()));
        }

    }

    float CalculationMaxDistance()
    {
        if(isHeightOnly)
        {
            return Mathf.Abs(Mathf.FloorToInt((goalPos.y - player.position.y) * altitudeDisplay.GetScale()));
        }
        else
        {
            return Vector2.Distance(new Vector2(player.position.x, player.position.y),
            new Vector2(goalPos.x, goalPos.y));
        }
    }

    public int GetDistance()
    {
        return CalculationDistance();
    }

    public float GetMaxDistance()
    {
        return maxDistance;
    }
}
