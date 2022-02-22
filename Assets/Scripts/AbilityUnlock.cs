using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockBomb;
    public GameObject pickupEffect;
    public string unlockMessage;
    public TMP_Text unlockText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Get the ability tracker script
            PlayerAbilityTracker player = other.GetComponentInParent<PlayerAbilityTracker>();


            //If the ability pickup is set for a certain ability, unlock that ability
            if(unlockDoubleJump)
            {
                player.canDoubleJump = true;
            }

            if (unlockDash)
            {
                player.canDash = true;
            }

            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }

            if (unlockBomb)
            {
                player.canDropBomb = true;
            }

            //Create particle effect
            Instantiate(pickupEffect, transform.position, transform.rotation);

            //Detach text from parent pickup, display text, destroy text, destory pickup
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;
            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);
            Destroy(unlockText.transform.parent.gameObject, 5f);
            Destroy(gameObject);
        }
    }
}
