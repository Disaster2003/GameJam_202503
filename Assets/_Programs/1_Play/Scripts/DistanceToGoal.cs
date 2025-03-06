using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceToGoal : MonoBehaviour
{
    [SerializeField] private GoalTrigger goalTrigger;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI GoalDistance;
    [SerializeField, Header("�S�[���܂ł̋�����������������")] private bool isHeightOnly = true;
    private Vector3 goalPos;
    private AltitudeDisplay altitudeDisplay;

    void Start()
    {
        goalPos = goalTrigger.GetGoalPos();
        altitudeDisplay = FindFirstObjectByType<AltitudeDisplay>();
    }

    void Update()
    {
        //�v���C���[�̈ʒu����S�[���܂ł̋����i��Βl�j
        if(isHeightOnly)
        {
            //���������iY���W�j
            int distance = Mathf.Abs(Mathf.FloorToInt((goalPos.y - player.position.y) * altitudeDisplay.GetScale()));
            GoalDistance.text = "�S�[���܂�:" + distance + "m";
        }
        else
        {
            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.y),
            new Vector2(goalPos.x, goalPos.y));

            int DisplayDistance = Mathf.Abs(Mathf.FloorToInt(distance * altitudeDisplay.GetScale()));
            GoalDistance.text = "�S�[���܂�:" + DisplayDistance + "m";
        }
    }
}
