using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataSummarizer : MonoBehaviour {
    public List<VillageCtrl> villages;
    public Text txt;

    protected Phoneme[] phonemeOrder;


    public void PopulatePhonemeOrder() {
        AgentCtrl agent = villages[0].agents[0];
        phonemeOrder = agent.idiolect.Keys.ToArray();
    }


    /// <summary>
    /// Calculates the difference in two dialects. Also, seattle and portland
    /// just seemed like they'd be better names than villageA and villageB.
    /// </summary>
    public float CalcDiff(VillageCtrl seattle, VillageCtrl portland) {
        float diffSq = 0f;
        foreach (Phoneme phone in phonemeOrder) {
            float s = seattle.AvgPronunciation(phone) * 100f;
            float p = portland.AvgPronunciation(phone) * 100f;
            s = Mathf.Round(s);
            p = Mathf.Round(p);
            float diff = s - p;
            diffSq += (diff * diff) / 100f;
        }
        return Mathf.Sqrt(diffSq);
    }


    public float AvgDist() {
        if (villages.Count == 0)
            return 0f;

        if (phonemeOrder == null)
            PopulatePhonemeOrder();

        float oldSpeed = Time.timeScale;
        Time.timeScale = 0f;

        float total = 0f;

        float n = villages.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = i + 1; j < n; j++)
                total += CalcDiff(villages[i], villages[j]);

        Time.timeScale = oldSpeed;
        // This is just the average, trust me.
        return 2 * total / (n * n + n);
    }


    public void DrawMatrix() {
        if (villages.Count == 0)
            return;

        if (phonemeOrder == null)
            PopulatePhonemeOrder();

        float oldSpeed = Time.timeScale;
        Time.timeScale = 0f;

        string s = "";
        float totalDist = 0f;

        foreach (VillageCtrl rowVillage in villages) {
            foreach (VillageCtrl colVillage in villages) {
                float dist = CalcDiff(rowVillage, colVillage);
                totalDist += dist;
                s += string.Format("{000:0.00}  ", dist);
            }

            s += "\n";
        }

        float avgDist = totalDist / (villages.Count * villages.Count);
        txt.text = string.Format("Avgerage Distance: {0:0.00}\n{1}", avgDist, s);

        Time.timeScale = oldSpeed;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            DrawMatrix();
        }
    }
}
