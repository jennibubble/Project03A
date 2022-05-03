using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{

    public GameObject cam1;
    public GameObject cam2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TriggerKey"))
        {
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
       
    }
}
