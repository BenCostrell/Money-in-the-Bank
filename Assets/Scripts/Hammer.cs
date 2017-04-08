using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Player player;
    public float knockback;
    public float stunDuration;

    // Use this for initialization
    void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = col.gameObject;
        if (obj.tag == "Bank")
        {
            Bank bank = obj.GetComponent<Bank>();
            if (bank.owner != player)
            {
                obj.GetComponent<Bank>().GetSmashed();
            }
        }
        if (obj.tag == "Player")
        {
            Player playerHit = obj.GetComponent<Player>();
            if (player != playerHit)
            {
                Vector2 knockbackVector;
                if (player.transform.localScale.x == 1)
                {
                    knockbackVector = Vector2.right;
                }
                else
                {
                    knockbackVector = Vector2.left;
                }
                playerHit.GetKnockedBack(knockback * knockbackVector);
                StunTask getStunned = new StunTask(stunDuration, playerHit);
                Services.TaskManager.AddTask(getStunned);
            }
        }
    }
}
