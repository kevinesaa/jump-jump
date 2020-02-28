using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameController gameController;

    public Animator fireAnimator;
    public Transform firePoint;
    public float fireDistance = 1.5f;
    public float fireForce = 10;
    public float incrementByFire = 0.5f;
    public float minAcctionDistance = 0.001f;
    public float coyoteDuration = 0.05f;

    public AudioClip shootClip;
    public AudioClip emptyClip;
    public AudioClip failShootClip;
    public AudioClip gameOver;


    public RaycastHit2D FloorCheck { get; private set; }
    public bool PlayerIsAlive { get;  set; }
    public bool FirstShoot { get; set; }
    public bool HasBullet { get; set; }
    public int FireSuccessfulCount { get; set; }
    public float FireforceAccumulator { get; set; }

    private Vector3 initialPosition;
    private Rigidbody2D mRigidbody2D;
    private PlayerInput playerInput;
    private float coyoteTime;
    private AudioSource audioSource;

#if UNITY_EDITOR
    RaycastHit2D editorGroundCheckHit;
#endif

    private void Awake()
    {
        initialPosition = transform.position;
        PlayerReset();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
    }

    

    private void Update()
    {
        FloorCheck = Physics2D.Raycast(firePoint.position, Vector2.down, fireDistance);
#if UNITY_EDITOR
        editorGroundCheckHit = FloorCheck;
#endif

        if (FirstShoot)
        {
            Vector2 currentVelocity = mRigidbody2D.velocity;
            if (currentVelocity.y < 0)
            {
                float distance = Vector3.Distance(initialPosition, transform.position);
                gameController.SetDistanceCount(distance);
            }
        }
    }

    private void FixedUpdate()
    {
        if (PlayerIsAlive)
        {
            Vector2 currentVelocity = mRigidbody2D.velocity;
            bool isFalling = currentVelocity.y < 0;
            if (playerInput.FireButtonPressed && (isFalling || !FirstShoot))
            {
                if (HasBullet) 
                    Shoot();
                else
                    EmptyShoot();
            }


        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerIsAlive) 
        {
            coyoteTime = Time.time + coyoteDuration;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 currentVelocity = mRigidbody2D.velocity;
        bool isFalling = currentVelocity.y <= 0;
        if (PlayerIsAlive && FirstShoot && isFalling)
        {
            PlayerIsAlive = Time.time <= coyoteTime;

            if (!PlayerIsAlive)
            {
                audioSource.clip = gameOver;
                audioSource.Play();
                gameController.GameOver();
            }

        }
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color color = editorGroundCheckHit ? Color.green : Color.red;
        Debug.DrawRay(firePoint.position, Vector2.down * fireDistance, color);
    }
#endif

    public void PlayerReset()
    {
        transform.position = initialPosition;
        FirstShoot = false;
        HasBullet = true;
        FireSuccessfulCount = 0;
        FireforceAccumulator = 0;
        PlayerIsAlive = true;
    }

    private void Shoot()
    {
        audioSource.clip = shootClip;
        audioSource.Play();
        fireAnimator.SetTrigger("shoot");
        if (FloorCheck)
        {
            Vector2 currentVelocity = mRigidbody2D.velocity;
            currentVelocity.y = 0;
            mRigidbody2D.velocity = currentVelocity;

            float distance = float.IsNaN(FloorCheck.distance) ||
                                FloorCheck.distance < minAcctionDistance ? 2*minAcctionDistance : FloorCheck.distance;

            float distanceMultify = Mathf.Abs(fireDistance / distance);
            distanceMultify = (distanceMultify > fireDistance) ? fireDistance : distanceMultify;
            Vector2 force = (distanceMultify + FireforceAccumulator + fireForce) * Vector2.up;
            mRigidbody2D.AddForce(force, ForceMode2D.Impulse);
            FireSuccessfulCount++;
            FireforceAccumulator += incrementByFire;
            gameController.ShootCount(FireSuccessfulCount);
            if (!FirstShoot)
            {
                FirstShoot = true;
            }
        }
        else
        {
            audioSource.clip = failShootClip;
            audioSource.Play();
            if (FirstShoot)
            {
                HasBullet = false;
            }
        }
        
    }

    private void EmptyShoot()
    {
        audioSource.clip = emptyClip;
        audioSource.Play();
    }

}
