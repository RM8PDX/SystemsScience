using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationMap : MonoBehaviour {
    public float mapWidth;
    public List<VillageCtrl> villageList = new List<VillageCtrl>();


    public List<VillageCtrl> GetNeighbors(Vector2 origin, int numNeighbors = 5) {
        // I can't figure out why my commented out code doesn't work, so
        // here's the dumb way of doing it (as in it doesn't scale well).
        List<VillageCtrl> neighbors = villageList.OrderBy(
            x => Vector2.Distance(origin, x.GetPosition())
        ).ToList();

        int maxVal = Mathf.Min(numNeighbors, neighbors.Count);
        if (maxVal <= 0)
            return new List<VillageCtrl>();
        else
            return neighbors.GetRange(0, maxVal);
        //List<VillageCtrl> neighbors = new List<VillageCtrl>();
        //foreach (VillageCtrl neighbor in villageList)
        //    if (r.Overlaps(neighbor.GetRect()))
        //        neighbors.Add(neighbor);
        //return neighbors;
    }
}
