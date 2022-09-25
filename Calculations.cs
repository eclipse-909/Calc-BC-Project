using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculations : MonoBehaviour
{
    public static float deltaV;
    
    // on click Launch button
    public void RunCalculations()
    {
        float tf = RocketInfo.fuelTank.Fuel / RocketInfo.engine.FuelConsum;
        float a0 = (RocketInfo.engine.Thrust * 1000) / ((RocketInfo.commandPod.DryMass + RocketInfo.fuelTank.DryMass + RocketInfo.engine.DryMass) + (RocketInfo.fuelTank.FueledMass));
        float af = (RocketInfo.engine.Thrust * 1000) / (RocketInfo.commandPod.DryMass + RocketInfo.fuelTank.DryMass + RocketInfo.engine.DryMass);
        deltaV = CalculateDeltaV();
        float j = (af - a0) / tf;
        Polynomial function = new Polynomial(new List<float> {a0, j}); // a0 + jX
        Polynomial integral = new Polynomial(new List<float> {a0, j}).indefIntegral();
        float rocketDeltaV = new Polynomial(new List<float> {a0, j}).indefIntegral().defIntegral(0, tf);
        if (MissionInfo.missionInfo.NumAstronauts > RocketInfo.commandPod.ScaleIndex + 1)
        {
            MissionEnd.missionEndState = "Not Enough Astronauts";
        } else  if (a0 <= 9.81f)
        {
            MissionEnd.missionEndState = "TWR Too Small";
        } else if (rocketDeltaV > (deltaV * 1.01f))
        {
            MissionEnd.missionEndState = "Too Much Fuel";
        } else if (rocketDeltaV > deltaV)
        {
            MissionEnd.missionEndState = "Mission Success";
        } else {
            MissionEnd.missionEndState = "Not Enough Fuel";
        }
    }

    // calculates required deltaV for the mission
    public static float CalculateDeltaV()
    {
        float deltaV = 9400;
        for (int i = 0; i < MissionInfo.missionInfo.DestinationCircumstance + 1; i++)
        {
            deltaV += MissionInfo.missionInfo.DestinationPlanet.DeltaVatCircumstance[i];
        }
        if (MissionInfo.missionInfo.DestinationPlanet.Atmosphere && MissionInfo.missionInfo.DestinationCircumstance == 2)
        {
            deltaV -= MissionInfo.missionInfo.DestinationPlanet.DeltaVatCircumstance[2];
        }
        if (MissionInfo.missionInfo.ReturnHome)
        {
            deltaV *= 2;
            if (MissionInfo.missionInfo.DestinationPlanet.Atmosphere && MissionInfo.missionInfo.DestinationCircumstance == 2)
            {
                deltaV += MissionInfo.missionInfo.DestinationPlanet.DeltaVatCircumstance[2];
            }
            deltaV -= 9400;
        }
        return deltaV;
    }

    public class Polynomial
    {
        public List<float> Coefficients {get; set;}

        public Polynomial(List<float> c)
        {
            // a[0]*X^0 + a[1]*X^1 + a[2]*X^2 + ... + a[n]*X^n
            Coefficients = c;
        }

        public Polynomial indefIntegral()
        {
            Polynomial integral = this;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                // divide the coefficient by the new power (power rule)
                integral.Coefficients[i] = Coefficients[i] / (i + 1);
            }
            // increase the power by 1 for all terms
            integral.Coefficients.Insert(0, 0);
            // constant of integration is assumed to be 0 for coding purposes
            // you also subtract the constant from itself when using definite integrals so it doesn't matter anyway
            return integral;
        }
        
        // indefIntegral() must be called first
        // Polynomial.indefIntegral().defIntegral(float lowerBound, float upperBound)
        public float defIntegral(float lowerBound, float upperBound)
        {
            // plug in the upper and lower bounds
            float sumUpper = 0;
            float sumLower = 0;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                sumUpper += Coefficients[i] * Mathf.Pow(upperBound, i);
                sumLower += Coefficients[i] * Mathf.Pow(lowerBound, i);
            }
            // return the definite integral
            return sumUpper - sumLower;
        }

        // displays the function in generic notation for debugging purposes
        // a[0]*X^0 + a[1]*X^1 + a[2]*X^2 + ... + a[n]*X^n
        public override string ToString()
        {
            string r = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                if (Coefficients[i] != 0)
                {
                    r += Coefficients[i];
                    if (i != 0)
                    {
                        r += "X^" + i;
                    }
                    r += " + ";
                }
            }
            return r.Substring(0, r.Length - 3);
        }
    }
}