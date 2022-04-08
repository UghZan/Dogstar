using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int seed = 13371337;
    public int money = 0;
    public bool debug;

    public string[] firstSyl;
    public string[] secondSyl;
    public string[] thirdSyl;

    public GameObject player;
    public GameObject dogStar;
    public GameObject universe;
    private DateTime begin = new DateTime(2434, 8, 3, 15, 17, 11);
    public Station[] stations;
    //private GameObject[] objects;
    public GameObject[] rocks;
    public GameObject[] darkMatter;
    public GameObject[] debris;
    public GameObject[] artifacts;

    public int questProgress;

    public bool[] artifacts_found;
    public string[] verses = { "1. The Darkness consuming\n\t\t\t\tStars above",
        "2. The cataclysm closing\n\t\t\t\ton our Sun",
        "3. The fate of our kin\n\t\t\t\tas dark as the Sky",
        "4. The Father decides to do\n\t\t\t\tthe forbidden ritual",
        "5. The Father speaks to the Dark Sky,\n\t\t\t\tin verses long dead",
        "6. The language of not living,\n\t\t\t\tand it answers him",
        "7. The Plague consumes our Star,\n\t\t\t\tbut it still remains",
        "8. The light is dim,\n\t\t\t\tbut it never perishes",
        "9. The eternal twilight,\n\t\t\t\tin which we dwell",
        "10. The miserable survival of our race,\n\t\t\t\tconsumed in",
        "11. The thoughts of the Dark,\n\t\t\t\tand the pale light of",
        "12. The Dog Star,\n\t\t\t\tas so It calls Itself"
};
    internal int current_verse = -1;

    public Color[] art_colors;

    public GameObject artifact;
    public GameObject asteroidField;
    public GameObject pickup;
    public GameObject rad_field;

    public int artifactAmount = 10;
    public int artifactSpawnRadius = 50000;
    public int asteroidFields = 200;
    public int asteroidSpawnRadius = 100000;
    public int radFields = 200;
    public int radSpawnRadius = 500000;

    public double time = 0;
    public int currentStation = 0;

    public List<Quest> currentQuests = new List<Quest>();

    private int resetConfirm;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        UnityEngine.Random.InitState(seed);
        //objects = new GameObject[objectAmountPerStation * stations.Length];
        debris = Resources.LoadAll<GameObject>("Prefabs/debris/");
        darkMatter = Resources.LoadAll<GameObject>("Prefabs/dark_matter/");
        rocks = Resources.LoadAll<GameObject>("Prefabs/rocks/");
        SaveManager.LoadFile();
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <  stations.Length; i++)
        {
            stations[i].stationID = i;
        }
        artifacts_found = new bool[12];
        if (!debug)
        {
            GenerateAsteroidFields();
            GenerateArtifacts();
            GenerateRadFields();
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveManager.SaveFile();
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (resetConfirm == 0)
            {
                ShipUI.instance.SendMessageToScreen("SYS", "ATTENTION: A FACTORY RESET WAS REQUESTED. PRESS P TWO MORE TIMES TO CONFIRM.");
                resetConfirm++;
            }
            else if (resetConfirm == 1)
            {
                ShipUI.instance.SendMessageToScreen("SYS", "AFTER RESETING YOUR SHIP TO FACTORY SETTINGS YOU WILL LOSE ALL YOUR PROGRESS DUE TO QUANTUM EFFECTS. PRESS P ONE MORE TIME TO CONFIRM.");
                resetConfirm++;
            }
            else if (resetConfirm == 2)
            {
                SaveManager.ResetFile();
                StartCoroutine(player.GetComponent<spaceship>().GameOver());
            }
        }
        if (!Inventory.instance.openInv)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    public string getCurrentTimeFormatted()
    {
            //game begins at 03.08.2234 15:17:11
        DateTime current = begin.AddSeconds(time);
        return current.ToString("ddMMyy_HH:mm:ss");

    }

    public string getCurrentTimeOnly()
    {
        //game begins at 03.08.2234 15:17:11
        DateTime current = begin.AddSeconds(time);
        return current.ToString("HH:mm:ss");

    }

    public void GenerateArtifacts()
    {
        for (int i = 0; i < artifactAmount; i++)
        {
            int pattern = UnityEngine.Random.Range(0, 4);
            Color color = art_colors[UnityEngine.Random.Range(0, art_colors.Length)];

            Vector3 pos = UnityEngine.Random.onUnitSphere * artifactSpawnRadius + new Vector3(0, 0, 100000);
            GameObject art = Instantiate(artifact, pos, Quaternion.identity, universe.transform);
            art.GetComponent<Artifact>().color = color;
            art.GetComponent<Artifact>().pattern = pattern;
        }
    }

    public void GenerateRadFields()
    {
        Collider[] cols;
        for (int i = 0; i < radFields; i++)
        {
            int size = UnityEngine.Random.Range(500, 10000);
            float level = UnityEngine.Random.Range(1, 20) * size / 2000;
            int success = 0;
            Vector3 pos = Vector3.zero;
            while (success < 5)
            {
                pos = UnityEngine.Random.insideUnitSphere * radSpawnRadius + new Vector3(0, 0, 100000);
                if ((cols = Physics.OverlapSphere(pos, size, Physics.DefaultRaycastLayers)).Length > 0)
                {
                    success++;
                    print(cols.Length + "_first one is: " + cols[0].name);
                }
                else
                {
                    break;
                }
            }
            if (success < 5)
            {
                GameObject art = Instantiate(rad_field, pos, Quaternion.identity, universe.transform);
                art.GetComponent<RadiationField>().radiationLevel = level;
                art.GetComponent<SphereCollider>().radius = size;
            }
        }
    }

    public void LearnVerse()
    {
        current_verse++;
        artifacts_found[current_verse] = true;
    }

    public void GenerateAsteroidFields()
    {
        Collider[] cols;
        for (int i = 0; i < asteroidFields; i++)
        {
            int size = UnityEngine.Random.Range(300, 1500);
            int asts = size / 15;
            int mainOre = MainOre();
            float mainOreChance = UnityEngine.Random.Range(0.25f, 0.55f);
            int sOre = SpecialOre();
            int success = 0;
            Vector3 pos = Vector3.zero;
            while (success < 5)
            {
                pos = UnityEngine.Random.insideUnitSphere * asteroidSpawnRadius + new Vector3(0,0,100000);
                if ((cols = Physics.OverlapSphere(pos, size, Physics.DefaultRaycastLayers)).Length > 0)
                {
                    success++;
                    print(cols.Length + "_first one is: " + cols[0].name);
                }
                else
                {
                    break;
                }
            }
            if (success < 5)
            {
                GameObject astField = Instantiate(asteroidField, pos, Quaternion.identity, universe.transform);
                AsteroidField af = astField.GetComponent<AsteroidField>();
                af.size = size;
                af.amountOfAsteroids = asts;
                af.mainOre = mainOre;
                af.mainChance = mainOreChance;
                af.rareOre = sOre;
            }
            else
                print("failed to place asteroid field");
        }
    }

    public int MainOre()
    {
        //carbon ferrite titanium
        //thorium umbrite rhodite

        int[] chances = { 3000, 2400, 1700, 1200, 700, 200};
        int sum = 0;
        for (int i = 0; i < chances.Length; i++)
        {
            sum += chances[i];
        }
        int num = UnityEngine.Random.Range(0, sum) + 1;
        for (int i = 0; i < chances.Length; i++)
        {
            num -= chances[i];
            if (num < 0)
                return i;
        }
        return 0;
    }

    public int SpecialOre()
    {
        //iridium petro voiden
        int[] chances = { 3000, 1000, 200 };
        int sum = 0;
        for (int i = 0; i < chances.Length; i++)
        {
            sum += chances[i];
        }
        int num = UnityEngine.Random.Range(0, sum) + 1;
        for (int i = 0; i < chances.Length; i++)
        {
            num -= chances[i];
            if (num < 0)
                return i;
        }
        return 0;
    }

    public bool ChangeFunds(int cash, string sender, string message)
    {
        if (cash < 0 && money + cash < 0)
        {
            ShipUI.instance.SendMessageToScreen(sender, string.Format("Error: Insufficient funds. Transaction cancelled."));
            return false;
        }
        money += cash;
        money = Mathf.Clamp(money, 0, int.MaxValue);
        if (cash < 0)
            ShipUI.instance.SendMessageToScreen(sender, string.Format("${0} has been withdrawn from your balance by {1}. Note: {2}.", Math.Abs(cash).ToString(), sender, message), true, false);
        else
            ShipUI.instance.SendMessageToScreen(sender, string.Format("${0} has been deposited on your balance by {1}. Note: {2}.", cash.ToString(), sender, message), true, false);
        return true;
    }

}
