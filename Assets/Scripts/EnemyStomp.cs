using System.Collections.Specialized;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Weakpoint")
    //    {
    //        Debug.Log("DIE");
    //        //collision.gameObject.GetComponent<Pumpkin>().Death();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weakpoint")
        {
            Debug.Log("DIE");
            collision.gameObject.GetComponentInParent<Enemy>().Death();
            GetComponentInParent<Player>().Jump();
        }
    }
}