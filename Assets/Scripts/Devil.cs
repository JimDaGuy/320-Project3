using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : Vehicle
{

    //attributes
    //public Material targetMat;
    public GameObject player;

    // Use this for initialization
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void CalcSteeringForces()
    {
        // if player is in starting zone, don't begin to seek
        if (player.transform.position.x > 170 && player.transform.position.x < 230 
                && player.transform.position.z > 815 && player.transform.position.z < 880)    // x = 170 to 239 z = 880 to 815
        {
                return;
        }

        //find the closest human and hunt them down, paying no mind to building edges
        Vector3 ultimateForce = new Vector3();

        //seeking the nearby lights in the scene
        float maxDistance = 50.0f;
        foreach (GameObject lightOrb in GameObject.FindGameObjectWithTag("Manager").GetComponent<MainSceneManager>().lightOrbs)
        {   
            if (gameObject != null && lightOrb != null)
            {
                float distanceToOrb = Vector3.Distance(gameObject.transform.position, lightOrb.transform.position);
                if (distanceToOrb < maxDistance)
                {
                    ultimateForce = Seek(lightOrb.transform.position) * distanceToOrb;
                    maxDistance = distanceToOrb;
                }
                //delete the light orb if the monster is close enough and temporarily stop the monster
                if(maxDistance < 1.0f)
                {
                    velocity = new Vector3();
                    acceleration = new Vector3();
                    Destroy(lightOrb);
                    break;
                }
            }
        }

        //if no lights were found, look for the player nearby
        if (maxDistance == 50.0f)
        {
            //make sure the player exists and its close enough to the monster to matter
            if(player && Vector3.Distance(gameObject.transform.position, player.transform.position) < maxDistance)
            {
                ultimateForce += Seek(player.transform.position) * seekWeight;
            }
            //wandering if nothing is nearby or the player has despawned
            else
            {
                ultimateForce += Wander() * wanderWeight;
            }
        }
        //avoid obstacles
        ultimateForce += AvoidObstacles() * avoidObstacleWeight;
        //avoid other monsters
        ultimateForce += Separation(GameObject.FindGameObjectWithTag("Manager").GetComponent<MainSceneManager>().monsters) * separationWeight;
        ultimateForce = ultimateForce.normalized * maxForce;
        ApplyForce(ultimateForce);
    }

    //protected override void OnRenderObject()
    //{
    //    base.OnRenderObject();
    //    if (Manager.Instance.debugLines)
    //    {
    //        if (player)
    //        {
    //            targetMat.SetPass(0);
    //            GL.Begin(GL.LINES);
    //            GL.Vertex(transform.position);
    //            GL.Vertex(player.transform.position);
    //            GL.End();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        // decrement player health when a ghost or vampire is "hitting" them
        if (other.transform.parent != null && other.transform.parent.gameObject.tag == "Meat")
        {
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score++;
            Destroy(gameObject);
        }

    }
}
