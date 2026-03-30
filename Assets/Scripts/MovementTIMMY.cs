using System.Xml.Serialization;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MovementTIMMY : MonoBehaviour
{
    public delegate void Hits();
    public static event Hits OpPunchLeft;
    public static event Hits OpPunchRight;
    public delegate void Mult();
    public static event Mult MCAdd2;
    public static event Mult MCAdd7;
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
    public bool CanPunchLeft = false;
    public bool CanPunchRight = false;
    public bool CheckForAuto = false;
    public bool canBlock = false;
    public bool LeftAutoPunch = false;
    public bool RightAutoPunch = false;
    public bool BlockPunching = false;
    public Collider hitboxLeft;
    public Collider hitboxRight;
    public Collider enemyHitbox;
    public bool isBlocking = false;
    public float TIMEFROMBLOCK = 0;
    public float currentTimescale = 1.5f;
    public int knockBackMult = 100;
    public float forceRight = 200;
    public bool canMove = true;
    public bool NOINPUTS = false;
    public Transform enemy;
    public AudioSource swings;
    public AudioSource lefthits;
    public AudioSource blockhits;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = currentTimescale;
        moveSpeed /= currentTimescale;
    }
    private void OnEnable()
    {
        OpAITIMMY.MePunchLeft += KnockbackLeft;
        UITRACKTIMMY.CanLeftPunch += EnableLeft;
        UITRACKTIMMY.CanBlockNow += EnableBlock;
        UITRACKTIMMY.CanRightPunch += EnableRight;
    }

    private void OnDisable()
    {
        OpAITIMMY.MePunchLeft -= KnockbackLeft;
        UITRACKTIMMY.CanLeftPunch -= EnableLeft;
        UITRACKTIMMY.CanBlockNow -= EnableBlock;
        UITRACKTIMMY.CanRightPunch -= EnableRight;
    }
    void EnableLeft()
    {
        CanPunchLeft = true;
    }
    void EnableRight()
    {
        CanPunchRight = true;
    }
    void EnableBlock()
    {
        canBlock = true;
    }
    void KnockbackLeft()
    {
        knockBackMult += 2;
        if (!isBlocking)
        {
            MCAdd7();
            knockBackMult += 5;
            canMove = false;
            Invoke(nameof(CanMvoeAgain), 0.3f);
            rb.AddForce(-enemy.transform.forward * (forceRight / 5) * (knockBackMult / 100f), ForceMode.Impulse);
            lefthits.Play();
            Invoke(nameof(STOPLEFt), 0.4f);
        }
        else
        {
            MCAdd2();
            blockhits.Play();
            Invoke(nameof(blockStop), 0.4f);
        }
    }

    void STOPLEFt()
    {
        lefthits.Stop();
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

    private void OnLeftPunch()
    {
        if (NOINPUTS) return;
        if (CanPunch && !BlockPunching && CanPunchLeft)
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
                moveSpeed = 10 / currentTimescale;
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
        if (CanPunch && !BlockPunching && CanPunchRight)
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
                moveSpeed = 10 / currentTimescale;
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

    private void OnBlocking(InputValue inputValue) {
        if (NOINPUTS || canBlock == false) return;
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
            moveSpeed = 10 / 3 / currentTimescale;
            CheckForAuto = false;
        }
        else
        {
            moveSpeed = 10 / currentTimescale;
        }
    }

    void LeftPunch()
    {
        if ((CanPunch && !isBlocking && CanPunchLeft) || BlockPunching)
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
        if (NOINPUTS) return;
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
        if ((CanPunch && !isBlocking && CanPunchRight) || BlockPunching)
        {
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
