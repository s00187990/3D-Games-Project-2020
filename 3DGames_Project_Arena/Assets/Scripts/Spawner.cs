using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Spawner : MonoBehaviour
{


    public GameObject[] spawnObjects;
    public Transform[] Locations;
    public float SpawnEvery;

    public bool SpawnRandom = true; 
    public bool SpawnOnStart;

    SpawnLocation[] spawnLocations;
    float timeToSpawn;




    private void Awake()
    {
        PopulateLocations();
        timeToSpawn = Time.time + SpawnEvery;
    }

    private void PopulateLocations()
    {
        spawnLocations = new SpawnLocation[Locations.Length];
        for (int i = 0; i < Locations.Length; i++)
        {
            int prefabIndex = i < spawnObjects.Length ? i : spawnObjects.Length - 1;
            spawnLocations[i] = new SpawnLocation(Locations[i], prefabIndex);
            if (SpawnOnStart)
                Spawn(spawnLocations[i]);
        }
    }

    private void Spawn(SpawnLocation spawnLocation)
    {
        if (SpawnRandom)
        {
            spawnLocation.Spawn(spawnObjects[UnityEngine.Random.Range(0, spawnObjects.Length)]);
            return;
        }
        spawnLocation.Spawn(ref spawnObjects);
    }

    private void Update()
    {



        if (Time.time >= timeToSpawn)
        {
            timeToSpawn = Time.time + SpawnEvery;
            if (spawnLocations == null) PopulateLocations();
            foreach (var item in spawnLocations)
            {
                if (item.HasWeapon)
                {
                    if (item.CurrentSpawnObject == null)
                        item.HasWeapon = false;
                    continue;
                }
                else
                {
                    Spawn(item);
                    break;
                }
            }
        }
    }


    private class SpawnLocation
    {
        Transform Transform;
        public bool HasWeapon;
        public GameObject CurrentSpawnObject;
        public Vector3 location { get { return Transform.position; } }
        public int PrefabID;


        public SpawnLocation(Transform transform,int prefabIndex)
        {
            Transform = transform;
            PrefabID = prefabIndex;
        }

        public void Spawn(GameObject gameObject)
        {
            CurrentSpawnObject = Instantiate(gameObject, location, Transform.rotation);
            HasWeapon = true;
        }
        public void Spawn(ref GameObject[] prefabs)
        {
            Spawn(prefabs[PrefabID]);
        }

    }
}
