using DG.Tweening;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject currentTarget = null;
    Rigidbody rb;
    bool tweenInProgress = false;
    public float bulletSpeed;
    float timeLimit = 4f;
    float timer = 0f;

    float tweenTime = .5f;

    [HideInInspector] public Vector3 originalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        tweenInProgress = false;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        currentTarget = player.GetComponentInChildren<BulletSpawner>().currentTarget;

        if (currentTarget != null && currentTarget.CompareTag("Enemy") && !tweenInProgress)
        {
            tweenInProgress = true;
            transform.DOMove(currentTarget.transform.position, tweenTime);
            currentTarget = null;
            player.GetComponentInChildren<BulletSpawner>().currentTarget = null;
            player.GetComponentInChildren<PlayerController>().currentTarget = null;
        }
        else
        {
            rb.velocity = transform.forward.normalized * bulletSpeed;
        }


        timer += Time.deltaTime;

        if (timer > timeLimit)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || !other.gameObject.CompareTag("Friend"))
        {
            gameObject.SetActive(false);
        }
    }

}
