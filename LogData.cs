using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogData : MonoBehaviour {
    public bool enableLogging = true;
    public float timeStep = 30f;  // in seconds
    public bool useNewline = true;

    public string savePath = "C:\\Users\\Ryan\\Desktop\\data.txt";
    protected List<float> averageDistance = new List<float>();
    protected DataSummarizer data;


    public void GrabSettings() {
        SettingsSetter settings = FindObjectOfType<SettingsSetter>();

        enableLogging = settings.enableLogging;
        useNewline    = settings.useNewLine;
        timeStep      = settings.timeStep;
    }


    public void Start() {
        data = FindObjectOfType<DataSummarizer>();
        InvokeRepeating("LogAvg", 5f, timeStep);
        //savePath = Path.Combine(Application.persistentDataPath, "systems_science.txt");
        savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        savePath += "\\systems_science_data.csv";
    }


    public void LogAvg() {
        string s;
        if (useNewline)
            s = string.Format("{0:0.00}\n", data.AvgDist());
        else
            s = string.Format("{0:0.00}, ", data.AvgDist());
        File.AppendAllText(savePath, s);
    }

    public void WriteSettings() {
        SettingsSetter settings = FindObjectOfType<SettingsSetter>();
        string s = "";
        s += string.Format("seed, {0}\n", settings.seed);
        s += string.Format("numberOfVillages, {0}\n"   , settings.numberOfVillages   );
        s += string.Format("avgVillagePop, {0}\n"      , settings.avgVillagePop      );
        s += string.Format("populationRange, {0}\n"    , settings.populationRange    );
        s += string.Format("numVillageNeighbors, {0}\n", settings.numVillageNeighbors);
        s += string.Format("villageSize, {0}\n"        , settings.villageSize        );
        s += string.Format("popSizeModifier, {0}\n"    , settings.popSizeModifier    );
        s += string.Format("avgVillageDistance, {0}\n" , settings.avgVillageDistance );
        s += string.Format("neighborSearchArea, {0}\n" , settings.neighborSearchArea );
        s += string.Format("enableLogging, {0}\n"      , settings.enableLogging      );
        s += string.Format("useNewLine, {0}\n"         , settings.useNewLine         );
        s += string.Format("timeStep, {0}\n"           , settings.timeStep           );
        s += string.Format("minWordLength, {0}\n"      , settings.minWordLength      );
        s += string.Format("maxWordLength, {0}\n"      , settings.maxWordLength      );
        s += string.Format("driftFreq, {0}\n"          , settings.driftFreq          );
        s += string.Format("driftMagnitude, {0}\n"     , settings.driftMagnitude     );
        s += string.Format("neighborCalcFreq, {0}\n"   , settings.neighborCalcFreq   );
        s += string.Format("centralPull, {0}\n"        , settings.centralPull        );
        s += string.Format("speechPlasticity, {0}\n"   , settings.speechPlasticity   );
        s += string.Format("travelFreq, {0}\n"         , settings.travelFreq         );
        s += string.Format("travelFreqRange, {0}\n"    , settings.travelFreqRange    );
        s += string.Format("visitDuration, {0}\n"      , settings.visitDuration      );
        s += string.Format("speechBubbleOn, {0}\n"     , settings.speechBubbleOn     );
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        path += "\\settings.csv";
        File.WriteAllText(path, s);
    }
}
