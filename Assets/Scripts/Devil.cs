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

        //seeking the lights in the scene
        float mainLightDistance = float.MaxValue;
        foreach (GameObject lightOrb in GameObject.FindGameObjectWithTag("Manager").GetComponent<MainSceneManager>().lightOrbs)
        {
            float distanceToOrb = Vector3.Distance(gameObject.transform.position, lightOrb.transform.position);
            if ( distanceToOrb < mainLightDistance)
            {
                ultimateForce = Seek(lightOrb.transform.position) * distanceToOrb;
                mainLightDistance = distanceToOrb;
            }
        }

        //seeking the player in the scene
        if (player)
        {
            //if there are no lights, pursue the player soley
            if(mainLightDistance == float.MaxValue)
                ultimateForce += Seek(player.transform.position) * seekWeight;
            //the devil has gotten relatively close to the light, start to turn their attention back towards the player
            //else if(mainLightDistance < 20.0f)
            //    ultimateForce += Seek(player.transform.position) * seekWeight / 2;
        }
        //wandering once the player has despawned
        else
        {
            ultimateForce += Wander() * wanderWeight;
        }
        //avoid obstacles
        ultimateForce += AvoidObstacles() * avoidObstacleWeight;
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
