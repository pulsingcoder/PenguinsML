using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro; // using for text display
using System;

public class PenguinArea : Area  // inheriting from area
{
    public PenguinAgents penguinAgent; // ourt agent
    public Fish fishPrefab;  // our food
    public GameObject penguinBaby; // target 
    public TextMeshPro cummulativeRewardText; // points
    private PenguinAcademy penguinAcademy;
    [HideInInspector]
    public float fishSpeed = 0f;
    [HideInInspector]
    public float feedRadius = 1f;

    public List<GameObject> fishList;
   
    public override void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        PlaceFish(4, fishSpeed);
    }
    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float maxAngle, float minAngle,float maxRadius, float minRadius)
    {
        float radius = minRadius;
        if (maxRadius > minRadius)
        {
            radius = UnityEngine.Random.Range(minRadius, maxRadius);

        }
        return center + Quaternion.Euler(0f,UnityEngine.Random.Range(minAngle,maxAngle),0f) * Vector3.forward * radius;
    }
    private void RemoveAllFish()
    {
        if (fishList !=null)
        {
            for (int i=0;i<fishList.Count;i++)
            {
                if (fishList[i] != null)
                {
                    Destroy(fishList[i]);
                }
            }
            fishList = new List<GameObject>();
        }
    }

    private void PlacePenguin()
    {
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 360f, 0f, 9f, 0f) + Vector3.up * 0.5f; // to avoid down 
        penguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        
    }

    private void PlaceBaby()
    {
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, 45f, -45f, 9f, 4f) + Vector3.up * 0.5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void PlaceFish(int count, float fishSpeed)
    {
        for (int i=0;i<count;i++)
        {
            GameObject fishobject = Instantiate<GameObject>(fishPrefab.gameObject);
            fishobject.transform.position = ChooseRandomPosition(transform.position, 260f, 100f, 13f, 2f) + Vector3.up * 0.5f; // to avoid down 
            fishobject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            fishobject.transform.parent = transform;
            fishList.Add(fishobject);
            fishobject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }
    private void Update()
    {
        cummulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }
}
