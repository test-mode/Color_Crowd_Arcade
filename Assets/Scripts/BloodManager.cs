using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BloodManager : MonoBehaviour
{
    //Settings
    Tween tween;

    // Connections

    // State Variables
    public float timeToDisappear;
    public float duration;
    float timer;
    bool tweenStarted;

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {

    }
    void InitState()
    {
        tweenStarted = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        

        if (timer > timeToDisappear && !tweenStarted)
        {
            tweenStarted = true;
            tween = GetComponent<SpriteRenderer>().DOFade(0, duration).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        if (tweenStarted && tween.IsComplete())
        {
            Destroy(gameObject);
        }
    }
}

