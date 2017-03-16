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
    public float neighborPush     = 200f;


    // Agent Controller variables.
    public float speechPlasticity = 0.01f;
    public float travelFreq       = 60f;
    public float travelFreqRange  = 30f;
    public float visitDuration    = 15f;
    public bool  speechBubbleOn   = false;


    public void WriteSettings() {
        string s = "";
        s += string.Format("seed, {0}\n", seed);
        s += string.Format("numberOfVillages, {0}\n"   , numberOfVillages   );
        s += string.Format("avgVillagePop, {0}\n"      , avgVillagePop      );
        s += string.Format("populationRange, {0}\n"    , populationRange    );
        s += string.Format("numVillageNeighbors, {0}\n", numVillageNeighbors);
        s += string.Format("villageSize, {0}\n"        , villageSize        );
        s += string.Format("popSizeModifier, {0}\n"    , popSizeModifier    );
        s += string.Format("avgVillageDistance, {0}\n" , avgVillageDistance );
        s += string.Format("neighborSearchArea, {0}\n" , neighborSearchArea );
        s += string.Format("enableLogging, {0}\n"      , enableLogging      );
        s += string.Format("useNewLine, {0}\n"         , useNewLine         );
        s += string.Format("timeStep, {0}\n"           , timeStep           );
        s += string.Format("minWordLength, {0}\n"      , minWordLength      );
        s += string.Format("maxWordLength, {0}\n"      , maxWordLength      );
        s += string.Format("driftFreq, {0}\n"          , driftFreq          );
        s += string.Format("driftMagnitude, {0}\n"     , driftMagnitude     );
        s += string.Format("neighborCalcFreq, {0}\n"   , neighborCalcFreq   );
        s += string.Format("centralPull, {0}\n"        , centralPull        );
        s += string.Format("neighborPush, {0}\n"       , neighborPush       );
        s += string.Format("speechPlasticity, {0}\n"   , speechPlasticity   );
        s += string.Format("travelFreq, {0}\n"         , travelFreq         );
        s += string.Format("travelFreqRange, {0}\n"    , travelFreqRange    );
        s += string.Format("visitDuration, {0}\n"      , visitDuration      );
        s += string.Format("speechBubbleOn, {0}\n"     , speechBubbleOn     );
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
    public void SetSeed(int n) {
        seed = n;
    }

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

    public void SetNeighborPush(float n) {
        neighborPush = n;
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
