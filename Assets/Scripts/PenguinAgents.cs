using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PenguinAgents : Agent
{
    public GameObject heartPrefab;
    public GameObject regurgitatedFishPrefab;
    private PenguinArea penguinArea;
    private Animator animator;
    private RayPerception3D rayPerception;
    private GameObject baby;
    private bool isFull; // if true than penguin stomach is full

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Convert actions into axis value
        float forward = vectorAction[0];
        float leftOrRight = 0f;
        if (vectorAction[1] == 1f)
        {
            leftOrRight = -1f;

        }
        else if (vectorAction[1] == 2f)
        {
            leftOrRight = 1f;
        }

        // set animator parameters
        animator.SetFloat("Vertical", forward);
        animator.SetFloat("Horizontal", leftOrRight);

        // Tiny negative reward for each step 

        AddReward(-1f / agentParameters.maxStep);

    }
    public override void AgentReset()
    {
        isFull = false;
        penguinArea.ResetArea();
    }
    public override void CollectObservations()
    {
        // has the penguin eaten
        AddVectorObs(isFull);
        // distance between penguin and baby
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));
        //  direction to baby
        AddVectorObs((baby.transform.position - transform.position).normalized);
        //  DIrection penguin is facing 
        AddVectorObs(transform.forward);
        // Ray Perception 
        // *******************************//
        // Parameters 
        // rayDistance : how far to ray cast
        // rayAngles : Angles to raycast (0 is right, 90 is forward, 180 is left)
        // detectable objects: List of tags corresponds to object which agent can see
        // startOffset : Starting height offset of ray from center of object 
        // endOffset : End height offset of ray from center of object
        float rayDistance = 20f;
        float[] rayAngles = { 30, 60, 90, 120, 150 };
        string[] detectableObjects = { "baby", "fish", "wall" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));


    }
    private void Start()
    {
        penguinArea = GetComponentInParent<PenguinArea>();
        baby = penguinArea.penguinBaby;
        animator = GetComponent<Animator>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, baby.transform.position) < penguinArea.feedRadius)
        {
            // close enough, try to feed the fish
            RegurgitatedFish();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
        {
            // another way
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            RegurgitatedFish();
        }
    }

    private void EatFish(GameObject fishObject)
    {
        if (isFull) return;
        isFull = true;
        penguinArea.RemoveSpecificFish(fishObject);
        AddReward(1f);
    }

    private void RegurgitatedFish()
    {
        if (!isFull) // nothing to Regurgitate
            return;
        isFull = false;

        // Spawn Regurgitated fish
        GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = transform.position;
        Destroy(regurgitatedFish, 4f);
        // heart
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = transform.position;
        Destroy(heart, 4f);
        AddReward(1f);

    }
}
