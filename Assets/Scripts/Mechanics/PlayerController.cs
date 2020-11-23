using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        #region OurStuff

        public float pushVelocity;
        public int pushIncrease;
        public float speed;
        public int allowedJumps;
        public GameObject powerDisplay;

        private float defaultPushVelocity;
        private bool hasLanded = true;
        private int remainingJumps;
        private PowerScript powerScript;

        #endregion

        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public GameObject satellite;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        //public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        //SpriteRenderer spriteRenderer;
        //internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            //spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
            remainingJumps = allowedJumps;
            powerScript = powerDisplay.GetComponent<PowerScript>();
        }

        /*void Start()
        {
            remainingJumps = allowedJumps;
            powerScript = powerDisplay.GetComponent<PowerScript>();
        }*/

        protected override void Update()
        {
            if (controlEnabled)
            {
                //move.x = Input.GetAxis("Horizontal");
                /*if (Input.GetButtonDown("ForcePush"))
                {
                    var dir = satellite.transform.position - transform.position;
                    //Debug.Log("player ForcePush dir: " + dir.ToString() + " X: " + dir.x + " Y: " + dir.y);
                    velocity.y = -dir.y * pushVelocity;
                    velocity.x = -dir.x * pushVelocity;
                }*/

                if (Input.GetButtonDown("Jump") && remainingJumps > 0)
                {
                    defaultPushVelocity = pushVelocity;
                    StartCoroutine(IncreasePushOnButton(0.5f, KeyCode.Space));
                }
                else if (Input.GetButtonUp("Jump") && remainingJumps > 0)
                {
                    jumpState = JumpState.PrepareToJump;

                    /*var dir = satellite.transform.position - transform.position;
                    //Debug.Log("test  ForcePush dir: " + dir.ToString() + " X: " + dir.x + " Y: " + dir.y);
                    //velocity = new Vector2(-dir.x * pushVelocity, -dir.y * pushVelocity);
                    //Bounce(new Vector2(-dir.x * pushVelocity, -dir.y * pushVelocity));

                    //reset pusVelocity
                    pushVelocity = defaultPushVelocity;

                    //tag car to be airborne
                    hasLanded = false;

                    powerScript.resetPowerLevel();

                    //Lower allowed jumps left
                    remainingJumps--;
                    powerScript.decreaseJumpTokens(remainingJumps);*/
                }

                /*if (jumpState == JumpState.Grounded && Input.GetButtonUp("Jump"))
                {
                    jumpState = JumpState.PrepareToJump;
                }*/
                /*else if (Input.GetButtonUp("Jump"))
                {
                    //Debug.Log("GetButtonUp - Jump");
                    //stopJump = true;
                    //Schedule<PlayerStopJump>().player = this;
                }*/
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    remainingJumps = allowedJumps;
                    powerScript.resetJumpTokens();
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {

            float moveDirX = 0.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("LeftArrow down");
                moveDirX = IsGrounded ? speed * -1 : speed / 4 * -1;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("RightArrow down");
                moveDirX = IsGrounded ? speed : speed / 4;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                Debug.Log("Key Up");
                moveDirX = velocity.x / 2;
            }

            Debug.Log("MoveDirX: " + moveDirX.ToString());


            if (jump && IsGrounded)
            {
                //velocity.y = 7 * model.jumpModifier;
                //velocity.x = 7 * model.jumpModifier;



                var dir = satellite.transform.position - transform.position;
                //Debug.Log("test  ForcePush dir: " + dir.ToString() + " X: " + dir.x + " Y: " + dir.y);
                velocity = new Vector2(-dir.x * pushVelocity + moveDirX, -dir.y * pushVelocity);
                //Bounce(new Vector2(-dir.x * pushVelocity, -dir.y * pushVelocity));

                //reset pusVelocity
                pushVelocity = defaultPushVelocity;

                //tag car to be airborne
                hasLanded = false;

                powerScript.resetPowerLevel();

                //Lower allowed jumps left
                remainingJumps--;
                powerScript.decreaseJumpTokens(remainingJumps);



                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            /*if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true; */

            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            //targetVelocity = move * maxSpeed;
        }

        /*private void forceJump()
        {
            Debug.Log("forceJump running");


            if (Input.GetButtonUp("Jump"))
            {
                var dir = satellite.transform.position - transform.position;
                Debug.Log("test  ForcePush dir: " + dir.ToString() + " X: " + dir.x + " Y: " + dir.y);
                velocity = new Vector2(-dir.x * pushVelocity, -dir.y * pushVelocity);

                //reset pusVelocity
                pushVelocity = defaultPushVelocity;

                //tag car to be airborne
                hasLanded = false;

                powerScript.resetPowerLevel();

                //Lower allowed jumps left
                remainingJumps--;
                powerScript.decreaseJumpTokens(remainingJumps);
            }
        }*/

        public IEnumerator IncreasePushOnButton(float delay, KeyCode code)
        {
            //Maximum 4 powerups
            while (Input.GetKey(code) && pushVelocity<(defaultPushVelocity + pushIncrease*4))
            {
                pushVelocity= pushVelocity+pushIncrease;
                powerScript.increasePowerLevel();
                Debug.Log("PushVelocity reached: " + pushVelocity);
                yield return new WaitForSeconds(delay);
            }

        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}