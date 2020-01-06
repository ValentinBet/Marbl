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
    public AudioClip hitWood;
    public AudioClip hitGround;
    public AudioClip shootSound;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && collision.relativeVelocity.sqrMagnitude > 64)
        {
            GameObject impact = Instantiate(impactPrefab[Random.Range(0, impactPrefab.Count)], collision.contacts[0].point, Quaternion.identity);
            float size = collision.relativeVelocity.sqrMagnitude / 400;
            size = Mathf.Clamp(size, 0, 3);
            impact.transform.localScale = new Vector3(size, size, size);
            Destroy(impact, 2);

            float screenShakeDistance = Vector3.Distance(Camera.main.transform.position, this.gameObject.transform.position);
            float screenShakePower = Mathf.Clamp(collision.relativeVelocity.sqrMagnitude / 300 - screenShakeDistance / 30, 0, 20);

            if (screenShakePower > 0)
            {
                cameraPlayer.InitShakeScreen(screenShakePower, 0.10f);
            }
        }

        if (collision.gameObject.tag == "Ball" && photonView.IsMine)
        {
            BallSettings ballSettingGiver = GetComponent<BallSettings>();
            BallSettings ballSettingReciever = collision.gameObject.GetComponent<BallSettings>();

            if (ballSettingReciever.currentSpeed < ballSettingGiver.currentSpeed)
            {
                //print(ballSettingGiver.myteam + " - " + ballSettingGiver.currentSpeed + " --> " + ballSettingReciever.myteam + " - " + ballSettingReciever.currentSpeed);

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
}

public enum CollideStates
{
    Null,
    Reciever,
    Giver
}
