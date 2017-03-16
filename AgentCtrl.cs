using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AgentCtrl : MonoBehaviour {
    public float speechPlasticity;
    public float travelFreq;
    public float travelFreqRange;
    public float visitDuration;
    public bool speechBubbleOn;
    public Material defaultMaterial;
    public Material visitingMaterial;
    public GameObject speechBubble;
    public Dictionary<Phoneme, float> idiolect = new Dictionary<Phoneme, float>();

    protected SettingsSetter settings;
    protected SwadeshList swadesh;
    protected VillageCtrl home;


    public void GrabSettings() {
        settings = FindObjectOfType<SettingsSetter>();

        speechPlasticity = settings.speechPlasticity;
        travelFreq       = settings.travelFreq;
        travelFreqRange  = settings.travelFreqRange;
        visitDuration    = settings.visitDuration;
        speechBubbleOn   = settings.speechBubbleOn;
    }


    public void Start() {
        GrabSettings();

        gameObject.GetComponent<Renderer>().material = defaultMaterial;
        swadesh = GameObject.FindGameObjectWithTag("GameController").GetComponent<SwadeshList>();
        foreach (Phoneme phone in swadesh.phonemes)
            idiolect.Add(phone, Random.Range(0.01f, 1.00f));

        MaybeTransport();
    }


    public void SetHome(VillageCtrl home) {
        this.home = home;
    }


    /// <summary>
    /// Every transFreqRange seconds, we'll randomly transport to another 
    /// </summary>
    public void MaybeTransport() {
        if (Random.Range(0, 100) % 2 == 0 && home.neighbors.Count > 0)
            RandomlyTransport();

        Invoke("MaybeTransport", travelFreq + Random.Range(0.0f, 1.0f) * travelFreqRange);
    }


    public void MagicallyTransport(VillageCtrl destination) {
        Vector3 pos = destination.gameObject.transform.position;
        Vector3 destScale = destination.gameObject.transform.localScale;
        pos.x += Random.Range(-destScale.x/4f, destScale.x/4f);
        pos.z += Random.Range(-destScale.z/4f, destScale.z/4f);
        pos.y = 1f;
        gameObject.transform.position = pos;
    }


    public IEnumerable<float> IntermediateDistances() {
        float sum = 0f;
        foreach (VillageCtrl neighbor in home.neighbors) {
            sum += Vector2.Distance(home.GetPosition(), neighbor.GetPosition());
            yield return sum;
        }
    }


    public float TotalNeighborDistance() {
        float sum = 0f;
        foreach (VillageCtrl neighbor in home.neighbors)
            sum += Vector2.Distance(home.GetPosition(), neighbor.GetPosition());
        return sum;
    }


    public void RandomlyTransport() {
        gameObject.GetComponent<Renderer>().material = visitingMaterial;

        // We're randomly chosing a neighbor, but weighted by how close it is.
        float rnd = Random.Range(0.0f, 1.0f);
        float sum = TotalNeighborDistance();
        int idx = 0;
        foreach (float runningSum in IntermediateDistances()) {
            // sum - runningSum is required so that closer places are more
            // liekly to be chosen, instead of the other way around.
            if ((sum - runningSum) / sum <= rnd)
                break;
            idx++;
        }
        VillageCtrl destination = home.neighbors[idx];
        MagicallyTransport(destination);
        Invoke("GoHome", visitDuration * Random.Range(0.5f, 1.5f));
    }


    protected void GoHome() {
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
        MagicallyTransport(home);
    }


	void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "agent") {
            AgentCtrl stranger = collision.gameObject.GetComponent<AgentCtrl>();
            foreach (Phoneme phone in RndWord()) {
                float hisPoununciation = stranger.GetPronunciation(phone);
                float ourPronunciation = idiolect[phone];

                if (hisPoununciation > ourPronunciation)
                    idiolect[phone] = ourPronunciation + speechPlasticity;
                else
                    idiolect[phone] = ourPronunciation - speechPlasticity;

                //if (ourPronunciation > 0.90)
                //    Debug.Log("Phoneme mutation should go here.");

                if (speechBubbleOn) {
                    GameObject bubble = Instantiate(speechBubble);
                    Vector3 pos = gameObject.transform.position;
                    pos.y = 10f;
                    bubble.transform.position = pos;
                }
            }
        }
	}


    public Phoneme RndPhoneme() {
        int rndIdx = Random.Range(0, idiolect.Count - 1);
        return idiolect.ElementAt(rndIdx).Key;
    }


    public List<Phoneme> RndWord() {
        if (swadesh == null)
            return new List<Phoneme>();
        int rndIdx = Random.Range(1, swadesh.swadishDict.Count - 1);
        return swadesh.swadishDict.ElementAt(rndIdx).Value;
    }


    public float GetPronunciation(Phoneme query) {
        return idiolect[query];
    }
}
