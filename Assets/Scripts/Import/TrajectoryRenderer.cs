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
    [SerializeField] private Gradient heightValues;
    [SerializeField] private GameObject landingZone;
    [SerializeField] private float landingGrowthRate = 0.1f;
    private float g;
    private float maxDist = 0.0f;
    private Vector3 direction;
    private float radianAngle;
    private PhotonView photonView;

    private float storedOffset = 0.0f;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);

        mouseControl = this.GetComponent<PUNMouseControl>();
        photonView = GetComponent<PhotonView>();
        landingZone = Instantiate(landingZone);
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
        angle = mouseControl.elevation;
        if (force != 0)
        {
            RenderArc();
            UpdateCurveMaterial();
        } else
        if (angle == 0)
        {
            ResetLandingZone();
        }
    }

    private void RenderArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
        Vector3[]lrP = new Vector3[lr.positionCount];
        lr.GetPositions(lrP);
        lr.colorGradient = DefineGradient(lrP);
        if (angle != 0)
        {
            if (!PlaceLandingZone(lrP, 9))
                ResetLandingZone();
        }
        else if (landingZone.transform.position != Vector3.down * 100)
        {
            ResetLandingZone();
        }
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
        Debug.Log(direction);
        for (int i = 0; i <= resolution; i++)
        {
            float percent = (float)i / (float)resolution;

            Vec3ArcArray[i] = CalculateArcPoint(percent, maxDist);
        }
        
        return Vec3ArcArray;
    }

    private Vector3 CalculateArcPoint(float percent, float maxDist)
    {
        float z = percent * maxDist + percent * (-1.13f*force+10.2f);

        float y = (((angle / force) ) / 360.0f) * (-g * z * z + multiplier * force * z)/2.0f; //9.2f == base multiplier
        if (y < 0)
        {
            y = 0;
        }

        return new Vector3(direction.x * (z- percent * (-1.13f * force + 10.2f)), y, direction.z * (z- percent * (-1.13f * force + 10.2f)));
    }

    private Gradient DefineGradient(Vector3[] positions)
    {
        int delta = Mathf.FloorToInt(positions.Length / 8.0f);

        //DefineGradient
        Gradient grd = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[8];
        for (int i = 0; i < 8; i++)
        {
            colorKeys[i] = new GradientColorKey(heightValues.Evaluate(Mathf.Lerp(0.0f, 1.0f, lr.GetPosition(delta * i).y/6.0f)),i*0.125f);
        }
        grd.SetKeys(colorKeys, lr.colorGradient.alphaKeys);
        return grd;
    }

    private bool PlaceLandingZone(Vector3[] positions,int middleindex)
    {
        landingZone.SetActive(true);
        RaycastHit raycastHit = new RaycastHit();
        RaycastHit storedHit = raycastHit;
        bool lastFound = false;
        for (int i = middleindex; i < positions.Length; i++)
        {
            Vector3 CheckPos = positions[i] + transform.position;
            //Debug.DrawRay(position, Vector3.down);

            if (Physics.Raycast(CheckPos, Vector3.down, out raycastHit,0.4f))
            {
                storedHit = raycastHit;
                landingZone.transform.position = storedHit.point + Vector3.up * 0.1f;
                landingZone.transform.LookAt(storedHit.point + storedHit.normal);
                landingZone.transform.localScale = Vector3.one * maxDist * landingGrowthRate;
                return true;
            }
            else if (lastFound)
            {
                Debug.Log("FirstFound");
                landingZone.transform.position = storedHit.point + Vector3.up * 0.1f;
                landingZone.transform.LookAt(storedHit.point + storedHit.normal);
                landingZone.transform.localScale = Vector3.one * maxDist * landingGrowthRate;
                return true;
            }
        }
        return false;
    }

    public void ResetLandingZone()
    {
        landingZone.transform.position = Vector3.down * 100;
        landingZone.SetActive(false);
    }

    private void UpdateCurveMaterial()
    {
        storedOffset += Time.deltaTime;
        Mathf.Repeat(storedOffset,1.0f);
        lr.sharedMaterial.SetTextureOffset("_MainTex", Vector2.right * -storedOffset);
    }

    private void ResetCurveMaterial()
    {
        lr.sharedMaterial.SetTextureOffset("_MainTex", Vector2.zero);
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
