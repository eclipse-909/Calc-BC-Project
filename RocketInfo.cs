using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketInfo : MonoBehaviour
{
    public static List<RocketPart> parts = new List<RocketPart>();
    static int currIndex;
    public static CommandPod commandPod;
    public static FuelTank fuelTank;
    public static Engine engine;
    public static List<GameObject> partsGO = new List<GameObject>();
    public GameObject cp;
    public GameObject ft;
    public GameObject en;
    public static float[] scales = {1, 1.25f, 1.5f};
    public InputField tankHeight;
    public InputField tankFuel;
    public static List<GameObject> panels = new List<GameObject>();
    public GameObject cpPanel;
    public GameObject ftPanel;
    public GameObject enPanel;
    public static List<Text> rocketTexts = new List<Text>();
    public Text cpPannelText;
    public Text ftPannelText;
    public Text enPannelText1;
    public Text enPannelText2;
    public Text rocketInfoText;
    public GameObject missionInfoPanel;
    public Text missionInfoPanelText;
    public GameObject launchPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        commandPod = new CommandPod(0, 1);
        fuelTank = new FuelTank(0, 384, 1);
        engine = new Engine(0);
        parts.Add(commandPod);
        parts.Add(fuelTank);
        parts.Add(engine);
        partsGO.Add(cp);
        partsGO.Add(ft);
        partsGO.Add(en);
        panels.Add(cpPanel);
        panels.Add(ftPanel);
        panels.Add(enPanel);
        rocketTexts.Add(cpPannelText);
        rocketTexts.Add(ftPannelText);
        rocketTexts.Add(enPannelText1);
        rocketTexts.Add(enPannelText2);
        rocketTexts.Add(rocketInfoText);
        tankHeight.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
        tankFuel.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
        tankHeight.text = "80";
        missionInfoPanelText.text = "Astronauts: " + MissionInfo.missionInfo.NumAstronauts + "\nDestination Planet: " + MissionInfo.missionInfo.DestinationPlanet.Name + "\nDestination Circumstance: " + MissionInfo.missionInfo.DestinationPlanet.PlanetCircumstances[MissionInfo.missionInfo.DestinationCircumstance] + "\nReturn Home: " + MissionInfo.missionInfo.ReturnHome + "\n\nYou need to have a thrust-to-weight ratio greater than 1. Gravity on Earth is 9.81 m/s^2 for reference\n\nPlanets with an atmosphere allow you to aerobrake, meaning you can ignore the deltaV cost when landing.\nAtmospheric planets include:\nEarth\nVenus\nMars\nTitan";
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
            RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "CommandPod")
                {
                    currIndex = 0;
                } else if (hit.collider.gameObject.name == "FuelTank")
                {
                    currIndex = 1;
                } else if (hit.collider.gameObject.name == "Engine")
                {
                    currIndex = 2;
                } 
            } else {
                currIndex = -1;
            }
            ShowPanel();
        } else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "CommandPod")
                {
                    currIndex = 0;
                } else if (hit.collider.gameObject.name == "FuelTank")
                {
                    currIndex = 1;
                } else if (hit.collider.gameObject.name == "Engine")
                {
                    currIndex = 2;
                }
            } else {
                currIndex = -1;
            }
            ShowPanel();
        }
    }

    public void ToggleMissionInfo()
    {
        missionInfoPanel.SetActive(!missionInfoPanel.activeSelf);
    }

    public void ShowPanel()
    {
        cpPanel.SetActive(false);
        ftPanel.SetActive(false);
        enPanel.SetActive(false);
        if (currIndex != -1)
        {
            panels[currIndex].SetActive(true);
        }
    }

    public void ToggleLaunchPanel()
    {
        launchPanel.SetActive(!launchPanel.activeSelf);
    }
    
    public void ScaleDown()
    {
        if (parts[currIndex].ScaleIndex > 0)
        {
            parts[currIndex].ScaleIndex -= 1;
            parts[currIndex].UpdateScale(parts[currIndex].ScaleIndex);
        }
    }

    public void ScaleUp()
    {
        if (parts[currIndex].ScaleIndex < scales.Length - 1)
        {
            parts[currIndex].ScaleIndex += 1;
            parts[currIndex].UpdateScale(parts[currIndex].ScaleIndex);
        }
    }
    
    public void SetHeight()
    {
        float.TryParse(tankHeight.text, out float h);
        if (h < 0)
        {
            h = 0;
        }
        fuelTank.Height = 4.8f * h;
        fuelTank.UpdateScale(fuelTank.ScaleIndex);
    }

    public void SetFuel()
    {
        float.TryParse(tankFuel.text, out float f);
        if (f > fuelTank.FuelCap)
        {
            fuelTank.Fuel = fuelTank.FuelCap;
            tankFuel.text = "" + fuelTank.Fuel;
        } else if (f < 0)
        {
            fuelTank.Fuel = 0;
            tankFuel.text = "0";
        } else {
            fuelTank.Fuel = f;
        }
        fuelTank.UpdateScale(fuelTank.ScaleIndex);
    }
    
    public class RocketPart
    {
        public float DryMass {get; set;}
        public int ScaleIndex {get; set;}

        public RocketPart(int i)
        {
            ScaleIndex = i;
        }

        public virtual void UpdateScale(int s)
        {
            float midpoint = ((partsGO[0].transform.position.y + 210) + (partsGO[2].transform.position.y - 46.5f)) / 2;
	        partsGO[1].transform.position = new Vector3(540, partsGO[1].transform.position.y - (midpoint - 960), 0);
            partsGO[0].transform.position = new Vector3(540, partsGO[1].transform.position.y + (fuelTank.Height / 2) + (210 * scales[commandPod.ScaleIndex]), 0);
            partsGO[2].transform.position = new Vector3(540, partsGO[1].transform.position.y - (fuelTank.Height / 2) - (46.5f * scales[engine.ScaleIndex]), 0);
            rocketTexts[0].text = "Dry Mass: " + commandPod.DryMass + " kg\nCrew Capacity: " + commandPod.CrewCap;
            rocketTexts[1].text = "Dry Mass: " + fuelTank.DryMass + " kg\nFuel Capacity: " + fuelTank.FuelCap + " kL"; // some of these are not displaying, see if there's a way to only display 3 decimals and change the text boxes around
            rocketTexts[2].text = "Dry Mass: " + engine.DryMass + " kg\nThrust: " + engine.Thrust + " kN";
            rocketTexts[3].text = "\nFuel Consumption: " + engine.FuelConsum;
            rocketTexts[4].text = "Dry Mass: " + (commandPod.DryMass + fuelTank.DryMass + engine.DryMass) + " kg\nFuel Mass: 2 kg/kL\nThrust: " + engine.Thrust + " kN\nFuel-\nConsumption: " + engine.FuelConsum + " kL/s";
        }
    }

    public class CommandPod : RocketPart
    {
        // scale = 400, width = 180, height = 420
        public int CrewCap {get; set;}

        public CommandPod(int i, int c) : base(i)
        {
            CrewCap = c;
            DryMass = 500 * scales[ScaleIndex];
        }

        public override void UpdateScale(int s)
        {
            CrewCap = s + 1;
            DryMass = 500 * scales[s];
            partsGO[0].transform.localScale = new Vector3(400 * scales[s], 400 * scales[s], 1);
            base.UpdateScale(s);
        }
    }

    public class FuelTank : RocketPart
    {
        // scale = 80, width = 180, height = 384
        public float Height {get; set;}
        public float Width {get; set;}
        public float FuelCap {get; set;}
        public float Fuel {get; set;}
        public float FueledMass {get; set;}

        public FuelTank(int i, float h, float f) : base(i)
        {
            float Height = 4.8f * h;
            float Width = 180 * scales[ScaleIndex];
            FuelCap = (Mathf.PI * Mathf.Pow(Width / 60, 2) * Height) / 50;
            DryMass = ((2 * Mathf.PI * (Width / 60) * Height) + (2 * Mathf.PI * Mathf.Pow(Width / 60, 2))) / 1000;
            Fuel = f;
            FueledMass = Fuel * 2;
        }

        public override void UpdateScale(int s)
        {
            Width = 180 * scales[s];
            FuelCap = (Mathf.PI * Mathf.Pow(Width / 60, 2) * Height) / 50;
            DryMass = (2 * Mathf.PI * (Width / 60) * Height + 2 * Mathf.PI * Mathf.Pow(Width / 60, 2)) / 1000;
            FueledMass = Fuel * 2;
            partsGO[1].transform.localScale = new Vector3(80 * scales[s], (Height / 4.8f), 1);
            base.UpdateScale(s);
        }
    }

    public class Engine : RocketPart
    {
        // scale = 100, width = 94, height = 93
        public float Thrust {get; set;}
        public float FuelConsum {get; set;}

        public Engine(int i) : base(i)
        {
            Thrust = 20 * Mathf.Pow(scales[ScaleIndex], 2);
            FuelConsum = .125f * Mathf.Pow(scales[ScaleIndex], 3);
            DryMass = 750 * scales[ScaleIndex];
        }

        public override void UpdateScale(int s)
        {
            Thrust = 20 * Mathf.Pow(scales[s], 2);
            FuelConsum = .125f * Mathf.Pow(scales[s], 3);
            DryMass = 750 * scales[s];
            partsGO[2].transform.localScale = new Vector3(100 * scales[s], 100 * scales[s], 1);
            base.UpdateScale(s);
        }
    }
}