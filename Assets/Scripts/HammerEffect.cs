using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEffect : MonoBehaviour
{
    public float Timer;
    private float Life, radius;
    public ParticleSystem part;
    public ParticleSystem.ShapeModule sh;

    void Start()
    {
        part = transform.GetComponent<ParticleSystem>();
        sh = part.shape;
    }

    void Update()
    {
        radius += Time.deltaTime * 10f;
        sh.radius = radius;
        Life += Time.deltaTime;

        if (Life > Timer)
        {
            Destroy(this.gameObject);
        }
    }
}
