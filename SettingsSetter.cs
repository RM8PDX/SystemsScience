using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsSetter : MonoBehaviour {
    public GameObject settingsMenu;
    protected float oldSpeed = 1f;
    protected bool isPaused = false;


    // Sim Instantiator variables.
    public int seed = 2017;

    public int   numberOfVillages    = 10;
    public int   avgVillagePop       = 25;
    public int   populationRange     = 20;
    public int   numVillageNeighbors = 5;
    public float villageSize         = 50f;
    public float popSizeModifier     = 100f;
    public float avgVillageDistance  = 200f;
    public float neighborSearchArea  = 100000f;


    // Log Data variables.
    public bool enableLogging = true;
    public bool useNewLine    = true;
    public float timeStep     = 120;


    // Swadesh List variables
    public int minWordLength = 2;
    public int maxWordLength = 16;


    // Village Controller variables.
    public float driftFreq        = 1f;
    public float driftMagnitude   = 1f;
    public float neighborCalcFreq = 20f;
    public float centralPull      = 100f;


    // Agent Controller variables.
    public float speechPlasticity = 0.01f;
    public float travelFreq       = 60f;
    public float travelFreqRange  = 30f;
    public float visitDuration    = 15f;
    public bool  speechBubbleOn   = false;


    public void Start() {
        DontDestroyOnLoad(gameObject);
    }


    public void Reload() {
        settingsMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
            isPaused = true;
            oldSpeed = Time.timeScale;
            Time.timeScale = 0f;
            settingsMenu.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = false;
            Time.timeScale = oldSpeed;
            settingsMenu.SetActive(false);
        }
    }


    // These setters are necessary so that Unity's default sliders can access them.
    public void SetNumberOfVillages(int n) {
        numberOfVillages = n;
    }

    public void SetAvgVillagePop(int n) {
        avgVillagePop = n;
    }

    public void SetPopulationRange(int n) {
        populationRange = n; 
    }

    public void SetAvgVillageDistance(float n ) {
        avgVillageDistance = n;
    }

    public void SetVillageSize(float n) {
        villageSize = n;
    }

    public void SetNeighborSearchArea(float n) {
        neighborSearchArea = n;
    }

    public void SetNumNeighbors(int n) {
        numVillageNeighbors = n;
    }

    public void EnableLogging(bool truth) {
        enableLogging = truth;
    }

    public void UseNewLine(bool truth) {
        useNewLine = truth;
    }

    public void SetTimeStep(float n) {
        timeStep = n;
    }

    public void SetMinWordLength(int n) {
        minWordLength = n;
    }

    public void SetMaxWordLength(int n) {
        maxWordLength = n;
    }

    public void SetDriftFreq(float n) {
        driftFreq = n;
    }

    public void SetDriftMagnitude(float n) {
        driftMagnitude = n;
    }

    public void SetNeighborCalcFreq(float n) {
        neighborCalcFreq = n;
    }

    public void SetCentralPull(float n) {
        centralPull = n;
    }

    public void SetSpeechPlasticity(float n) {
        speechPlasticity = n;
    }

    public void SetTravelFreq(float n) {
        travelFreq = n;
    }

    public void SetTravelFreqRange(float n) {
        travelFreqRange = n;
    }

    public void SetVisitDuration(float n) {
        visitDuration = n;
    }

    public void SetSpeechBubble(bool truth) {
        speechBubbleOn = truth;
    }
}
