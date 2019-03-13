using UnityEngine;
using System.Collections;

public class HPPlane : PhysicsObject
{
    //member variables
    protected Vector2 m_Normal;
    protected float m_Distance;
    protected Vector4 m_Colour;
    protected GameObject m_GameObject;

    //accessors
    public Vector2 Normal { get => m_Normal; }
    public float Distance { get => m_Distance; } 
    public Vector4 Colour { get => m_Colour; set => m_GameObject.GetComponent<Renderer>().material.color = new Color(value.x, value.y, value.z, value.w); }

    //constructor
    public HPPlane(Vector2 normal, float distance, Vector4 colour) : base(0)
    {
        //set variables
        m_Normal = normal;
        m_Distance = distance;
        m_Colour = colour;
        Vector2 position = -(m_Normal.normalized * m_Distance);
        float rawRotation = Vector2.SignedAngle(Vector2.up, m_Normal);

        //create gameobject
        m_GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_GameObject.transform.localRotation = Quaternion.Euler(0, 0, rawRotation);
        m_GameObject.transform.position = position;
        m_GameObject.transform.localScale = new Vector3(30, 0.05f, 1);
        m_GameObject.GetComponent<Renderer>().material.color = new Color(m_Colour.x, m_Colour.y, m_Colour.z, m_Colour.w);
    }

    //inherited member override functions
    public override void MakeGizmo()
    {
        
    }

    public override void ResetPosition()
    {
        
    }

    public override void FixedUpdate(Vector2 gravity, float timestep)
    {
        
    }

    public override void Debug()
    {
        
    }

    //member functions
    public void ResolveCollision(HPRigidBody actor2)
    {
        Vector2 relativeVelocity = actor2.Velocity;

        float elasticity = 1.0f;
        float j = Vector2.Dot(-(1 + elasticity) * (relativeVelocity), m_Normal) / (1 / actor2.Mass);

        Vector2 force = m_Normal * j;

        actor2.ApplyForce(force);
    }

}
