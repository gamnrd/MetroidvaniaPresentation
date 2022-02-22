using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;
    public GameObject deathEffect;
    private Vector3 respawnPoint;
    public float waitToRespawn;
    private GameObject player;

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

    
    // Start is called before the first frame update
    void Start()
    {
        //Create refrence to player
        player = PlayerHealthController.instance.gameObject;
        //Set inital respawn to players initial spawn
        respawnPoint = player.transform.position;
    }


    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        //Set player inactive, if there is a death effect spawn it then wait for waitToRespawn
        player.SetActive(false);
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        }
        yield return new WaitForSeconds(waitToRespawn);

        //Could re-load level, but this will also reset ability items even if obtained
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Move player back to respawn point, set active and refill health
        player.transform.position = respawnPoint;
        player.SetActive(true);
        PlayerHealthController.instance.FillHealth();
    }
}
