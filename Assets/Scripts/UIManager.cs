using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject LeaderBoard;
    public String[] names;
    public String currentName;
    public GameObject Player;
    bool Paused;
    public GameObject Pistol;
    EnemyManager enemyManager;
    public bool GameEnded;
    public GameObject AR;
    public GameObject bloodScene;
    public Text actionSpecifier;
    public GameObject StartPosition;
    public Text EndGameText;
    bool firsttime;
    public GameObject instructionPanel;
    public Text nameUI;

    //Script which controls everything in the canvas
    void Start()
    {
        names = new String[10];
        firsttime = true;
        enemyManager = GetComponent<EnemyManager>();
        actionSpecifier.text = "Start Game";
        PauseGame();
        RestartGame();
        //PlayerPrefs.DeleteAll();

    }


    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("cancel pressed");
            if (Paused == false)
            {
                actionSpecifier.text = "Resume Game";
                PauseGame();
            }
        }
    }


    //Pausing the game function which sets the time scale to 0 and stop all the active scripts
    void PauseGame()
    {

     
            Paused = true;
            Pistol.GetComponent<WeaponScript>().enabled = false;
            AR.GetComponent<WeaponScript>().enabled = false;
            Player.GetComponent<PlayerScript>().enabled = false;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            startPanel.SetActive(true);
            gamePanel.SetActive(false);

            Player.GetComponent<FirstPersonController>().enabled = false;
            //Player.GetComponent<FirstPersonController>().changeLock(false);
            Player.GetComponent<PickUp>().enabled = false;

            Cursor.visible = true;
        
    }

    //Setting back the time scale to 1 and activating the disabled scirpts
    public void Unpause()
    {

        if (GameEnded)
        {
            RestartGame();
        }
        else
        {
            Debug.Log("Unpause");
            Cursor.lockState = CursorLockMode.Locked;
            Paused = false;
            Pistol.GetComponent<WeaponScript>().enabled = true;
            AR.GetComponent<WeaponScript>().enabled = true;
            Time.timeScale = 1f;
            Cursor.visible = false;
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            Player.GetComponent<PickUp>().enabled = true;
            Player.GetComponent<PlayerScript>().enabled = true;
            //Player.GetComponent<FirstPersonController>().changeLock(true);
            Player.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    //Exit button function
    public void Quit()
    {
        Application.Quit();
    }

    //Ending the current game and setting up the parameters for another one
    public void EndGame()
    {
        StartCoroutine(Death());

    }
    IEnumerator Death()
    {

        int score = this.GetComponent<EnemyManager>().score;
        UpdateHS(score);
        Paused = true;
        //disable guns
        Pistol.GetComponent<WeaponScript>().enabled = false;
        AR.GetComponent<WeaponScript>().enabled = false;


        //disabelt the player
        Player.GetComponent<FirstPersonController>().enabled = false;
        //Player.GetComponent<FirstPersonController>().changeLock(false);
        Player.GetComponent<PickUp>().enabled = false;
        EndGameText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        actionSpecifier.text = "Restart Game";
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //go back  to the start screen
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        GameEnded = true;
    }


    //Change the saved high scores as well as the names based on the current game
    void UpdateHS(int score)
    {
        currentName = nameUI.text.ToString();
        Debug.Log(currentName);
        int lastlowerscore = 0;
        int[] scores = new int[10];
        for (int i = 0; i < 5; i++)
        {
            names[i] = PlayerPrefs.GetString("names" + i, "-");//Internal data base use to save the scores
            scores[i] = PlayerPrefs.GetInt("HighScore" + i, 0);

            if (score > scores[i])
                lastlowerscore = i;
        }

        for (int i = 0; i < 5; i++)
        {
            if (i < lastlowerscore)
            {
                PlayerPrefs.SetInt("HighScore" + i, scores[i + 1]);
                PlayerPrefs.SetString("names" + i, names[i + 1]);
            }
            else
            {
                if (i == lastlowerscore)
                {
                    PlayerPrefs.SetInt("HighScore" + i, score);
                    PlayerPrefs.SetString("names" + i, currentName);
                }
                else
                {
                    PlayerPrefs.SetInt("HighScore" + i, scores[i]);
                    PlayerPrefs.SetString("names" + i, names[i]);
                }
            }
        }




    }
    //Get the values from the playerprefs data base and display them
    public void ShowLeaderBoard()
    {
        LeaderBoard.SetActive(true);
        gamePanel.SetActive(false);
        startPanel.SetActive(false);
        int[] scores = new int[10];
        for (int i = 0; i < 5; i++)
        {
            LeaderBoard.transform.GetChild(i + 1).gameObject.GetComponent<Text>().text = "" + (5-i);
            LeaderBoard.transform.GetChild(i + 6).gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("names" + i, "-");
            LeaderBoard.transform.GetChild(i+11).gameObject.GetComponent<Text>().text = ""+PlayerPrefs.GetInt("HighScore" + i, 0);



        }

    }

    public void reTurnToHomePage()
    {
        LeaderBoard.SetActive(false);
        startPanel.SetActive(true);
    }
    
    public void RestartGame()
    {
        Player.transform.position = StartPosition.transform.position;
        Player.transform.rotation = StartPosition.transform.rotation;
        Debug.Log("restarted");
        EndGameText.gameObject.SetActive(false);
        bloodScene.SetActive(true);
        Player.GetComponent<PlayerScript>().Restart();
        Pistol.GetComponent<WeaponScript>().Restart();
        AR.GetComponent<WeaponScript>().Restart();
        enemyManager.Restart();
        GameEnded = false;
        if(!firsttime)
        Unpause();
        firsttime = false;

    }
    public void goToInstruction()
    {
        startPanel.SetActive(false);
        instructionPanel.SetActive(true);
    }
    public void returnInstruction()
    {
        startPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }
}