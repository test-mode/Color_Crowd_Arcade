using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Settings
    [HideInInspector] public bool onSafeZone = true;
    [HideInInspector] public bool playerDied = false;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public Image coneImage;

    //Connections
    [Header("Connections")]
    public GameObject door;
    public GameObject gameManager;
    public DynamicJoystick joystick;
    public DOTweenAnimation doorTween;
    public LevelData levelData;
    public GameObject childMesh;
    public GameObject joystickHandle;

    Rigidbody rb;
    Animator animator;

    public GameObject gun1;
    public GameObject gun2;

    //Variables
    [Header("Variables")]
    public float playerSpeed;
    [HideInInspector] public bool stopMoving;
    bool tweenActive = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        coneImage = GetComponentInChildren<Image>();

        gun1.SetActive(true);
    }

    public void FixedUpdate()
    {
        if (stopMoving)
        {
            currentTarget = null;
            direction = Vector3.zero;
            rb.velocity = Vector3.zero;
            joystick.ResetAxes();
        }
        else
        {
            joystick.enabled = true;
            currentTarget = GetComponentInChildren<BulletSpawner>().currentTarget;
            direction = (Vector3.forward * joystick.Vertical) + (Vector3.right * joystick.Horizontal);
            rb.velocity = playerSpeed * Time.fixedDeltaTime * direction;
        }


        if (gameManager.GetComponent<GameManager>().gameOver)
        {
            rb.velocity = Vector3.zero;
            SetIdleAnimation();
        }
        else
        {
            if (direction != Vector3.zero)
            {
                CrowdSetRunAnimation();
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                transform.LookAt(transform.position + direction);

                if (currentTarget != null && GetComponentInChildren<BulletSpawner>().fireBullets)
                {
                    Vector3 currentTargetLookPosition = currentTarget.transform.position;
                    currentTargetLookPosition.y = transform.position.y;
                    childMesh.transform.LookAt(currentTargetLookPosition);
                }
                else
                {
                    childMesh.transform.rotation = transform.rotation;
                }

                if (direction.magnitude < .5f)
                {
                    SetWalkAnimation();
                }
                else
                {
                    SetRunAnimation();
                }
            }
            else
            {
                SetIdleAnimation();
                CrowdSetIdleAnimation();
            }
        }



        if (Vector3.Distance(gameObject.transform.position, door.transform.position) < 5f && !tweenActive)
        {
            if (doorTween.isActive)
            {
                doorTween.DORestart();
            }
            doorTween.DOPlay();
            tweenActive = true;
        }
        else if (Vector3.Distance(gameObject.transform.position, door.transform.position) > 5f)
        {
            doorTween.DOPlayBackwards();
            tweenActive = false;
        }
    }

    private void CrowdSetIdleAnimation()
    {
        var friends = GetComponent<FriendsFormShield>().friends;
        var friendsAmount = GetComponent<FriendsFormShield>().friends.Count;
        for (int i = 0; i < friendsAmount; i++)
        {
            friends[i].GetComponent<FriendManager>().SetIdleAnimation();
        }
    }

    private void CrowdSetRunAnimation()
    {
        var friends = GetComponent<FriendsFormShield>().friends;
        var friendsAmount = GetComponent<FriendsFormShield>().friends.Count;
        for (int i = 0; i < friendsAmount; i++)
        {
            friends[i].GetComponent<FriendManager>().SetRunAnimation();
        }
    }

    private void CrowdSetWalkAnimation()
    {
        var friends = GetComponent<FriendsFormShield>().friends;
        var friendsAmount = GetComponent<FriendsFormShield>().friends.Count;
        for (int i = 0; i < friendsAmount; i++)
        {
            friends[i].GetComponent<FriendManager>().SetWalkAnimation();
        }
    }

    public void SetIdleAnimation()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }
    public void SetWalkAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
    }
    public void SetRunAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            onSafeZone = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            onSafeZone = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            onSafeZone = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Money"))
        //{
        //    gameManager.GetComponent<GameManager>().playerTotalMoney += levelData.enemyPrice;
        //    Destroy(collision.gameObject);
        //}

        //if (collision.transform.parent.CompareTag("XpDiamond"))
        //{
        //    Debug.Log("collision");
        //    gameManager.GetComponent<XpManager>().playerTotalXp += 10;
        //    Destroy(collision.gameObject);
        //}

        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDied = true;
        }

        if (collision.gameObject.CompareTag("Enemy Bound"))
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider>());
        }
    }
}