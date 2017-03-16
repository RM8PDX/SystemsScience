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
    public float keepDist = 100f;

    private Vector3 curVel;

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
        GrabSettings();

        rb = GetComponent<Rigidbody>();
        simCtrl = GetComponent<SimInstantiator>();
        worldMap = FindObjectOfType<SimulationMap>();
        InvokeRepeating("CalcNeighbors", neighborCalcFreq, neighborCalcFreq);
        InvokeRepeating("Drift", villageNumber/100f, driftFreq);

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


    /// <summary>
    /// Repell Vector A off of Vector B proportional to the distance, as if
    /// through magnetic repulsion.
    /// </summary>
    /// <param name="A">Position of object being pushed.</param>
    /// <param name="B">Position of object exerting repelling force.</param>
    /// <param name="maxValue">Maximum magnitude to get pushed by.</param>
    public Vector3 GetPush(Vector3 A, Vector3 B, float maxValue = 100f) {
        Vector3 toA = A - B;
        return Mathf.Max(maxValue - toA.magnitude, 0f) * toA.normalized;
    }


    /// <summary>
    /// Pull Vector A towards Vector B as if through a magnetic pull.
    /// </summary>
    /// <param name="A">Position of object being pulled.</param>
    /// <param name="B">Position of object exerting the pulling force.</param>
    /// <param name="maxValue">Maximum magnitude to get pulled by.</param>
    public Vector3 GetPull(Vector3 A, Vector3 B, float maxValue = 100f) {
        return GetPush(B, A, maxValue);
    }


    /// <summary>
    /// Pull Vector A towards Vector B as if A were attached to a rubber band
    /// to a fixed point, B.
    /// </summary>
    /// <param name="A">Position of object being pulled.</param>
    /// <param name="B">Position of object exerting the pulling force.</param>
    /// <param name="maxValue">Maximum magnitude to get pulled by.</param>
    /// <returns></returns>
    public Vector3 GetElasticPull(Vector3 A, Vector3 B, float maxValue = 100f) {
        Vector3 toB = B - A;
        return Mathf.Min(toB.magnitude, maxValue) * toB.normalized;
    }


    public void Drift() {
        Vector3 rnd = new Vector3(Random.Range(-0.9f, 0.9f), 0f, Random.Range(-0.9f, 0.9f));
        rnd *= driftMagnitude*10;
        rnd += GetElasticPull(gameObject.transform.position, Vector3.zero, keepDist/1000f);

        foreach (VillageCtrl village in neighbors) {
            rnd += GetPush(gameObject.transform.position, village.transform.position, keepDist*2);
        }

        curVel = rnd;
    }


    public void LateUpdate() {
        rb.AddForce(curVel);

        // For whatever reason, Unity is REALLY bad at actually doing this on its' own.
        Vector3 pos = gameObject.transform.position;
        pos.y = 0f;
        gameObject.transform.position = pos;
    }
}
