using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionEnd : MonoBehaviour
{
    public static string missionEndState;
    public Text titleText;
    public Text descriptionText;
    public Text deltaVText;
    
    // Start is called before the first frame update
    void Start()
    {
        switch(missionEndState) 
        {
        case "TWR Too Small":
            titleText.text = "Mission Failed";
            descriptionText.text = "Your thrust-to-weight ratio was too low. Either reduce the mass of your rocket or increase the thrust.";
            break;
        case "Mission Success":
            titleText.text = "Mission Success";
            descriptionText.text = "You had just enough fuel and completed the mission.";
            break;
        case "Not Enough Fuel":
            titleText.text = "Mission Failed";
            descriptionText.text = "Your rocket doesn't have enough deltaV to complete its mission.";
            break;
        case "Too Much Fuel":
            titleText.text = "Mission Failed";
            descriptionText.text = "Your rocket has too much deltaV and you're wasting a lot of resources.";
            break;
        case "Not Enough Astronauts":
            titleText.text = "Mission Failed";
            descriptionText.text = "You didn't bring enough astronauts.";
            break;
        default:
            break;
        }
        deltaVText.text = "Required deltaV: " + Calculations.CalculateDeltaV() + " m/s\nYour rocket's deltaV: " + Calculations.deltaV + " m/s";
    }
}