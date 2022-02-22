using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        //if first run keep
        if (instance == null)
        {
            instance = this;
            //After respawn don't spawn new
            DontDestroyOnLoad(gameObject);
        }
        //if already exists don't spawn new
        else
        {
            Destroy(gameObject);
        }
    }

    public Slider healthSlider;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
