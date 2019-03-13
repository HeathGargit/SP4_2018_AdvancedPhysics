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
        HPSphere ball1 = new HPSphere(new Vector2(-4.0f, 0.3f), new Vector2(0,0), 0.0f, 1.0f, 0.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(ball1);
        HPSphere ball2 = new HPSphere(new Vector2(4.0f, 0), new Vector2(0,0), 0.0f, 1.2f, 0.5f, new Vector4(0, 0, 0, 1));
        m_PhysicsScene.AddActor(ball2);
        HPPlane plane1 = new HPPlane(new Vector2(0.05f,1), 4, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(plane1);
        HPPlane topbound = new HPPlane(new Vector2(0, -1.0f), 4.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(topbound);
        HPPlane rightbound = new HPPlane(new Vector2(-1.0f, 0), 8.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(rightbound);
        HPPlane bottombound = new HPPlane(new Vector2(0, 1.0f), 4.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(bottombound);
        HPPlane leftbound = new HPPlane(new Vector2(1.0f, 0), 8.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(leftbound);

        ball1.ApplyForce(new Vector2(1.0f, 0f));
        ball2.ApplyForce(new Vector2(-1.0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        //calc deltatime
        m_TimeThisFrame = Time.unscaledTime;
        float deltatime = m_TimeThisFrame - m_TimeLastFrame;
        m_TimeLastFrame = m_TimeThisFrame;

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
