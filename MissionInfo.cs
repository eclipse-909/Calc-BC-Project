using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo : MonoBehaviour
{
    public static MissionManager.Mission missionInfo;
    public static string missionText;
    static string comeHome;
    public static bool isRandom;
    public static int astroDropValue = 0;
    public static int planetDropValue = 0;
    public static int circumDropValue = 0;
    public static int homeDropValue = 0;

    public static void RandomMission()
    {
        isRandom = true;
        MissionText.mission = new MissionManager.Mission();
        missionInfo = MissionText.mission;
        if (MissionText.mission.ReturnHome)
        {
            comeHome = " and bring them back to Earth safely";
        } else {
            comeHome = "";
        }
        string astro = " astronauts";
        if (MissionText.mission.NumAstronauts == 1)
        {
            astro = " astronaut";
        }
        missionText = "Bring " + MissionText.mission.NumAstronauts + astro + " to " + MissionText.mission.DestinationPlanet.PlanetCircumstances[Random.Range(0, MissionText.mission.DestinationPlanet.PlanetCircumstances.Length)] + " " + MissionText.mission.DestinationPlanet.Name + comeHome + ".\nBuild a rocket and figure out how much fuel you need to bring. Round UP the fuel to the nearest third decimal place.\nPlanets with an atmosphere allow you to aerobrake, meaning you can ignore the deltaV cost when landing. Atmospheric planets include Earth, Venus, Mars, and Titan.";
    }

    public static void CustomMission()
    {
        isRandom = false;
        MissionText.mission = new MissionManager.Mission(1, MissionManager.solarSystem[0], 0, true);
        missionInfo = MissionText.mission;
    }
}