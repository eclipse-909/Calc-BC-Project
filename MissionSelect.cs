using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{
    public static bool randomMission;
    
    public void RandomMission()
    {
        MissionInfo.isRandom = true;
        MissionInfo.RandomMission();
        MissionText.missionFound = false;
    }

    public void CustomMission()
    {
        MissionInfo.isRandom = false;
        MissionInfo.CustomMission();
        MissionText.missionFound = false;
    }
}