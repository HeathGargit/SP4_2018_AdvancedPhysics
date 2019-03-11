using UnityEngine;
using System.Collections;

public abstract class PhysicsObject
{
    protected PhysicsObject(ShapeType shapeID) { m_ShapeId = shapeID; }

    protected enum ShapeType { PLANE, SPHERE, BOX };
    protected ShapeType m_ShapeId;

    public abstract void FixedUpdate(Vector2 gravity, float timestep);
    public abstract void Debug();
    public abstract void MakeGizmo();
    public abstract void ResetPosition();
}
