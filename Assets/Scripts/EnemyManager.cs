using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static Dreamteck.Splines.SplineThreading.ThreadDef;
using Color = UnityEngine.Color;

public class EnemyManager : MonoBehaviour
{
    //Settings
    bool hasBeenHit = false;

    //Connections
    [HideInInspector] public CrowdCapacityUpgrade crowdCapacityUpgrade;
    [HideInInspector] public GameObject gameManager;
    [HideInInspector] public GameObject player;
    public SkinnedMeshRenderer meshRenderer;
    public NavMeshAgent navMeshAgent;
    public LevelData levelData;
    public GameObject blood;
    Animator animator;

    //Variables
    [HideInInspector] public int friendsAmount;
    public float runSpeed;
    public float walkSpeed;
    public float runDistance;

    float timer = 0f;
    float timeLimit = 2f;

    void Start()
    {
        InitConnections();
        InitStates();
    }
    private void InitConnections()
    {
        animator = GetComponent<Animator>();
        player.GetComponentInChildren<BulletSpawner>().GetEnemies(gameObject);
    }
    private void InitStates()
    {
        transform.tag = "Enemy";
        meshRenderer.material.color = Color.red;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (player.GetComponent<PlayerController>().playerDied)
        {
            navMeshAgent.enabled = false;
            SetIdleAnimation();
        }
        else
        {
            navMeshAgent.enabled = true;
            if (!player.GetComponent<PlayerController>().onSafeZone && Vector3.Distance(gameObject.transform.position, player.transform.position) < runDistance)
            {
                RunToTarget();
                SetRunAnimation();
                
            }
            else
            {
                WalkAround();
                SetWalkAnimation();
            }
        }
    }

    private void WalkAround()
    {
        navMeshAgent.enabled = false;

        timer += Time.deltaTime;

        transform.Translate(Time.deltaTime * walkSpeed * Vector3.forward);
        //navMeshAgent.SetDestination(Vector3.forward);

        if (timer > timeLimit)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 randomRotation = new Vector3(0, angle, 0);
            transform.DORotate(randomRotation, 2f);

            timer = 0f;
        }
    }

    private void RunToTarget()
    {
        navMeshAgent.enabled = true;

        transform.LookAt(player.transform);
        navMeshAgent.SetDestination(player.transform.position);
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


    void OnTriggerEnter(Collider other)
    {
        int crowdCapacity = gameManager.GetComponent<Upgrades>().crowdCapacity;
        friendsAmount = player.GetComponent<FriendsFormShield>().friends.Count;

        if (other.CompareTag("Bullet") && !hasBeenHit && friendsAmount < crowdCapacity)
        {
            hasBeenHit = true;
            gameObject.tag = "Friend";

            other.gameObject.SetActive(false);
            meshRenderer.material = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
            player.GetComponent<FriendsFormShield>().FormShield(gameObject);
            gameManager.GetComponent<TotalEnemies>().RecordEnemies(gameObject);
            player.GetComponentInChildren<BulletSpawner>().GetEnemies(gameObject);

            GetComponent<FriendManager>().enabled = true;

            this.enabled = false;
        }
    }

}
