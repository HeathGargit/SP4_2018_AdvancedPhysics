using UnityEngine;
using System.Collections;

public class BilliardBall : HPSphere
{
    public BilliardBall(Vector2 position, Vector2 velocity, float rotation, float mass, float radius, Vector4 colour) : base(position, velocity, rotation, mass, radius, colour)
    {

    }

    public bool CheckCollision(Vector2 point)
    {
        if (point != null)
        {
            float distance = Vector2.Distance(point, Position);
            float impactDistance = Radius;
            if (distance <= impactDistance)
            {
                return true;
            }
        }

        return false;
    }
}
