using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEntryManager : MonoBehaviour
{
    private List<PlanetLevelEntry> planetLevelEntries = new List<PlanetLevelEntry>();

    private void Start()
    {
        planetLevelEntries = new List<PlanetLevelEntry>(GetComponentsInChildren<PlanetLevelEntry>());
        int id = 0;
        foreach (var planetLevelEntry in planetLevelEntries)
        {
            planetLevelEntry.SetID(id);
            planetLevelEntry.Disable();
            id++;
        }
    }

    public void OnPlanetPress(PlanetLevelEntry entry)
    {
        foreach (var planetLevelEntry in planetLevelEntries)
        {
            if (planetLevelEntry.GetID() != entry.GetID())
            {
                planetLevelEntry.Disable();
            }
        }
    }
}
