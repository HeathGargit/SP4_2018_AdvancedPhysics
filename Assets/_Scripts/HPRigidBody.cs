using UnityEngine;

public abstract class HPRigidBody : PhysicsObject
{
    //member variables
    protected Vector2 m_Position, m_Velocity;
    protected float m_Rotation, m_Mass;
    protected float m_LinearDrag, m_AngularDrag;

    //accessors
    public Vector2 Position { get => m_Position; }
    public Vector2 Velocity { get => m_Velocity; }
    public float Rotation { get => m_Rotation; }
    public float Mass { get => m_Mass; }

    //constructor
    public HPRigidBody(int shapeID, Vector2 position, Vector2 velocity, float rotation, float mass) : base((ShapeType)shapeID)
    {
        m_ShapeId = (ShapeType)shapeID;
        m_Position = position;
        m_Velocity = velocity;
        m_Rotation = rotation;
        m_Mass = mass;
    }

    //abstract functions
    public abstract bool CheckCollision(PhysicsObject other);

    //overrides
    public override void FixedUpdate(Vector2 gravity, float timestep)
    {
        // applies gravity
        ApplyForce(gravity * m_Mass * timestep);

        //updates the position based on velocity.
        m_Position += m_Velocity * timestep;
    }
    public override void Debug()
    {
        //put debug stuff here
    }

    //member functions
    public void ApplyForce(Vector2 force)
    {
        m_Velocity += force / m_Mass;
    }

    public void ApplyForceToActor(HPRigidBody actor2, Vector2 force)
    {
        actor2.ApplyForce(force);
        ApplyForce(-force);
    }

    public void ResolveCollision(HPRigidBody actor2)
    {
        Vector2 normal = (actor2.Position - m_Position);
        normal.Normalize();
        Vector2 relativeVelocity = actor2.Velocity - m_Velocity;

        float elasticity = 1.0f;
        float j = Vector2.Dot(-((1 + elasticity) * (relativeVelocity)), normal) / Vector2.Dot(normal, normal * ((1 / m_Mass) + (1 / actor2.Mass)));

        Vector2 force = normal * j;

        ApplyForceToActor(actor2, force);
    }
}
