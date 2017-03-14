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
}
