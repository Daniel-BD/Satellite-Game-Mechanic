using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject satellite;
    public Rigidbody2D car;
    public float pushVelocity;
    public float pushIncrease;
    public float speed;
    public float inFlightXSpeed;
    public int allowedJumps;
    public GameObject powerDisplay;

    private float defaultPushVelocity;
    private bool hasLanded = true;
    private int remainingJumps;
    private PowerScript powerScript;

    private Vector2 currentVel;
    
    

    // Start is called before the first frame update
    void Start()
    {
        remainingJumps = allowedJumps;
        powerScript = powerDisplay.GetComponent<PowerScript>();
    }

    void Update()
    {
        // So that you can only jump a certain amount of jumps
        if (remainingJumps != 0)
        {
            jump();
        }

    }

    void FixedUpdate()
    {
        // So that you can't move in air (We might want to look into this)
        checkIfHasLanded();
        move(!hasLanded);
    }



    //Supporting Methods

    public IEnumerator IncreasePushOnButton(float delay, KeyCode code)
    {
        //Maximum 4 powerups
        while (Input.GetKey(code) && pushVelocity<(defaultPushVelocity + pushIncrease*4))
        {
            pushVelocity= pushVelocity+pushIncrease;
            powerScript.increasePowerLevel();

            //Unfreeze the car after 2 ticks
            if (pushVelocity > (defaultPushVelocity + pushIncrease * 3) && car.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                car.constraints = RigidbodyConstraints2D.FreezeRotation;
                car.velocity = currentVel;
            }
            

            yield return new WaitForSeconds(delay);
        }

    }

   
    private void jump()
    {
        if (Input.GetButtonDown("ForcePush"))
        {
            currentVel = car.velocity;
            if (!hasLanded || car.velocity.y < 0)
            {
                car.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            
            Debug.Log("DOWN: ForcePush");
            defaultPushVelocity = pushVelocity;
            StartCoroutine(IncreasePushOnButton(0.5f, KeyCode.Space));

        }

        if (Input.GetButtonUp("ForcePush"))
        {
            if (car.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                car.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            Debug.Log("UP: ForcePush");
            var dir = satellite.transform.position - transform.position;
            car.velocity = new Vector2(-dir.x * pushVelocity, -dir.y * pushVelocity);

            //reset pushVelocity
            pushVelocity = defaultPushVelocity;

            //tag car to be airborne
            hasLanded = false;

            powerScript.resetPowerLevel();

            //Lower allowed jumps left
            remainingJumps--;
            powerScript.decreaseJumpTokens(remainingJumps);
        }
    }

    private void move(bool inFlight)
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (inFlight || car.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                //car.AddForce(new Vector2(-1f, 0.0f));
                //car.velocity = car.velocity + new Vector2(inFlightXSpeed * -0.2f, 0.0f);
                //var tempVelocity = car.velocity;
                //car.position = car.position + new Vector2(-0.1f, 0.0f);
                //car.velocity = tempVelocity;
            }
            else
            {
                car.velocity = new Vector2(0f, car.velocity.y);
                car.position = car.position + new Vector2(-0.1f, 0.0f);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (inFlight || car.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                //car.AddForce(new Vector2(1f, 0.0f));
                //car.position = car.position + new Vector2(0.05f, 0.0f);
                //car.velocity = car.velocity + new Vector2(inFlightXSpeed * 0.2f, 0.0f);
            }
            else
            {
                car.velocity = new Vector2(0f, car.velocity.y);
                car.position = car.position + new Vector2(0.1f, 0.0f);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            //If we want to implement slide we can try to do that here, but the "GetKeyUp" call is unreliable
            //and doesn't catch every key up event...
        }

    }

    //Bad way of doing it, causes a bit of lag
    private void checkIfHasLanded()
    {
        if (car.velocity.y == 0 && car.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            hasLanded = true;
            remainingJumps = allowedJumps;
            powerScript.resetJumpTokens();
        }
    }

    /// <summary>
    /// Teleport to some position.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        Debug.Log("teleporting car");
        car.position = position;
        car.velocity *= 0;
    }

    public void resetPosition()
    {
        car.position = new Vector2(0.39f, -0.49f);
        car.velocity = new Vector2(0f, 0f);
    }


}
