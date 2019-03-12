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

    public delegate bool CollisionFunction(PhysicsObject inputA, PhysicsObject inputB);
    public static CollisionFunction[] CollisionFunctions = { Plane2Plane, Plane2Sphere, Plane2Box, Sphere2Plane, Sphere2Sphere, Sphere2Box, Box2Plane, Box2Sphere, Box2Box };

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

            /*//check for collisions and do stuff
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
            }*/

            CheckForCollisions();

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

    public void CheckForCollisions()
    {
        int actorCount = m_Actors.Count;

        for(int outer = 0; outer < actorCount - 1; outer++)
        {
            for(int inner = outer + 1; inner < actorCount; inner++)
            {
                PhysicsObject outerObject = m_Actors[outer];
                PhysicsObject innerObject = m_Actors[inner];
                int functionIndex = (innerObject.ShapeID * PhysicsObject.SHAPE_COUNT) + outerObject.ShapeID;
                
                if(CollisionFunctions[functionIndex] != null)
                {
                    CollisionFunctions[functionIndex](outerObject, innerObject);
                }
            }
        }
    }

    public static bool Plane2Plane(PhysicsObject inputA, PhysicsObject inputB)
    {
        return false;
    }

    public static bool Plane2Sphere(PhysicsObject inputA, PhysicsObject inputB)
    {
        HPPlane planeA;
        HPSphere sphereB;
        if (inputA.ShapeID == 0)
        {
            planeA = (HPPlane)inputA;
            sphereB = (HPSphere)inputB;
        }
        else
        {
            planeA = (HPPlane)inputB;
            sphereB = (HPSphere)inputA;
        }

        if (planeA != null && sphereB != null)
        {
            Vector2 collisionNormal = planeA.Normal;
            float planeToSphere = Vector2.Dot(sphereB.Position, collisionNormal) + planeA.Distance;

            if(Mathf.Abs(planeToSphere) <= sphereB.Radius)
            {
                sphereB.ApplyForce(-sphereB.Velocity);

                return true;
            }

            /*float sphereToPlane = Vector2.Dot(sphereB.Position, collisionNormal) - planeA.Distance; //distance form the center of the circle to the plane

            if(sphereToPlane < 0)
            {
                collisionNormal *= -1;
                sphereToPlane *= -1;
            }

            float intersection = sphereB.Radius - sphereToPlane;
            if (intersection > 0)
            {
                sphereB.ApplyForce(-sphereB.Velocity);

                return true;
            }*/
        }


        return false;
    }

    public static bool Plane2Box(PhysicsObject inputA, PhysicsObject inputB)
    {
        return false;
    }

    public static bool Sphere2Plane(PhysicsObject inputA, PhysicsObject inputB)
    {
        return Plane2Sphere(inputB, inputA);
    }

    public static bool Sphere2Sphere(PhysicsObject inputA, PhysicsObject inputB)
    {
        //cast the objects to spheres
        HPSphere sphereA = (HPSphere)inputA;
        HPSphere sphereB = (HPSphere)inputB;

        if(sphereA != null && sphereB != null) //if the spheres were cast correctly
        {
            //calculate values for collision check
            float distance = Vector2.Distance(sphereB.Position, sphereA.Position);
            float impactDistance = sphereA.Radius + sphereB.Radius;
            if (distance <= impactDistance) //if we collided
            {
                sphereA.ApplyForce(-sphereA.Velocity); //set to 0 for now.
                return true;
            }
        }

        return false;
    }

    public static bool Sphere2Box(PhysicsObject inputA, PhysicsObject inputB)
    {
        return false;
    }

    public static bool Box2Plane(PhysicsObject inputA, PhysicsObject inputB)
    {
        return Plane2Box(inputB, inputA);
    }

    public static bool Box2Sphere(PhysicsObject inputA, PhysicsObject inputB)
    {
        return Sphere2Box(inputB, inputA);
    }

    public static bool Box2Box(PhysicsObject inputA, PhysicsObject inputB)
    {
        return false;
    }
}
