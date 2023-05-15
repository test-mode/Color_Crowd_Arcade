using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConeManager : MonoBehaviour
{
    Image coneImage;
    public GameObject player;

    void Awake()
    {
        coneImage = GetComponentInChildren<Image>();
    }
    void Start()
    {
        
    }


    void Update()
    {
        coneImage.rectTransform.localRotation = Quaternion.Euler(0, 0, 270 + coneImage.fillAmount * 180);
    }

}
