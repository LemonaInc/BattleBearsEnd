using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour {

    public int team;
    public float captureValue;

    private float captureRate = 0.02f;
    private SkinnedMeshRenderer flag;

    void Start ()
    {
        flag = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update ()
    {
        Color teamColor = GameManager.instance.teamColors[team];
        //We use Lerp to show a color between white and the teamColor, based on the captureValue
        flag.material.color = Color.Lerp(Color.white, teamColor, captureValue);
    }

    void OnTriggerStay (Collider otherCollider)
    {
        Unit unit = otherCollider.GetComponent<Unit>();
        if (unit.team == team)
        {
            captureValue += captureRate;
            //Ensuring that the flag can never be captured for more than 1, 
            //otherwise if 1 teddy waits at an outpost for 10 min, it would take 10 min to decapture
            if (captureValue >= 1)
            {
                captureValue = 1;
            }
        } else
        {
            captureValue -= captureRate;
            //If captureValue reaches 0, we switch teams
            if (captureValue <= 0)
            {
                team = unit.team;
                captureValue = 0;
            }
        }
    }
}
