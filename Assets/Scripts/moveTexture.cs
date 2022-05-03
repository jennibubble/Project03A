using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTexture : MonoBehaviour
{
    public float scrollSpeedx = 2f;
    Renderer rend;


    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        float scaleX = Mathf.Cos(Time.time) * 1f + 1;
        float scaleY = Mathf.Cos(Time.time) * 2f + 1;
        rend.material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));

    }
}
