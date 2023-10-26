// Copyright (c) 2016 Unity Technologies. MIT license - license_unity.txt
// #NVJOB Simple Boids. MIT license - license_nvjob.txt
// #NVJOB Nicholas Veselov - https://nvjob.github.io
// #NVJOB Simple Boids v1.1.1 - https://nvjob.github.io/unity/nvjob-boids


using System.Collections;
using UnityEngine;

[AddComponentMenu("#NVJOB/Boids/Flock Manager")]


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class FlockManager : MonoBehaviour
{

    #region General Settings
    [Header("General Settings")]
    public Vector2 behavioralCh = new Vector2(2.0f, 6.0f);
    public Vector2 xBounds;
    public Vector2 yBounds;
    public Vector2 zBounds;
    public bool debug;
    #endregion

    #region Flock Settings
    [Header("Flock Settings")]
    [Range(0, 5000)] public int fragmentedFlock = 30;
    [Range(0, 1)] public float fragmentedFlockYLimit = 0.5f;
    [Range(0, 1.0f)] public float posChangeFrequency = 0.5f;
    [Range(0, 100)] public float smoothChFrequency = 0.5f;
    Vector3 flockPos;
    Vector3 flockVelocity;
    #endregion

    [HideInInspector]
    public Boid[] boids;
    public GameObject boidPrefab;
    public float boidSpawnRadius = 10;
    public int boidSpawnCount = 10;
    public Color boidColour;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Awake() {

        GetComponent<MeshRenderer>().enabled = debug;
        Vector3 rdvf = Random.onUnitSphere * fragmentedFlock;
        flockPos = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * fragmentedFlockYLimit), rdvf.z);

        makeBoids();
        StartCoroutine(BehavioralChange());
    }

    void LateUpdate() {

        MoveFlock();
        
    }

    void makeBoids() {
        boids = new Boid[boidSpawnCount];

        Transform boidParent = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        boidParent.SetParent(transform.parent);
        boidParent.GetComponent<SphereCollider>().enabled = false;
        boidParent.GetComponent<MeshRenderer>().enabled = false;

        for (int i = 0; i < boidSpawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * boidSpawnRadius;
            GameObject obj = Instantiate(boidPrefab, boidParent);
            obj.transform.position = pos;
            obj.transform.forward = Random.insideUnitSphere;

            Material material = obj.transform.GetComponentInChildren<MeshRenderer>().material;
            if (material != null) {
                material.color = boidColour;
            }

            Boid boid = obj.GetComponent<Boid>();
            if (boid != null) {
                boid.target = transform;
                boids[i] = boid;
            }
        }
    }

    void MoveFlock() {

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, flockPos, ref flockVelocity, smoothChFrequency);

    }

    IEnumerator BehavioralChange() {
        //--------------

        while (true) {
            yield return new WaitForSeconds(Random.Range(behavioralCh.x, behavioralCh.y));

            if (Random.value < posChangeFrequency) {
                Vector3 rdvf = Random.insideUnitSphere * fragmentedFlock;
                rdvf.x = Mathf.Clamp(rdvf.x, xBounds.x, xBounds.y);
                rdvf.y = Mathf.Clamp(rdvf.y, yBounds.x, yBounds.y);
                rdvf.z = Mathf.Clamp(rdvf.z, zBounds.x, zBounds.y);
                flockPos = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * fragmentedFlockYLimit), rdvf.z);
            }
            
        }

        //--------------
    }

}