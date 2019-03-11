using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsScene
{
    protected Vector2 m_Gravity;
    protected float m_TimeStep;
    protected List<PhysicsObject> m_Actors;

    static float accumulatedTime = 0.0f;

    public Vector2 Gravity { get => m_Gravity; set => m_Gravity = value; }
    public float TimeStep { get => m_TimeStep; set => m_TimeStep = value; }

    public PhysicsScene()
    {
        m_TimeStep = 0.01f;
        m_Gravity = new Vector2(0, 0);
        m_Actors = new List<PhysicsObject>();
    }

    public void AddActor(PhysicsObject actor)
    {
        m_Actors.Add(actor);
    }

    public void RemoveActor(PhysicsObject actor)
    {
        m_Actors.Remove(actor);
    }

    //Update Physics at a fixed timestep
    public void Update(float dt)
    {
        //Debug.Log(dt);

        List<PhysicsObject> dirty = new List<PhysicsObject>();

        accumulatedTime += dt;

        while(accumulatedTime >= m_TimeStep) //check if enough time has passed to do a fixed update
        {
            foreach(PhysicsObject pActor in m_Actors)
            {
                pActor.FixedUpdate(m_Gravity, m_TimeStep); //update each physics object
            }

            accumulatedTime -= m_TimeStep;

            //check for collisions and do stuff
            foreach(PhysicsObject pActor in m_Actors) //for each actor
            {
                foreach(PhysicsObject pOther in m_Actors) //check against all other actors
                {
                    if(pActor == pOther) //if the actors are the same actor
                    {
                        continue; // move to the next iteration of the while loop
                    }

                    if(dirty.Contains(pActor) && dirty.Contains(pOther)) //if both actors are in the dirty list
                    {
                        continue; //their collisions have been checked, so we can move into the next iteration of the while loop
                    }

                    // check for the collision
                    HPRigidBody pRigid = (HPRigidBody)pActor;
                    if(pRigid.CheckCollision(pOther)) //if there is a collision
                    {
                        pRigid.ApplyForceToActor((HPRigidBody)pOther, pRigid.Velocity * pRigid.Mass); //apply NEWTONS THIRD LAW
                        //add both to the dirty list
                        dirty.Add(pRigid);
                        dirty.Add(pOther);
                    }
                }
            }

            dirty.Clear(); //clear out the dirty list.
        }
    }

    public void UpdateGizmos()
    {
        foreach(PhysicsObject pActor in m_Actors)
        {
            pActor.MakeGizmo();
        }
    }

    public void DebugScene()
    {
        int count = 0;
        foreach(PhysicsObject pActor in m_Actors)
        {
            Debug.Log("Actor" + count + " Output:");
            pActor.Debug();
            count++;
        }
    }
}
