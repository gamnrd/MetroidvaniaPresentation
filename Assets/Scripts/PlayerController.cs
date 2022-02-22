using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Animator anim;

    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public BulletController shotToFire;
    public Transform shotPoint;


    public SpriteRenderer theSR, afterImage;
    public float afterImageLifetime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    public float waitAfterDashing;
    private float dashRechargeCounter;

    //Abilities
    private PlayerAbilityTracker abilities;

    private bool canDoubleJump;

    public float dashSpeed;
    public float dashTime;
    private float dashCounter;

    public GameObject standing, ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    public Transform bombPoint;
    public GameObject bomb;


    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
    }




    // Update is called once per frame
    void Update()
    {
        //If dash is recharging
        if(dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else //dash is recharged
        {
            //If dash button pressed and player standing
            if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
            {
                //Start dash timer (check movement for dash code)
                dashCounter = dashTime;
                ShowAfterImage(); //display after images
            }
        }



        if (dashCounter > 0)
        {
            //Countdown dash counter
            dashCounter -= Time.deltaTime;
            //Dash left/right
            theRB.velocity = new Vector2(transform.localScale.x * dashSpeed, theRB.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if(afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = waitAfterDashing;
        }
        else
        {
            //Move left/right
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);
            
            //Handle direction change
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                shotToFire.moveDir.x = -1;
            }
            else if (theRB.velocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                shotToFire.moveDir.x = -1;
            }
        }



        //check if on ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        //jump
        if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump))) //If jump pressed and player is on the ground
        {
            if (isOnGround)
            {
                canDoubleJump = true;
            }
            else
            {
                anim.SetTrigger("doubleJump");
                canDoubleJump = false;
            }
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }



        //Shoot
        if (Input.GetButtonDown("Fire1"))
        {
            if(standing.activeSelf)
            {
                anim.SetTrigger("shotFired");
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0);
            }
            else if(ball.activeSelf && abilities.canDropBomb)
            {
                Instantiate(bomb, bombPoint.position, bombPoint.rotation);

            }
        }


        //Ball mode
        if(!ball.activeSelf && abilities.canBecomeBall)
        {
            if(Input.GetAxisRaw("Vertical") < -.9f)
            {
                ballCounter -= Time.deltaTime;
                if(ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
        else if(ball.activeSelf)
        {
            if(Input.GetAxisRaw("Vertical") > .9f)
            {
                ballCounter -= Time.deltaTime;
                if(ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                }

            }
            else
            {
                ballCounter = waitToBall;
            }
        }


        if(standing.activeSelf)
        {
            //Set animators "isonground" bool to the value of the scripts isonground
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("Speed", Mathf.Abs(theRB.velocity.x));
        }
        if (ball.activeSelf)
        {
            //Set animators "isonground" bool to the value of the scripts isonground
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
    }


    //Create Dash after images
    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }



}
