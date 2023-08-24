using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Races: 0 male human, 1 female human, 2 droid, 3 alien
/// Classes: 0 fighter, 1 biologist, 2 hunter, 3 engineer, 4 hacker
/// </summary>

public class CreationManager : MonoBehaviour
{
    string race;
    string classe;

    public static List<int> attributes = new();

    static int pointsRemaining;

    public TMP_Text raceText;
    public TMP_Text classeText;
    public List<TMP_Text> atts;
    public List<TMP_Text> sheetAtts;
    public TMP_Text prText;
    public List<TMP_Text> statsTexts;
    public TMP_Text skillsText;
         
    private void Start()
    {
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

    public void UpdateAttributes()
    {
        for (int i = 0; i < atts.Count; i++)
        {
            atts[i].text = attributes[i].ToString();
            sheetAtts[i].text = attributes[i].ToString();
        }
        prText.text = "Points remaining: "+pointsRemaining.ToString();

        statsTexts[0].text = (attributes[2] * 10).ToString();
        statsTexts[1].text = (attributes[0] + attributes[2]).ToString();
        statsTexts[2].text = (attributes[0] * attributes[1]).ToString();

        UpdateSkills();
    }

    void UpdateSkills()
    {
        string raceBased = "";
        switch (race) {
            case "Male Human":
                raceBased = "No race based skills \n";
                break;
            case "Female Human":
                raceBased = "Healing +1 \n Leadership +1 \n";
                break;
            case "Droid":
                raceBased = "No race based skills \n";
                break;
            case "Alien":
                raceBased = "Genetics Engineering +1 \n Piloting +1 \n";
                break;
            default:
                raceBased = "No skills based on race";
                break;
        }
        string classeBased = "";
        switch (classe)
        {
            case "Fighter":
                classeBased = "+1d4 melee damage \n +5 health per level \n";
                break;
            case "Hunter":
                classeBased = "+1d4 ranged weapon damage \n Tracking +1 \n";
                break;
            case "Biologist":
                classeBased = "genetics engineering +1 \n Taming +1";
                break;
            case "Engineer":
                classeBased = "Crafting +1 \n Piloting +1 \n";
                break;
            case "Hacker":
                classeBased = "Never fails hacking \n Unlock +1 \n";
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
}
