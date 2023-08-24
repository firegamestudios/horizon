using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Races: 0 male human, 1 female human, 2 droid, 3 alien
/// Classes: 0 fighter, 1 biologist, 2 hunter, 3 engineer, 4 hacker
/// </summary>

public class CreationManager : MonoBehaviour
{
    string playerName;
    string race;
    string classe;

    public static List<int> attributes = new();

    static int pointsRemaining;

    float health;
    float energy;
    float resistance;

    //skills
    float meleeDamage = 1;
    float rangedDamage = 1;
    float hacking = 2;
    float healing = 1;
    float leadership = 1;
    float genengineering = 1;
    float piloting = 1;
    float tracking = 1;
    float taming = 1;
    float unlock = 1;

    public TMP_Text raceText;
    public TMP_Text classeText;
    public List<TMP_Text> atts;
    public List<TMP_Text> sheetAtts;
    public TMP_Text prText;
    public List<TMP_Text> statsTexts;
    public TMP_Text skillsText;

    GameObject startButton;

    PlayerData playerData;
    SaveLoadManager saveLoadManager;

    private void Awake()
    {
        startButton = GameObject.Find("Canvas").transform.Find("Start Button").gameObject;
        playerData = new PlayerData();
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
    }

    private void Start()
    {
        saveLoadManager.DeletePlayerData();

        race = "Male Human";
        classe = "Fighter";
       
        //add 5 attributes
        for (int i = 0; i < 5; i++)
        {
            attributes.Add(0);
        }
        SetupRace("Male Human");
        SetupClasse("Fighter");
        ClearRP();
    }

    #region Randomizers
    public void RaceRandomizer()
    {
        int dice = Random.Range(0, 4);
        switch (dice) {
            case 0:
                SetupRace("Male Human");
                break;
            case 1:
                SetupRace("Female Human");
                break;
            case 2:
                SetupRace("Droid");
                break;
            case 3:
                SetupRace("Alien");
                break;

        }
    }
    public void ClasseRandomizer()
    {
        int dice = Random.Range(0, 5);
        switch (dice)
        {
            case 0:
                SetupClasse("Fighter");
                break;
            case 1:
                SetupClasse("Biologist");
                break;
            case 2:
                SetupClasse("Engineer");
                break;
            case 3:
                SetupClasse("Hunter");
                break;
            case 4:
                SetupClasse("Hacker");
                break;
        }
    }
    public void RandomizeAttributes()
    {
        ClearRP();

        while(pointsRemaining > 0)
        {
            int dice = Random.Range(0, 5);
           AttributeIncrease(dice);
            
        }
    }
    #endregion

    #region Update methods
    private void Update()
    {
        if(pointsRemaining == 0 && !startButton.activeInHierarchy)
        {
            startButton.SetActive(true);
        }
    }
    public void UpdateAttributes()
    {
        for (int i = 0; i < atts.Count; i++)
        {
            atts[i].text = attributes[i].ToString();
            sheetAtts[i].text = attributes[i].ToString();
        }
        prText.text = "Points remaining: "+pointsRemaining.ToString();

        //Stats logic
        health = (attributes[2] * 10);
        energy = (attributes[0] + attributes[2]);
        resistance = (attributes[0] * attributes[1]);
        statsTexts[0].text = health.ToString();
        statsTexts[1].text = energy.ToString();
        statsTexts[2].text = resistance.ToString();

        UpdateSkills();
    }

    void UpdateSkills()
    {
        string raceBased = "";
        switch (race) {
            case "Male Human":
                raceBased = "Based on race: \n No race based skills \n\n";
                break;
            case "Female Human":
                raceBased = "Based on race: \n Healing +1 \n Leadership +1 \n\n";
                break;
            case "Droid":
                raceBased = "Based on race: \n No race based skills \n\n";
                break;
            case "Alien":
                raceBased = "Based on race: \n Genetics Engineering +1 \n Piloting +1 \n\n";
                break;
            default:
                raceBased = "No skills based on race";
                break;
        }
        string classeBased = "";
        switch (classe)
        {
            case "Fighter":
                classeBased = "Based on class: \n +1d4 melee damage \n +5 health per level \n\n";
                break;
            case "Hunter":
                classeBased = "Based on class: \n +1d4 ranged weapon damage \n Tracking +1 \n\n";
                break;
            case "Biologist":
                classeBased = "Based on class: \n genetics engineering +1 \n Taming +1 \n\n";
                break;
            case "Engineer":
                classeBased = "Based on class: \n Crafting +1 \n Piloting +1 \n\n";
                break;
            case "Hacker":
                classeBased = "Based on class: \n Never fails hacking \n Unlock +1 \n\n";
                break;
            default:
                classeBased = "No class based on race";
                break;
        }

        skillsText.text = raceBased + classeBased;
    }

    void ClearRP()
    {
        pointsRemaining = 15;
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i] = 3;
        }
        UpdateAttributes();
    }

    #endregion

    #region Setup Methods

    public void SetupRace(string _race)
    {
        race = _race;
       
        raceText.text = race;

        UpdateAttributes();
    }

    public void SetupClasse(string _classe)
    {
        classe = _classe;

        classeText.text = "level 1 " + classe;

        UpdateAttributes();
    }

    public void AttributeIncrease(int att)
    {
        if(pointsRemaining <= 0) { return; }

        pointsRemaining--;
        attributes[att]++;

        UpdateAttributes();
    }

    public void AttributeDecrease(int att)
    {
        if(pointsRemaining > 14) { return; }
        if (attributes[att] == 3) { return; }

        pointsRemaining++;
        attributes[att]--;

        UpdateAttributes();
    }
    #endregion

    #region OnStart Game
    public void OnStartButton()
    {
        playerData.playerName = playerName;
        playerData.race = race;
        playerData.classe = classe;

        //Attributes
        playerData.attributes = new int[5];

        for (int i = 0; i < 5; i++)
        {
            playerData.attributes[i] = attributes[i];
        }

        playerData.Health = health;
        playerData.Energy = energy;
        playerData.Resistance = resistance;

        //Skills
        playerData.MeleeDamage = meleeDamage;
        playerData.RangedDamage = rangedDamage;
        playerData.Hacking = hacking;
        playerData.Healing = healing;
        playerData.Leadership = leadership;
        playerData.GenEngineering = genengineering;
        playerData.Piloting = piloting;
        playerData.Tracking = tracking;
        playerData.Taming = taming;
        playerData.Unlock = unlock;

        saveLoadManager.SavePlayerData();

        SceneManager.LoadScene("Junk Processing Plant");
    }
    #endregion
}
