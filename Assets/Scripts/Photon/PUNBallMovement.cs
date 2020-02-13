using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class PUNBallMovement : MonoBehaviour
{
    public float MaxSpeed = 15.0f;
    public float torqueForce = 1.0f;

    public float ImpactGivingCoef = 1.8f;
    public float ImpactRecievingCoef = 1.2f;
    public float MinimalImpactForce = 2.0f;

    public bool controllable = true;

    private PhotonView photonView;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
    private CollideStates amplify = CollideStates.Null;
    private CameraPlayer cameraPlayer;

    private float impactPower;
    private float MovementSpeed;

    public List<GameObject> impactPrefab;

    public AudioSource myAudioSource;

    public AudioClip hitMarbl;
    public AudioClip hitWoodSurface;
    public AudioClip hitGround;
    public AudioClip shootSound;
    public AudioClip hueCaptureSound;

    public GameObject fx_hitWoodSurface;
    public List<Vector3> v3Record;
    public Vector3 firstvector;
    public bool startRecord = false;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();

        impactPower = PhotonNetwork.CurrentRoom.GetImpactPower();
        MovementSpeed = PhotonNetwork.CurrentRoom.GetLaunchPower();

        myAudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        cameraPlayer = GameModeManager.Instance.localPlayerObj.GetComponent<CameraPlayer>();
    }
    private void Update()
    {
        if (!photonView.IsMine || !controllable)
        {
            return;
        }

        Ray ray = new Ray(transform.position, transform.position - Vector3.down * 0.5f);
        if (Physics.Raycast(ray, 0.5f, 10))
        {

            if (gameObject.layer == 12)
            {
                gameObject.layer = 13;
            }
        }
        else
        {
            gameObject.layer = 12;
        }
        if (startRecord == true)
        {
            v3Record.Add(transform.position-firstvector);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 1; i < v3Record.Count;i++)
                {
                    Debug.DrawLine(v3Record[i - 1], v3Record[i], Color.red, 5.0f);
                    
                    Debug.DrawLine(getPos(i - 1), getPos(i), Color.blue, 5.0f);
                }
                startRecord = false;
            }
        }
    }

    private Vector3 getPos(int _i)
    {
        float z = (_i / (float)v3Record.Count) *8.0f * (18.4f / Mathf.Abs(Physics2D.gravity.y)) + ((_i / (float)v3Record.Count) * 1.2f);//(((1.0f / (7.0f * 1.9f)) - 1.0f / 19.62f)) / (((45.0f - 25.0f) / 10.0f) * 0.923f + 1));
        float y = (((22.0f / 8.0f)) / 360.0f) * (-Mathf.Abs(Physics2D.gravity.y) * z * z + 18.4f * 8.0f * z) / 2.0f; //22 --> 3.8f --> 2.3f
        return new Vector3(0, y, z - ((_i / (float)v3Record.Count)*1.2f)); //0.015f-8 //0.035f-6 //0.025 - 7 //0.055 -5 //0.08 - 4 //0.115 - 3
    }

    public void MoveBall(Vector3 direction, float angle, float dragForce)
    {
        direction = new Vector3(direction.x * Mathf.Cos(Mathf.Deg2Rad * angle), ((45 - angle) / 45.0f + MovementSpeed * 2.0f) * Mathf.Sin(Mathf.Deg2Rad * angle) / (MovementSpeed * 2.0f), direction.z * Mathf.Cos(Mathf.Deg2Rad * angle));
        Vector3 _impulse = direction * (dragForce * rigidbody.mass * MovementSpeed * 2.0f);

        this.GetComponent<Rigidbody>().AddForceAtPosition(_impulse, transform.position, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(direction, Vector3.up) * -torqueForce, ForceMode.Force);

        if (shootSound != null)
        {
            myAudioSource.PlayOneShot(shootSound);
        }

        photonView.RPC("OnPlayerShoot", RpcTarget.Others);
    }

    public void MoveBall()
    {
        startRecord = true;
        firstvector = transform.position;
        Vector3 direction = Vector3.forward;
        float angle = 22.0f;
        float dragForce = 8.0f;
        direction = new Vector3(direction.x * Mathf.Cos(Mathf.Deg2Rad * angle), ((45 - angle) / 45.0f + MovementSpeed * 2.0f) * Mathf.Sin(Mathf.Deg2Rad * angle) / (MovementSpeed * 2.0f), direction.z * Mathf.Cos(Mathf.Deg2Rad * angle));
        Vector3 _impulse = direction * (dragForce * rigidbody.mass * MovementSpeed * 2.0f);

        this.GetComponent<Rigidbody>().AddForceAtPosition(_impulse, transform.position, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(direction, Vector3.up) * -torqueForce, ForceMode.Force);

        if (shootSound != null)
        {
            myAudioSource.PlayOneShot(shootSound);
        }

        photonView.RPC("OnPlayerShoot", RpcTarget.Others);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.sqrMagnitude > 16)
        {
            if (collision.gameObject.tag == "Ball")
            {
                if (hitMarbl != null)
                {
                    myAudioSource.PlayOneShot(hitMarbl);
                }

                GameObject impact = Instantiate(impactPrefab[Random.Range(0, impactPrefab.Count)], collision.contacts[0].point, Quaternion.identity);
                float size = collision.relativeVelocity.sqrMagnitude / 200;
                size = Mathf.Clamp(size, 0, 3);
                impact.transform.localScale = new Vector3(size, size, size);
                Destroy(impact, 2);

                float screenShakeDistance = Vector3.Distance(Camera.main.transform.position, this.gameObject.transform.position);
                float screenShakePower = collision.relativeVelocity.sqrMagnitude / 40 - screenShakeDistance / 30;

                if (screenShakePower > 0)
                {
                    cameraPlayer.InitShakeScreen(screenShakePower, 0.10f);
                }
            }

            if (collision.gameObject.layer == 14 && collision.collider.CompareTag("Wood")) //Layer Obstacle
            {
                if (hitWoodSurface != null)
                {
                    myAudioSource.PlayOneShot(hitWoodSurface);
                }

                GameObject _fxWallHitWood = Instantiate(fx_hitWoodSurface, collision.contacts[0].point, Random.rotation);
                _fxWallHitWood.transform.LookAt(collision.gameObject.transform);
                Destroy(_fxWallHitWood, 2);

                float screenShakeDistance = Vector3.Distance(Camera.main.transform.position, this.gameObject.transform.position);
                float screenShakePower = collision.relativeVelocity.sqrMagnitude / 80 - screenShakeDistance / 30;

                if (screenShakePower > 0)
                {
                    cameraPlayer.InitShakeScreen(screenShakePower, 0.10f);
                }
            }
        }

        if (collision.gameObject.tag == "Ball")
        {
            BallSettings ballSettingGiver = GetComponent<BallSettings>();
            BallSettings ballSettingReciever = collision.gameObject.GetComponent<BallSettings>();

            if (photonView.IsMine)
            {
                if (ballSettingReciever.currentSpeed < ballSettingGiver.currentSpeed)
                {
                    if (PhotonNetwork.CurrentRoom.GetHue() && ballSettingReciever.myteam != ballSettingGiver.myteam)
                    {
                        ballSettingReciever.ChangeTeam(ballSettingGiver.myteam);
                        PhotonNetwork.LocalPlayer.AddPlayerScore(1);
                    }
                    amplify = CollideStates.Giver;
                }
                else
                {
                    amplify = CollideStates.Reciever;
                }
            }

            if (ballSettingReciever.currentSpeed < ballSettingGiver.currentSpeed)
            {
                if (PhotonNetwork.CurrentRoom.GetHue() && ballSettingReciever.myteam != ballSettingGiver.myteam)
                {
                    if (hueCaptureSound != null)
                    {
                        myAudioSource.PlayOneShot(hueCaptureSound);
                    }
                }
            }

            QuickScoreboard.Instance.Refresh();
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PUNBallMovement>() != null && photonView.IsMine)
        {
            //Mathf.Abs(Mathf.Abs(collision.gameObject.transform.position.y) - Mathf.Abs(transform.position.y)) < 0.3f
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > MinimalImpactForce * MinimalImpactForce && rigidbody.velocity.sqrMagnitude > MinimalImpactForce * MinimalImpactForce)
            {

                if (amplify == CollideStates.Giver)
                {
                    //Debug.Log("Giving Collider " + gameObject.name);
                    float sqrSpeed = Mathf.Clamp((rigidbody.velocity * ImpactGivingCoef * impactPower * 1.3f - Vector3.up * rigidbody.velocity.y * (ImpactGivingCoef - 1)).sqrMagnitude, 0, MaxSpeed * MaxSpeed);
                    rigidbody.velocity = (rigidbody.velocity * ImpactGivingCoef * impactPower * 1.3f - Vector3.up * rigidbody.velocity.y * (ImpactGivingCoef - 1)).normalized * Mathf.Sqrt(sqrSpeed); ;
                }
                else
                {
                    //Debug.Log("Recieving Collider " + gameObject.name);
                    float sqrSpeed = Mathf.Clamp((rigidbody.velocity * ImpactRecievingCoef * impactPower * 1.3f - Vector3.up * rigidbody.velocity.y * (ImpactRecievingCoef - 1)).sqrMagnitude, 0, MaxSpeed * MaxSpeed);

                    rigidbody.velocity = (rigidbody.velocity * ImpactRecievingCoef * impactPower * 1.3f - Vector3.up * rigidbody.velocity.y * (ImpactRecievingCoef - 1)).normalized * Mathf.Sqrt(sqrSpeed);
                }
            }
        }
    }




    //public void MoveBall(Vector3 direction, float dragForce)
    //{
    //    direction = new Vector3(direction.x, 0, direction.z);
    //    Vector3 _impulse = direction * (dragForce * MovementSpeed);

    //    this.GetComponent<Rigidbody>().AddForce(_impulse, ForceMode.Impulse);
    //}

    [PunRPC]
    public void OnPlayerShoot()
    {
        if (shootSound != null)
        {
            myAudioSource.PlayOneShot(shootSound);
        }
    }
}




public enum CollideStates
{
    Null,
    Reciever,
    Giver
}
