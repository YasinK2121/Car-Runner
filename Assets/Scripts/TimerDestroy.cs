using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    public float Timer;
    private float a;
    // Update is called once per frame
    void Update()
    {
        a += Time.deltaTime;

        if (a > Timer)
        {
            Destroy(this.gameObject);
        }
    }
}
