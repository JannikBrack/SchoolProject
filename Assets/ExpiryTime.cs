using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpiryTime : MonoBehaviour
{
    private float e_Time;

    private void Awake()
    {
        e_Time = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if  (e_Time > 0)
        {
            e_Time -= Time.deltaTime;
        }
        else Destroy(gameObject);
    }
}
