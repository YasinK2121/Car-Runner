using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public GameObject hitParticle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Camera.main.transform.GetComponent<Animator>().SetBool("Damage", true);
            player.timer = true;
            Instantiate(hitParticle, new Vector3(transform.position.x, 0, transform.position.z), hitParticle.transform.rotation);
        }
    }
}
