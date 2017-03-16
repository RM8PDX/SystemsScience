using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Creates a bunch of "villages" (settlements with agents in them) in the game world.
/// </summary>
public class SimInstantiator : MonoBehaviour {
    public int numVillages;
    public int avgPopulation;
    public int popRange;
    public float villageWidth = 10f;
    public float villageSizeModifier = 150f;
    public float avgDist;
    public float neighborSearchArea;
    public int seed = 2017;
    private const int MAX_PLACEMENT_TRIES = 1000;

    public GameObject villagePrefab;
    public GameObject pauseMenu;

    protected SimulationMap worldMap;


    public void GrabAndSetSettings() {
        SettingsSetter settings = FindObjectOfType<SettingsSetter>();
        DontDestroyOnLoad(settings.gameObject);
        settings.settingsMenu = pauseMenu;

        numVillages         = settings.numberOfVillages;
        avgPopulation       = settings.avgVillagePop;
        popRange            = settings.populationRange;
        villageWidth        = settings.villageSize;
        villageSizeModifier = settings.popSizeModifier;
        avgDist             = settings.avgVillageDistance;
        neighborSearchArea  = settings.neighborSearchArea;
    }

	void Start () {
        GrabAndSetSettings();

        Time.timeScale = 1f;
        Random.InitState((int)seed);
        worldMap = FindObjectOfType<SimulationMap>();  // MakeVillage() needs this to work,
        Debug.Log(worldMap);

        for (int i = 0; i < numVillages; i++)
            MakeVillage();

        CalcNeighbors();
        GetComponent<DataSummarizer>().villages = worldMap.villageList;
        PhysicallyPlaceVillages();
        PlaceCamera();
	}


    /// <summary>
    /// Place the camera so that you can clearly see one of the villages and
    /// not some random empty spot with no sense of scale or position.
    /// </summary>
    protected void PlaceCamera() {
        if (worldMap.villageList.Count == 0) {
            Invoke("PlaceCamera", 10f);
            return;
        }
        VillageCtrl rndVillage = worldMap.villageList[Random.Range(0, worldMap.villageList.Count)];
        Vector3 pos = rndVillage.gameObject.transform.position;
        pos.y = 250;
        pos.z -= 100f;
        Camera.main.transform.position = pos;
    }


    /// <summary>
    /// We're just checking to make sure that a given village doesn't
    /// sit right on top of another one.
    /// </summary>
    public bool NoOverlaps(VillageCtrl village) {
        List<VillageCtrl> neighbors = worldMap.GetNeighbors(village.GetPosition());

        foreach (VillageCtrl neighbor in neighbors)
            if (village.GetRect().Overlaps(neighbor.GetRect()))
                return false;

        return true;
    }


    protected VillageCtrl MakeVillage() {
        VillageCtrl clone = Instantiate(villagePrefab).GetComponent<VillageCtrl>();
        clone.population = avgPopulation + Random.Range(-popRange, popRange);
        clone.gameObject.transform.localScale = new Vector3(villageWidth, 1f, villageWidth);

        for (int i = 0; i < MAX_PLACEMENT_TRIES; i++) {
            clone.worldPos = RndCoord();
            if (NoOverlaps(clone))
                break;
        }

        // Book-keeping.
        clone.villageNumber = worldMap.villageList.Count;
        worldMap.villageList.Add(clone);

        return clone;
    }


    public Vector2 RndCoord() {
        float w = worldMap.mapWidth;
        return new Vector2(Random.Range(-w/2f, w/2f), Random.Range(-w/2f, w/2f));
    }


    protected void PhysicallyPlaceVillages() {
        foreach (VillageCtrl village in worldMap.villageList) {
            Vector2 v2pos = village.GetPosition();
            village.gameObject.transform.position = new Vector3(v2pos.x, 0f, v2pos.y);
        }
    }


    public void CalcNeighbors() {
        foreach (VillageCtrl village in worldMap.villageList)
            village.CalcNeighbors();
    }
}
