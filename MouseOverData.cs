using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverData : MonoBehaviour {
    public Text uitxt;
    public Text langTxt;
    public GameObject PingPrefab;

    protected VillageCtrl lastVillage;
    protected List<PingCtrl> pings = new List<PingCtrl>();


	void Update () {
        // If our mouse is over a village, display information
        // about that village in the HUD.
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, Mathf.Infinity)) {
            // Check what village is under the mouse right now.
            VillageCtrl villageCtrl = hit.collider.gameObject.GetComponent<VillageCtrl>();
            if (villageCtrl == null || villageCtrl == lastVillage)   // don't update if the village hasn't changed
                return;
            lastVillage = villageCtrl;

            // Display basic village stats.
            uitxt.text = string.Format(
              "Village Number {0}\nPopulation: {1}\nx: {2:0.##}\ty: {3:0.##}",
              villageCtrl.villageNumber,
              villageCtrl.population   ,
              villageCtrl.worldPos.x   ,
              villageCtrl.worldPos.y   );

            // Display the current language for the group.
            if (Input.GetKeyDown(KeyCode.Space))
                DisplayVillageLanguage(villageCtrl);

            // Make some visual lines the go from the current
            // village to each of its official neighbors.
            KillPings();
            StartPings(villageCtrl);
        }
    }


    public void DisplayVillageLanguage(VillageCtrl villageCtrl) {
        AgentCtrl agent = villageCtrl.agents[0];
        string s = "";
        bool odd = true;
        foreach (Phoneme phone in agent.idiolect.Keys) {
            float f = villageCtrl.AvgPronunciation(phone);

            s += string.Format("{0} : {1:0.00}\t\t", phone.glyph, f);

            odd = !odd;
            if (odd)
                s += "\n";
        }

        langTxt.text = s;
    }


    public void StartPings(VillageCtrl sourceVillage) {
        foreach (VillageCtrl neighbor in sourceVillage.neighbors) {
            GameObject pingClone = Instantiate(PingPrefab);
            PingCtrl ctrl = pingClone.GetComponent<PingCtrl>();
            ctrl.ptA = sourceVillage.gameObject.transform.position;
            ctrl.ptB = neighbor.gameObject.transform.position;
            pingClone.transform.position = ctrl.ptA;
            pings.Add(ctrl);
        }
    }


    public void KillPings() {
        foreach (PingCtrl ping in pings)
            Destroy(ping.gameObject);
        pings.Clear();
    }
}
