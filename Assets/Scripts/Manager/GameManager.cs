using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Droidzone.Core
{
    public class GameManager : MonoBehaviour
    {

        MusicManager musicManager;

        public List<PC> pcs;

        //Important objects

        SaveLoadManager saveLoadManager;

        public static PlayerData playerData;

        private static PC pc;

        public static PC Pc { get => pc ??= FindAnyObjectByType<PC>(); set => pc = value; }

        public static GameManager Instance;

        #region Initialization
        private void Awake()
        {
            saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
            musicManager = FindAnyObjectByType<MusicManager>();
           
            //Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //Initialized by SaveLoadManager
        /// <summary>
        /// Here we will handle the PC activation, based on their race. They need to be different objects (list above) because they have different riggings.
        /// </summary>
        public void Initialize()
        {

            string race = playerData.race;

           
            ActivatePC(race);
        }

        void ActivatePC(string _race)
        {
            switch (_race)
            {
                case "Alien":
                    pcs[0].gameObject.SetActive(true);
                    break;
                case "Droid":
                    pcs[1].gameObject.SetActive(true);
                    break;
                case "Female Human":
                /// pcs[2].gameObject.SetActive(true); 
                ///  respawner.SetPlayer(pcs[3].gameObject);
                break;

                case "Male Human":
                     pcs[3].gameObject.SetActive(true); 
                     break;
              
                default:
                    break;
            }
        }
        #endregion

        #region Update

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SceneManager.LoadScene(3);
            }


        }
        #endregion
        // Call this method to switch to the exploration state



    }

}


