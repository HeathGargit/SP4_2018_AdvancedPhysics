using UnityEngine;

public class HPSphere : HPRigidBody
{
    //member variables
    protected float m_Radius;
    protected Vector4 m_Colour;
    protected GameObject m_GameObject; //the gameobject that represents this sphere

    //accessor
    public float Radius { get => m_Radius; }

    //constructor
    public HPSphere(Vector2 position, Vector2 velocity, float rotation, float mass, float radius, Vector4 colour) : base( 1, position, velocity, rotation, mass)
    {
        m_Radius = radius;
        m_Colour = colour;

        m_GameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        m_GameObject.transform.position = m_Position;
        m_GameObject.transform.localScale = new Vector3(m_Radius * 2, m_Radius * 2, m_Radius * 2);
        m_GameObject.GetComponent<Renderer>().material.color = new Color(m_Colour.x, m_Colour.y, m_Colour.z, m_Colour.w);
    }

    public override void MakeGizmo()
    {
        m_GameObject.transform.position = m_Position;
    }

    public override bool CheckCollision(PhysicsObject other)
    {
        HPSphere otherSphere = (HPSphere)other;
        if(otherSphere != null)
        {
            float distance = Vector2.Distance(otherSphere.Position, Position);
            float impactDistance = otherSphere.Radius + Radius;
            if (distance <= impactDistance)
            {
                return true;
            }
        }

        return false;
    }

    public override void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
