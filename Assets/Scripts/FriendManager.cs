using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using static Dreamteck.Splines.SplineThreading.ThreadDef;
using static TheraBytes.BetterUi.LocationAnimations;
using static UnityEngine.GraphicsBuffer;

public class FriendManager : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;

    //Connections

    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject gameManager;
    [HideInInspector] public GameObject positionPrefab;
    [HideInInspector] public CrystalManager crystalManager;
    [HideInInspector] public Vector3 mineDestination;
    public GameObject workMine;
    public GameObject blood;
    public GameObject shovel;

    Animator animator;


    //Settings
    public float joinPlayerDuration = 4f;
    public float joinMineDuration = 2f;
    float timeElapsedToJoin = 0f;
    float timeElapsedToJoinMine = 0f;

    public float workDurationSeconds;
    float timeElapsedAtWork;

    //In Script Variables
    Vector3 offset = Vector3.zero;

    [HideInInspector] public bool joinedPlayer = false;
    [HideInInspector] public bool joiningPlayer = false;
    [HideInInspector] public bool goToWork = false;
    [HideInInspector] public bool atWork = false;

    void Awake()
    {
        InitConnections();
    }

    void InitConnections()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
    }

    void FixedUpdate()
    {
        if (goToWork)
        {
            if (!atWork)
            {
                GoToMine();
            }
            else
            {
                DigTheMine();
            }
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        else
        {
            transform.rotation = player.transform.rotation;

            if (joinedPlayer)
            {
                FollowPlayer();
            }
            else
            {
                GetCloserToPlayer();
            }
        }
    }

    private void DigTheMine()
    {
        transform.LookAt(workMine.transform.position, Vector3.up);
        transform.position = mineDestination;
        SetIdleAnimation();

        timeElapsedAtWork += Time.deltaTime;

        if (timeElapsedAtWork > workDurationSeconds)
        {
            transform.tag = "Untagged";
            crystalManager.moneyWorkers.Remove(gameObject);
            crystalManager.xpWorkers.Remove(gameObject);
            crystalManager.extraMoneyWorkers.Remove(gameObject);
            crystalManager.extraXpWorkers.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void GoToMine()
    {
        if (timeElapsedToJoinMine < joinMineDuration)
        {
            transform.position = Vector3.Lerp(transform.position, mineDestination, timeElapsedToJoinMine / joinMineDuration);
            timeElapsedToJoinMine += Time.deltaTime;
            SetRunAnimation();
        }
        else
        {
            timeElapsedToJoinMine = 0f;
            atWork = true;
            shovel.SetActive(true);
            SetMineAnimation();
        }


        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
    }

    void FollowPlayer()
    {
        transform.position = player.transform.position - offset;
    }


    public void GetCloserToPlayer()
    {
        if (!joiningPlayer && positionPrefab != null)
        {
            if (timeElapsedToJoin < joinPlayerDuration)
            {
                transform.position = Vector3.Lerp(transform.position, positionPrefab.transform.position, timeElapsedToJoin / joinPlayerDuration);
                timeElapsedToJoin += Time.deltaTime;
            }
            else
            {
                joiningPlayer = true;
                timeElapsedToJoin = 0f;
            }
        }
        else
        {
            joinedPlayer = true;
            offset = player.transform.position - transform.position;
            Destroy(positionPrefab);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Friend"))
        {
            GameObject bloodStainRed = Instantiate(blood, transform.position + new Vector3(0, .01f, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
            bloodStainRed.GetComponent<SpriteRenderer>().color = Color.red;
            GameObject bloodStainBlue = Instantiate(blood, transform.position + new Vector3(0, .01f, 1), Quaternion.Euler(new Vector3(90, 0, 0)));

            gameObject.tag = "Untagged";
            other.gameObject.tag = "Untagged";
            gameManager.GetComponent<TotalEnemies>().RecordEnemies(other.gameObject);
            player.GetComponent<FriendsFormShield>().FormShield(gameObject);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        navMeshAgent.enabled = false;
    }

    public void SetWalkAnimation()
    {
        animator.SetBool("Run", false);
    }
    public void SetRunAnimation()
    {
        animator.SetBool("Run", true);
    }
    public void SetIdleAnimation()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Idle", true);
    }
    public void SetMineAnimation()
    {
        animator.SetBool("Mine", true);
    }
}
