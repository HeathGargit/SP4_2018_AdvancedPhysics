using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimController : MonoBehaviour
{
    //the physics sim
    private PhysicsScene m_PhysicsScene;

    //deltatime calcs
    private float m_TimeThisFrame, m_TimeLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        //disabling Unity's time scale so you know i'm not using that as a crutch.
        Time.timeScale = 0.0f;
        m_TimeThisFrame = Time.unscaledTime;

        //crete the scene
        m_PhysicsScene = new PhysicsScene();
        m_PhysicsScene.TimeStep = 0.01f;
        m_PhysicsScene.Gravity = new Vector2(0, 0);

        //add actors
        HPSphere ball1 = new HPSphere(new Vector2(-2.0f, 0f), new Vector2(0,0), 0.0f, 3.0f, 0.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(ball1);
        HPSphere ball2 = new HPSphere(new Vector2(2.0f, 0), new Vector2(0,0), 0.0f, 1.0f, 0.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(ball2);
        HPPlane plane1 = new HPPlane(new Vector2(0.2f,1), 4, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(plane1);

        ball1.ApplyForce(new Vector2(0, -1.0f));
        ball2.ApplyForce(new Vector2(0, -1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //calc deltatime
        m_TimeThisFrame = Time.unscaledTime;
        float deltatime = m_TimeThisFrame - m_TimeLastFrame;
        m_TimeLastFrame = m_TimeThisFrame;

        //Debug.Log(deltatime);

        //update the physics sim
        m_PhysicsScene.Update(deltatime);
        m_PhysicsScene.UpdateGizmos();

        //yes, im being cute here:
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
