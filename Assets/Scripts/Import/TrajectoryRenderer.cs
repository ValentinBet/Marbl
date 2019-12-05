using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class TrajectoryRenderer : MonoBehaviour
{
    public PUNMouseControl mouseControl;
    public float force = 0.0f;
    public float multiplier = 8.0f;

    private LineRenderer lr;
    private float velocity = 8.86f;
    [SerializeField] private float angle = 40.06f;
    [SerializeField] private int resolution = 20;
    private float g;
    private float maxDist = 0.0f;
    private Vector3 direction;
    private float radianAngle;
    private PhotonView photonView;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);

        mouseControl = this.GetComponent<PUNMouseControl>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        mouseControl.OnBallClicked += OnClickOnBall;
    }

    private void OnDisable()
    {
        mouseControl.OnBallClicked -= OnClickOnBall;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        force = mouseControl.dragForce;
        angle = mouseControl.orientation;

            if (force != 0)
            {
                RenderArc();
            }
    }

    private void RenderArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
        GradientColorKey[] colorKeys = lr.colorGradient.colorKeys;
        colorKeys[1].color = Color.red * angle / 32.0f;
        for (int i = 0; i < colorKeys.Length; i++)
        {
            Debug.Log(colorKeys[i].color);
        }
        lr.colorGradient.colorKeys = colorKeys;
    }

    private void OnClickOnBall(GameObject ball)
    {
        transform.position = ball.transform.position;
    }

    private Vector3[] CalculateArcArray()
    {
        Vector3[] Vec3ArcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        maxDist = force * multiplier / g;
        velocity = Mathf.Sqrt(maxDist * g / Mathf.Sin(radianAngle * 2));

        direction = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        for (int i = 0; i <= resolution; i++)
        {
            float percent = (float)i / (float)resolution;

            Vec3ArcArray[i] = CalculateArcPoint(percent, maxDist);
        }
        return Vec3ArcArray;
    }

    private Vector3 CalculateArcPoint(float percent, float maxDist)
    {
        float z = percent * maxDist;

        float y = ((angle / force) / 360.0f) * (-g * z * z + multiplier * force * z);
        if (y < 0)
        {
            y = 0;
        }

        return new Vector3(direction.x * z, y, direction.z * z);
    }

    public void SetVelocity(float newVel)
    {
        velocity = newVel;
    }

    public void SetNewTarget(GameObject go)
    {
        transform.position = go.transform.position;
    }
}
