using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionText : MonoBehaviour
{
    public Text RandomMissionText;
    public Text CustomMissionText;
    public Dropdown astronaut;
    public Dropdown planet;
    public Dropdown circumstance;
    public Dropdown home;
    public static bool missionFound;
    string comeHome;
    public static MissionManager.Mission mission;

    void Start()
    {
        LoadMission(MissionInfo.isRandom);
    }

    public void LoadMission(bool r)
    {
        mission = MissionInfo.missionInfo;
        if (r)
        {
            RandomMissionText.text = MissionInfo.missionText;
            RandomMissionText.gameObject.SetActive(true);
            CustomMissionText.gameObject.SetActive(false);
        } else {
            RandomMissionText.gameObject.SetActive(false);
            CustomMissionText.gameObject.SetActive(true);
            astronaut.value = MissionInfo.astroDropValue;
            planet.value = MissionInfo.planetDropValue;
            circumstance.value = MissionInfo.circumDropValue;
            home.value = MissionInfo.homeDropValue;
        }
    }

    public void AstroDropChanged()
    {
        mission.NumAstronauts = astronaut.value + 1;
        MissionInfo.astroDropValue = astronaut.value;
        MissionInfo.missionInfo = mission;
    }

    public void PlanetDropChanged()
    {
        mission.DestinationPlanet = MissionManager.solarSystem[planet.value];
        MissionInfo.planetDropValue = planet.value;
        MissionInfo.missionInfo = mission;

        circumstance.ClearOptions();
        List<string> options;
        if (mission.DestinationPlanet.Name.Equals("Jupiter") || mission.DestinationPlanet.Name.Equals("Saturn") || mission.DestinationPlanet.Name.Equals("Uranus") || mission.DestinationPlanet.Name.Equals("Neptune"))
        {
            options = new List<string> {"Flyby", "Orbit"};
        } else
        {
            options = new List<string> {"Flyby", "Orbit", "Land"};
        }
        circumstance.AddOptions(options);
    }

    public void CircumDropChanged()
    {
        mission.DestinationCircumstance = circumstance.value;
        MissionInfo.circumDropValue = circumstance.value;
        MissionInfo.missionInfo = mission;
    }

    public void HomeDropChanged()
    {
        if (home.options[home.value].text.Equals("Yes"))
        {
            mission.ReturnHome = true;
        } else if (home.options[home.value].text.Equals("No")) {
            mission.ReturnHome = false;
        }
        MissionInfo.homeDropValue = home.value;
        MissionInfo.missionInfo = mission;
    }
}