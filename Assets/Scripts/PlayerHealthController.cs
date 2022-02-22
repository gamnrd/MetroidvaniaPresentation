using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    [HideInInspector]
    public int currentHealth;
    public int maxHealth;
    public float invincibilityLength;
    private float invincCounter;
    public float flashLength;
    private float flashCounter;
    public SpriteRenderer[] playerSprites;

    private void Awake()
    {
        //if first run keep first player
        if (instance == null)
        {
            instance = this;
            //After respawn don't spawn new player
            DontDestroyOnLoad(gameObject);
        }
        //if player already exists don't spawn new player
        else
        {
            Destroy(gameObject);
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //If player is damaged, make the sprite blink to note damage
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;
            }
        }
        else if(invincCounter <= 0)
        {
            foreach (SpriteRenderer sr in playerSprites)
            {
                sr.enabled = true;
            }
            flashCounter = 0;
        }
        
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invincCounter <= 0)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //gameObject.SetActive(false);
                RespawnController.instance.Respawn();
            }
            else
            {
                invincCounter = invincibilityLength;
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }        
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
