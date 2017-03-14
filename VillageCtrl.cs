using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageCtrl : MonoBehaviour, IQuadTreeObject  {
    public GameObject agentPrefab;
    public int population;
    public int villageNumber;
    public List<VillageCtrl> neighbors = new List<VillageCtrl>();
    public List<AgentCtrl> agents;

    public Rigidbody rb;
    public SimInstantiator simCtrl;
    public SimulationMap worldMap;
    public float driftFreq;
    public float neighborCalcFreq;
    public float driftMagnitude;
    public float centralPull;
    public int neighborsPerVillage;

    public Vector2 worldPos {
        get {
            return GetPosition();
        } set {
            gameObject.transform.position = new Vector3(value.x, 0f, value.y);
        }
    }


    public void GrabSettings() {
        SettingsSetter settings = FindObjectOfType<SettingsSetter>();

        driftFreq           = settings.driftFreq;
        driftMagnitude      = settings.driftMagnitude;
        neighborCalcFreq    = settings.neighborCalcFreq;
        centralPull         = settings.centralPull;
        neighborsPerVillage = settings.numVillageNeighbors;
    }


    public void Start() {
        rb = GetComponent<Rigidbody>();
        simCtrl = GetComponent<SimInstantiator>();
        worldMap = FindObjectOfType<SimulationMap>();
        InvokeRepeating("CalcNeighbors", neighborCalcFreq, neighborCalcFreq);
        InvokeRepeating("Drift", villageNumber, driftFreq);

        Vector3 scale = gameObject.transform.localScale;
        for (int i = 0; i < population; i++) {
            // Make a new agent.
            GameObject clone = Instantiate(agentPrefab);
            AgentCtrl cloneCtrl = clone.GetComponent<AgentCtrl>();
            agents.Add(cloneCtrl);
            cloneCtrl.SetHome(this);

            // Set the agent moving along a random direction.
            Rigidbody cloneRB = clone.GetComponent<Rigidbody>();
            cloneRB.velocity = new Vector3(Random.Range(0f, 100f), 0f, Random.Range(0f, 100f));

            // Put the agent somewhere in the village.
            float rndX = Random.Range(-scale.x/2f, +scale.x/2f);
            float rndZ = Random.Range(-scale.z/2f, +scale.z/2f);
            rndX += gameObject.transform.position.x;
            rndZ += gameObject.transform.position.z;
            clone.transform.position = new Vector3(rndX, 1f, rndZ);    // 1 so agents aren't spawned inside the ground
        }
    }


    public Dictionary<Phoneme, float> GetVillageLanguage() {
        Dictionary<Phoneme, float> avgLang = agents[0].idiolect;
        foreach (Phoneme key in avgLang.Keys)
            avgLang[key] = 0f;

        // We'll add the values into avgLang first, then divide in a separate step.
        foreach (AgentCtrl agent in agents) {
            foreach (Phoneme phone in agent.idiolect.Keys) {
                avgLang[phone] += agent.idiolect[phone];
            }
        }

        // Now go through and divide each number.
        float n = (float)agents.Count;
        Phoneme[] keys = avgLang.Select(x => x.Key).ToArray();   // this temp array prevents an out of sync error
        foreach (Phoneme phone in keys)
            avgLang[phone] /= n;

        return avgLang;
    }


    public float AvgPronunciation(Phoneme phone) {
        float sum = 0f;
        foreach (AgentCtrl agent in agents)
            sum += agent.idiolect[phone];
        return sum / agents[0].idiolect.Count;
    }


    public Vector2 GetPosition() {
        Vector3 pos = gameObject.transform.position;
        return new Vector2(pos.x, pos.z);
    }


    public Rect GetRect() {
        Vector3 scale = gameObject.transform.localScale;
        Vector2 worldPos = GetPosition();
        return new Rect(worldPos.x, worldPos.y, scale.x, scale.z);
    }


    public void CalcNeighbors() {
        neighbors.Clear();  // in case we want to recalculate this after some changes
        if (worldMap == null)
            worldMap = FindObjectOfType<SimulationMap>();
        neighbors.AddRange( worldMap.GetNeighbors(GetPosition(), neighborsPerVillage) );
    }


    public void Drift() {
        Vector3 rnd = new Vector3(Random.Range(-0.9f, 0.9f), 0f, Random.Range(-0.9f, 0.9f));
        rnd *= driftMagnitude;
        Vector3 toCenter = -1 * gameObject.transform.position.normalized;
        rnd += toCenter * centralPull;
        rb.AddForce(rnd);
    }
}
