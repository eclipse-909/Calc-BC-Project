using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    static string[] canLand = {"flyby", "oribit around", "land on"};
    static string[] cannotLand = {"flyby", "oribit around"};
    public static List<Planet> solarSystem = new List<Planet>
    {
        // Earth = 9400
        // {Flyby, Orbit, Land}
        // Atmosphere = aerobrake on land
        new Planet("Mercury", canLand, false, new int[] {11860, 1220, 3060}),
        new Planet("Venus", canLand, true, new int[] {3850, 2940, 27000}),
        new Planet("Moon", canLand, false, new int[] {3260, 680, 1730}),
        new Planet("Mars", canLand, true, new int[] {4270, 1440, 3800}),
        new Planet("Phobos", canLand, false, new int[] {5550, 3, 8}),
        new Planet("Deimos", canLand, false, new int[] {5260, 2, 4}),
        new Planet("Jupiter", cannotLand, true, new int[] {6570, 17200}),
        new Planet("Io", canLand, false, new int[] {16890, 730, 1850}),
        new Planet("Europa", canLand, false, new int[] {15460, 580, 1480}),
        new Planet("Ganymede", canLand, false, new int[] {13270, 790, 1970}),
        new Planet("Callisto", canLand, false, new int[] {11710, 70, 1760}),
        new Planet("Saturn", cannotLand, true, new int[] {7710, 10230}),
        new Planet("Titan", canLand, true, new int[] {10770, 660, 7600}),
        new Planet("Uranus", cannotLand, true, new int[] {8490, 6120}),
        new Planet("Neptune", cannotLand, true, new int[] {8600, 6750})
    };

    public class Planet
    {
        public string Name {get; set;}
        public string[] PlanetCircumstances {get; set;}
        public bool Atmosphere {get; set;}
        public int[] DeltaVatCircumstance {get; set;}

        public Planet(string n, string[] c, bool a, int[] d)
        {
            Name = n;
            PlanetCircumstances = c;
            Atmosphere = a;
            DeltaVatCircumstance = d;
        }
    }

    public class Mission
    {
        public int NumAstronauts {get; set;}
        public Planet DestinationPlanet {get; set;}
        public int DestinationCircumstance {get; set;}
        public bool ReturnHome {get; set;}

        public Mission()
        {
            NumAstronauts = Random.Range(1, 4);
            DestinationPlanet = solarSystem[Random.Range(0, solarSystem.Count)];
            DestinationCircumstance = Random.Range(0, DestinationPlanet.PlanetCircumstances.Length);
            ReturnHome = Random.Range(0, 2) == 0;
        }

        public Mission(int a, Planet p, int c, bool r)
        {
            NumAstronauts = a;
            DestinationPlanet = p;
            DestinationCircumstance = c;
            ReturnHome = r;
        }
    }
}