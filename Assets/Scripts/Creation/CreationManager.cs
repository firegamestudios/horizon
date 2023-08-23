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
    int race;
    int classe;

    public static List<int> attributes = new();

    static int pointsRemaining;

    public TMP_Text raceText;

    private void Start()
    {
        race = 0;
        classe = 0;

        //Start attributes
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i] = 3;
        }

        pointsRemaining = 15;
    }

    public void SetupRace(int _race)
    {
        race = _race;
        raceText.text = "Level 1 " + race;
    }

    public void AttributeIncrease(int att)
    {

    }
    public void AttributeDecrease(int att)
    {

    }
}
