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

    float healthBonus = 0;
    float meleeBonus = 0;
    float rangedBonus = 0;

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
    public List<TMP_Text> skillsTexts;

    GameObject startButton;

    PlayerData playerData;
    SaveLoadManager saveLoadManager;

    private void Awake()
    {
        startButton = GameObject.Find("Canvas").transform.Find("Start Button").gameObject;
        
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();

        playerData = saveLoadManager.playerData;
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
        RaceRandomizer();
        ClasseRandomizer();
        RandomizeAttributes();
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
        UpdateAttributes();
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
        UpdateAttributes();
    }
    public void RandomizeAttributes()
    {
        ClearRP();

        while(pointsRemaining > 0)
        {
            int dice = Random.Range(0, 5);
           AttributeIncrease(dice);
            
        }

        UpdateAttributes();
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
        UpdateSkills();

        for (int i = 0; i < atts.Count; i++)
        {
            atts[i].text = attributes[i].ToString();
            sheetAtts[i].text = attributes[i].ToString();
        }
        prText.text = "Points remaining: "+pointsRemaining.ToString();

        //Stats logic
        health = (attributes[2] * 10) + healthBonus;
        energy = (attributes[0] + attributes[2]);
        resistance = (attributes[0] * attributes[1]);
        statsTexts[0].text = health.ToString();
        statsTexts[1].text = energy.ToString();
        statsTexts[2].text = resistance.ToString();
        meleeDamage = 1 + meleeBonus;
        rangedDamage = 1 + rangedBonus;
        
    }

    void UpdateSkills()
    {
        //Reset colors
        for (int i = 0; i < skillsTexts.Count; i++)
        {
            skillsTexts[i].color = new Color(255, 255, 255);
        }
        //Reset bonus
        healthBonus = 0;
        meleeBonus = 0;
        rangedBonus = 0;
        //Reset attribute colors
        for (int i = 0; i < 5; i++)
        {
            sheetAtts[i].color = new Color(255, 255, 255);
        }

        string raceBased = "";
        switch (race) {
            case "Male Human":
                raceBased = "Based on race: \n No race based skills \n\n";
                break;
            case "Female Human":
                raceBased = "Based on race: \n Healing +1 \n Leadership +1 \n\n";
                healing++;
                skillsTexts[2].color = new Color(255, 0, 0);
                leadership++;
                skillsTexts[3].color = new Color(255, 0, 0);
                break;
            case "Droid":
                raceBased = "Based on race: \n No race based skills \n\n";
               
                break;
            case "Alien":
                raceBased = "Based on race: \n Genetic Engineering +1 \n Piloting +1 \n\n";
                genengineering++;
                skillsTexts[5].color = new Color(255, 0, 0);
                piloting++;
                skillsTexts[6].color = new Color(255, 0, 0);
                break;
            default:
                raceBased = "No skills based on race";
                break;
        }
        string classeBased = "";
        switch (classe)
        {
            case "Fighter":
                classeBased = "Based on class: \n +4 melee damage \n +5 health per level \n\n";
                healthBonus = 5;
                meleeBonus = 4;
                skillsTexts[0].color = new Color(255, 0, 0);
                break;
            case "Hunter":
                classeBased = "Based on class: \n +3 ranged weapon damage \n Tracking +1 \n\n";
                tracking++;
                skillsTexts[7].color = new Color(255, 0, 0);
                rangedBonus = 3;
                skillsTexts[1].color = new Color(255, 0, 0);
                break;
            case "Biologist":
                classeBased = "Based on class: \n Genetic Engineering +1 \n Taming +1 \n\n";
                genengineering++;
                skillsTexts[5].color = new Color(255, 0, 0);
                taming++;
                skillsTexts[8].color = new Color(255, 0, 0);
                break;
            case "Engineer":
                classeBased = "Based on class: \n Crafting +1 \n Piloting +1 \n\n";
                attributes[3]++;
                sheetAtts[3].color = new Color(255, 0, 0);
                piloting++;
                skillsTexts[6].color = new Color(255, 0, 0);
                break;
            case "Hacker":
                classeBased = "Based on class: \n Never fails hacking \n Unlock +1 \n\n";
                hacking++;
                skillsTexts[4].color = new Color(255, 0, 0);
                unlock++;
                skillsTexts[9].color = new Color(255, 0, 0);
                break;
            default:
                classeBased = "No class based on race";
                break;
        }

        skillsText.text = raceBased + classeBased;

        if(healthBonus > 0)
        {
            statsTexts[0].color = new Color(255, 0, 0);
        }
        else
        {
            statsTexts[0].color = new Color(255,255,255);
        }

        skillsTexts[0].text = "Melee Damage: " + meleeDamage.ToString();
        skillsTexts[1].text = "Ranged Damage: " + rangedDamage.ToString();
        skillsTexts[2].text = "Healing: " + healing.ToString();
        skillsTexts[3].text = "Leadership: " + leadership.ToString();
        skillsTexts[4].text = "Hacking: " + hacking.ToString();
        skillsTexts[5].text = "Genetic Engineering: " + genengineering.ToString();
        skillsTexts[6].text = "Piloting: " + piloting.ToString();
        skillsTexts[7].text = "Tracking: " + tracking.ToString();
        skillsTexts[8].text = "Taming: " + taming.ToString();
        skillsTexts[9].text = "Unlock: " + unlock.ToString();
    }

    void ClearRP()
    {
        pointsRemaining = 15;
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i] = 3;
        }
        meleeDamage = 1;
        rangedDamage = 1;
        hacking = 2;
        healing = 1;
        leadership = 1;
        genengineering = 1;
        piloting = 1;
        tracking = 1;
        taming = 1;
        unlock = 1;

        UpdateAttributes();
    }

    #endregion

    #region Setup Methods

    public void SetupName()
    {

    }
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
        playerData.Level = 1;

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

        if(saveLoadManager.SavePlayerData())
        SceneManager.LoadScene("Junk Processing Plant");
    }
    #endregion
}
