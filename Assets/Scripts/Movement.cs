using System.Xml.Serialization;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public delegate void Hits();
    public static event Hits OpPunchLeft;
    public static event Hits OpPunchRight;
    public static event Hits PredictRight;
    public static event Hits Stop;
    public static event Hits StartBackUp;
    public static event Hits NowBlocking;
    public static event Hits NotBlocking;
    public delegate void Mult();
    public static event Mult MCAdd1;
    public static event Mult MCAdd2;
    public static event Mult MCAdd5;
    public static event Mult ThinkingLeft;
    public static event Mult ThinkingRight;
    private Vector2 MovementDirection;
    private Vector2 LookDirection;
    public float moveSpeed = 15;
    public Rigidbody rb;
    public Camera cam;
    public float sensitvity = 1;
    private Vector3 positionChange;
    public Animator leftAnimator;
    public Animator rightAnimator;
    public bool CanPunch = true;
    public bool CheckForAuto = false;
    public bool LeftAutoPunch = false;
    public bool RightAutoPunch = false;
    public bool BlockPunching = false;
    public Collider hitboxLeft;
    public Collider hitboxRight;
    public Collider enemyHitbox;
    public bool isThinking = false;
    public bool isBlocking = false;
    public float TIMEFROMBLOCK = 0;
    public float currentTimescale = 1.5f;
    public int knockBackMult = 100;
    public float forceRight = 200;
    public bool canMove = true;
    public Transform sphere1;
    public bool OVERIDETHINK = false;
    public bool NOINPUTS = false;
    public Transform enemy;
    public List<string> thinkingPunches = new List<string>();
    public Transform first;
    public Transform second;
    public Transform thrid;    
    public bool THinkingALlowed = false;
    public AudioSource swings;
    public AudioSource lefthits;
    public AudioSource righthits;
    public AudioSource blockhits;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = currentTimescale;
        moveSpeed /= currentTimescale;
    }
    private void OnEnable()
    {
        OpAI.MePunchLeft += KnockbackLeft;
        OpAI.MePunchRight += KnockbackRight;
        OpAI.MePunchBlock += KnockBackBlockHit;
    }

    private void OnDisable()
    {
        OpAI.MePunchLeft -= KnockbackLeft;
        OpAI.MePunchRight -= KnockbackRight;
        OpAI.MePunchBlock -= KnockBackBlockHit;
    }
    void KnockbackLeft()
    {
        knockBackMult += 2;
        MCAdd2();
        if (!isBlocking)
        {
            MCAdd5();
            knockBackMult += 5;
            canMove = false;
            Invoke(nameof(CanMvoeAgain), 0.3f);
            rb.AddForce(-enemy.transform.forward * (forceRight / 5) * (knockBackMult / 100f), ForceMode.Impulse);
            lefthits.Play();
            Invoke(nameof(STOPLEFt), 0.4f);
        }
        else
        {
            blockhits.Play();
            Invoke(nameof(blockStop), 0.4f);
        }
    }
    void KnockbackRight()
    {
        knockBackMult += 1;
        MCAdd1();
        if (!isBlocking)
        {
            canMove = false;
            Invoke(nameof(CanMvoeAgain), 0.8f);
            rb.AddForce(-enemy.transform.forward * forceRight * (knockBackMult / 100f), ForceMode.Impulse);
            righthits.Play();
            Invoke(nameof(STOPRIGht), 0.4f);
        }
        else
        {
            blockhits.Play();
            Invoke(nameof(blockStop), 0.4f);
        }
    }
    void KnockBackBlockHit()
    {
        MCAdd5();
        knockBackMult += 5;
        rb.AddForce(-enemy.transform.forward * forceRight * (knockBackMult / 75f), ForceMode.Impulse);
        canMove = false;
        Invoke(nameof(CanMvoeAgain), 0.5f);
    } 
    void STOPLEFt()
    {
        lefthits.Stop();
    }
    void STOPRIGht()
    {
        righthits.Stop();
    }
    void blockStop()
    {
        blockhits.Stop();
    }
    void CanMvoeAgain()
    {
        canMove = true;
    }
    private void OnMove(InputValue inputValue) {
        if (NOINPUTS) return;
        MovementDirection = inputValue.Get<Vector2>();
    }
    private void OnLook(InputValue inputValue) {
        if (NOINPUTS) return;
        LookDirection = inputValue.Get<Vector2>();
    }
    private int pain = 50;
    void HIT()
    {
        if (isThinking && pain > 1)
        {
            pain -= 1;
            Invoke(nameof(HIT), 0.02f);
        } 
        else
        {
            MovementDirection = new Vector2(0,0);
            rb.linearVelocity = new Vector3(0,0,0);
            Stop();
            pain = 50;
            OVERIDETHINK = true;
            isThinking = false;
            NOINPUTS = true;
            //StartBackUp();
            //NOINPUTS = false;
            //thinkingPunches.Clear();
        }
    }

    private void OnLeftPunch()
    {
        if (NOINPUTS) return;
        if (isThinking)
        {
            if (pain > 1)
            {
                pain -= 10;
                thinkingPunches.Add("Left");
            }
        }
        else if (CanPunch && !BlockPunching)
        { 
            if (isBlocking)
            {
                CanPunch = false;
                leftAnimator.SetBool("Block", false);
                rightAnimator.SetBool("Block", false);
                leftAnimator.SetBool("LeftPunch", false);
                rightAnimator.SetBool("RightPunch", false);
                isBlocking = false;
                BlockPunching = true;
                moveSpeed = 7 / currentTimescale;
                Invoke(nameof(LeftPunch), 0.5f + Mathf.Clamp(0.5f - (Time.time - TIMEFROMBLOCK), 0, 0.5f));
            }
            else
            {
                LeftPunch();
            }
        }
        else if (CheckForAuto)
        {
            LeftAutoPunch = true;
        }
    }
    private void OnRightPunch(InputValue inputValue)
    {
        if (NOINPUTS) return;
        if (isThinking)
        {
            if (pain > 1)
            {
                pain -= 50;
                thinkingPunches.Add("Right");
            }
        }
        else if (CanPunch && !BlockPunching)
        { 
            if (isBlocking)
            {
                CanPunch = false;
                leftAnimator.SetBool("Block", false);
                rightAnimator.SetBool("Block", false);
                leftAnimator.SetBool("LeftPunch", false);
                rightAnimator.SetBool("RightPunch", false);
                isBlocking = false;
                BlockPunching = true;
                moveSpeed = 7 / currentTimescale;
                Invoke(nameof(RightPunch), 0.5f + Mathf.Clamp(0.5f - (Time.time - TIMEFROMBLOCK), 0, 0.5f));
            }
            else
            {
                RightPunch();
            }
        }
        else if (CheckForAuto)
        {
            RightAutoPunch = true;
        }
    }

    private void OnThinking(InputValue inputValue) {
        if (!THinkingALlowed)
        {
            return;
        }
        if (OVERIDETHINK)
        {
            OVERIDETHINK = false;
            return;
        }
        if (NOINPUTS) return;
        isThinking = !isThinking;

        if (isThinking)
        {
            Invoke(nameof(HIT), 0.02f);
            Time.timeScale = 0.1f;
        }
        else
        {
            Time.timeScale = currentTimescale;
        }
    }

    private void OnBlocking(InputValue inputValue) {
        if (NOINPUTS) return;
        if (Time.time - TIMEFROMBLOCK >= 0.5)
        {
            BlockFunction();
        }
        else
        {
            Invoke(nameof(BlockFunction), 0.5f - (Time.time - TIMEFROMBLOCK));
        }
    }

    void BlockFunction()
    {
        TIMEFROMBLOCK = Time.time;
        isBlocking = !isBlocking;
        leftAnimator.SetBool("Block", isBlocking);
        rightAnimator.SetBool("Block", isBlocking);
        if (isBlocking)
        {
            NowBlocking();
            moveSpeed = 7 / 3 / currentTimescale;
            CheckForAuto = false;
        }
        else
        {
            NotBlocking();
            moveSpeed = 7 / currentTimescale;
        }
    }

    void LeftPunch()
    {
        if ((CanPunch && !isBlocking) || BlockPunching)
        {
            BlockPunching = false;
            CheckForAuto = false;
            LeftAutoPunch = false;
            RightAutoPunch = false;
            CanPunch = false;
            swings.Play();
            leftAnimator.SetBool("LeftPunch", true);
            Invoke(nameof(leftHitbox), 0.1f);
            Invoke(nameof(CanLEFTRIGHTPunch), 0.5f);
        }
    }

    void CanLEFTRIGHTPunch()
    {
        leftAnimator.SetBool("LeftPunch", false);
        rightAnimator.SetBool("RightPunch", false);
        if (NOINPUTS || !gameObject.GetComponent<Movement>().enabled) return;
        CanPunch = true;
        if (LeftAutoPunch)
        {
            LeftPunch();
        }
        else if (RightAutoPunch)
        {
            RightPunch();
        }
    }

    void leftHitbox()
    {
        CheckForAuto = true;

        if (hitboxLeft.bounds.Intersects(enemyHitbox.bounds))
        {
            OpPunchLeft();
        }
    }

    void rightHitbox()
    {
        CheckForAuto = true;
        if (hitboxRight.bounds.Intersects(enemyHitbox.bounds))
        {
            OpPunchRight();
        }
    }

    void RightPunch()
    {
        if ((CanPunch && !isBlocking) || BlockPunching)
        {
            PredictRight();
            BlockPunching = false;
            CheckForAuto = false;
            LeftAutoPunch = false;
            RightAutoPunch = false;
            CanPunch = false;
            swings.Play();
            rightAnimator.SetBool("RightPunch", true);
            Invoke(nameof(rightHitbox), 0.1f);
            Invoke(nameof(CanLEFTRIGHTPunch), 1.17f);
        }
    }

    void Update()
    {
        cam.transform.Rotate(Vector3.up, LookDirection.x * 10 * Time.deltaTime);
    }

    void FixedUpdate()
    {
        thrid.position = (first.position + second.position)/2;
        thrid.eulerAngles = new Vector3(90, 0, Mathf.Atan2(first.position.z - second.position.z, first.position.x - second.position.x) * Mathf.Rad2Deg + 90);
        thrid.localScale = new Vector3(0.79f, Vector3.Distance(first.position, second.position) / 2, 0.79f);
        if (NOINPUTS)
        {
            
        }
        else
        {
            positionChange = cam.transform.forward * MovementDirection.y + cam.transform.right * MovementDirection.x;
            if (positionChange != new Vector3(0,0,0) && canMove)
            {
                rb.linearVelocity = positionChange.normalized * moveSpeed;
            }
        }
    }
}
