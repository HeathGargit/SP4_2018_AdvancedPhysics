using UnityEngine;

public class SimController : MonoBehaviour
{
    //the physics sim
    private PhysicsScene m_PhysicsScene;

    //deltatime calcs
    private float m_TimeThisFrame, m_TimeLastFrame;

    //helpers for adding force to spheres
    BilliardBall m_TrackedSphere;

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
        BilliardBall ball1 = new BilliardBall(new Vector2(-4.0f, 0.0f), new Vector2(0,0), 0.0f, 1.2f, 0.25f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(ball1);
        BilliardBall ball2 = new BilliardBall(new Vector2(4.45f, 0.45f), new Vector2(0, 0), 0.0f, 1.0f, 0.25f, new Vector4(31.0f/255f, 116.0f/255f, 255.0f/255f, 1.0f));
        m_PhysicsScene.AddActor(ball2);
        BilliardBall ball3 = new BilliardBall(new Vector2(4.45f, -0.45f), new Vector2(0, 0), 0.0f, 1.0f, 0.25f, new Vector4(247/255f, 39/255f, 39/255f, 1.0f));
        m_PhysicsScene.AddActor(ball3);
        BilliardBall ball4 = new BilliardBall(new Vector2(4.9f, 0.0f), new Vector2(0, 0), 0.0f, 1.0f, 0.25f, new Vector4(1f, 1f, 0f, 1.0f));
        m_PhysicsScene.AddActor(ball4);
        BilliardBall ball5 = new BilliardBall(new Vector2(4.9f, -0.9f), new Vector2(0, 0), 0.0f, 1.0f, 0.25f, new Vector4(0f, 1f, 0f, 1.0f));
        m_PhysicsScene.AddActor(ball5);
        BilliardBall ball6 = new BilliardBall(new Vector2(4.9f, 0.9f), new Vector2(0, 0), 0.0f, 1.0f, 0.25f, new Vector4(170.0f / 255f, 0f, 1f, 1.0f));
        m_PhysicsScene.AddActor(ball6);
        BilliardBall ball0 = new BilliardBall(new Vector2(4.0f, 0), new Vector2(0,0), 0.0f, 1.0f, 0.25f, new Vector4(0, 0, 0, 1));
        m_PhysicsScene.AddActor(ball0);
        //HPPlane plane1 = new HPPlane(new Vector2(0.05f,1), 4, new Vector4(255, 255, 255, 1));
        //m_PhysicsScene.AddActor(plane1);
        HPPlane topbound = new HPPlane(new Vector2(0, -1.0f), 4.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(topbound);
        HPPlane rightbound = new HPPlane(new Vector2(-1.0f, 0), 8.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(rightbound);
        HPPlane bottombound = new HPPlane(new Vector2(0, 1.0f), 4.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(bottombound);
        HPPlane leftbound = new HPPlane(new Vector2(1.0f, 0), 8.5f, new Vector4(255, 255, 255, 1));
        m_PhysicsScene.AddActor(leftbound);

        //balltracking
        m_TrackedSphere = null;
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

        //add tracking for adding new forces
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("Mouse clicked at: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
            PhysicsObject clickedObject = m_PhysicsScene.GetPOAtPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(clickedObject != null)
            {
                //Debug.Log("This: " + clickedObject);
                m_TrackedSphere = (BilliardBall)clickedObject;
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(m_TrackedSphere != null)
            {
                m_TrackedSphere.ApplyForce(m_TrackedSphere.Position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            m_TrackedSphere = null;
        }

        //yes, im being cute here:
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
