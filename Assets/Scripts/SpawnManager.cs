using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public List<Transform> spawns;

    private ushort currentSpawn = 0;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this; 
        }
    }


    public Vector3 GetNextSpawn() {
        if (currentSpawn >= spawns.Count) {
            currentSpawn = 0;
        }

        return spawns[currentSpawn++].position;
    }
}