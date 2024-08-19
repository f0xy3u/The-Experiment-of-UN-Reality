using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game : MonoBehaviour
{
    //Variable setup
    List<string> entActivity = new List<string>();
    List<string> entActivityDes = new List<string>();
    List<int> entActivityVal = new List<int>();
    public float raidStatus = 0f;
    public float actionWait = 10f;
    public float researchStatus = 0f;
    double timeFromStart = 0;
    bool actionRunned = false;
    bool updateRunned = false;
    bool actionTimeUpdated = false;
    int activity;
    public float timeChanger = 0.9f;
    int speakAmnout = 0;
    bool notificationRunning = false;

    //Object setup
    public TMP_Text nadpis;
    public TMP_Text popis;
    public TMP_Text hodnota;
    public Canvas menu;
    public Slider rsrchStatusSlider;
    public Slider raidStatusSlider;
    public Image failNotification;

    //Final game check variables setup
    bool win = false;
    bool fail = false;
    bool timeLimit = false;
    float savedTime;
    int failID;
    
    //Timed programs
    void ActionStarter() {
        int rounded = (int)Math.Round(timeFromStart, 0);
        if (rounded % 10 == 0 && !actionRunned && rounded != 0) {
            actionRunned = true;
            activity = ActivitySelector();
            Debug.Log("aktualizace lol");
            menuOpener();
            nadpis.text = entActivity[activity];
            popis.text = entActivityDes[activity];
            hodnota.text = entActivityVal[activity].ToString();
        }

        //reset
        if (rounded % 10 != 0) actionRunned = false;
    }

    void researchStatusUpdate() {
        int rounded = (int)Math.Round(timeFromStart, 0);
        if (rounded % 3 == 0 && !updateRunned) {
            updateRunned = true;
            researchStatus += 1;
        }

        if (researchStatus % 10 == 0 && !actionTimeUpdated && researchStatus != 0) {
            actionTimeUpdated = true;
            actionWait *= 0.9f;
            if (raidStatus >= 2f) raidStatus -= 2f;
        }

        //reset
        if (rounded % 3 != 0) updateRunned = false;
    }

    //Start
    void Start()
    {
        setUp();
    }

    //Update
    void Update()
    {
        //Time and display updater
        timeFromStart += Time.deltaTime;
        displayUpdate();

        //Checking for game status
        gameStatusCheck();

        //Value corrector
        if (raidStatus < 0) raidStatus = 0;
        if (researchStatus < 0) researchStatus = 0;

        //Program
        if (!win && !fail) {
            ActionStarter();
            researchStatusUpdate();
        } else {
            if (win) PlayerPrefs.SetInt("endGameStatus", 1);
            if (fail) {
                switch (failID) {
                    case 1:
                        PlayerPrefs.SetInt("endGameStatus", 2);
                        SceneManager.LoadScene(1);
                        return;
                    case 2:
                        PlayerPrefs.SetInt("endGameStatus", 3);
                        SceneManager.LoadScene(1);
                        return;
                }
            }
            SceneManager.LoadScene(1);
        }
    }

    //Code for game logic
    void gameLogic(int activity, int playerChoice) {
        //Raid status updater
            if (playerChoice == 1) {
                Debug.Log("moznost 1");
                raidStatus += entActivityVal[activity];
                researchStatus -= 5f;
            }
            if (playerChoice == 2) {
                if (speakAmnout < 10) {
                    Debug.Log("moznost 2");
                    raidStatus += entActivityVal[activity];
                    speakAmnout += 1;
                }
            }
            if (playerChoice == 3) {
                Debug.Log("moznost 3");
                int rng = UnityEngine.Random.Range(1, 2);
                if(rng == 1) {
                    raidStatus -= 10;
                }
                if(rng == 2) {
                    raidStatus += entActivityVal[activity] * 1.5f;
                }
            }
            if (playerChoice == 4) {
                Debug.Log("moznost 4");
                raidStatus -= 20;
                researchStatus -= 20;
            }

        //Close menu
            menuCloser();
    }

    int ActivitySelector() {
        int num = UnityEngine.Random.Range(1, 125);
        switch (num) {
            case <=30:
                return(1);
            case <=50:
                return(2);
            case <=68:
                return(3);
            case <=82:
                return(4);
            case <=94:
                return(5);
            case <=104:
                return(6);
            case <=112:
                return(7);
            case <=119:
                return(8);
            case <=124:
                return(9);
            case <=125:
                return(10);
            default:
                return(0);
        }
    }

    void menuOpener() {
        menu.gameObject.SetActive(true);
    }

    void menuCloser() {
        menu.gameObject.SetActive(false);
    }

    public void buttonListener(int buttonSelected) {
        switch (buttonSelected) {
            case 1:
                gameLogic(activity, 1);
                return;
            case 2:
                gameLogic(activity, 2);
                return;
            case 3:
                gameLogic(activity, 3);
                return;
            case 4:
                gameLogic(activity, 4);
                return;                
        }
    }

    void displayUpdate() {
        rsrchStatusSlider.value = researchStatus;
        raidStatusSlider.value = raidStatus;
    }

    void gameStatusCheck() {
        if (researchStatus > 100) win = true;
        if (raidStatus > 100) {
            fail = true;
            failID = 1;
        }

        if (timeFromStart > 30) {
            if (researchStatus <= 10) {
                if (!timeLimit) {
                    savedTime = (float)timeFromStart;
                    timeLimit = true;
                } else {
                    if (!notificationRunning) StartCoroutine(failNotificationAnimation());
                    Debug.Log((float)timeFromStart - savedTime);
                    if((float)timeFromStart - savedTime > 90) {
                        failID = 2;
                        fail = true;
                    }
                }
            }
        }
    }

    IEnumerator failNotificationAnimation() {
        notificationRunning = true;
        failNotification.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        failNotification.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        notificationRunning = false;
    }

    //Setupping code
    void setUp() {
        Debug.Log("start");
        //Activity names
        entActivity.Add("Ignoring Instructions");
        entActivity.Add("Passive Resistance");
        entActivity.Add("Communicating with Other Subjects");
        entActivity.Add("Altering the Environment");
        entActivity.Add("Random Actions");
        entActivity.Add("Tech Sabotage");
        entActivity.Add("Creating Chaos");
        entActivity.Add("Violence Among Subjects");
        entActivity.Add("Rebellion Against the Player");
        entActivity.Add("Escaping the Simulation");

        //Activity description
        entActivityDes.Add("The subjects are ignoring your commands, disrupting the planned experiment.");
        entActivityDes.Add("The subjects are moving slowly and seem reluctant to complete their tasks.");
        entActivityDes.Add("The subjects are secretly exchanging information, forming their own theories about the experiment.");
        entActivityDes.Add("The subjects are tampering with the environment, shifting objects and setting up obstacles.");
        entActivityDes.Add("The subjects are behaving erratically, performing random actions that defy logic.");
        entActivityDes.Add("The subjects are sabotaging the technology, interfering with monitoring systems.");
        entActivityDes.Add("The subjects are causing chaos, destroying equipment and triggering false alarms.");
        entActivityDes.Add("The subjects have turned against each other, engaging in violent conflicts.");
        entActivityDes.Add("The subjects have discovered your influence and are attempting to overthrow your control.");
        entActivityDes.Add("The subjects have found a way to escape the simulation, breaking free from your control.");

        // Activity values
        entActivityVal.Add(1);
        entActivityVal.Add(3);
        entActivityVal.Add(5);
        entActivityVal.Add(7);
        entActivityVal.Add(10);
        entActivityVal.Add(15);
        entActivityVal.Add(20);
        entActivityVal.Add(25);
        entActivityVal.Add(30);
        entActivityVal.Add(50);
    }
}
