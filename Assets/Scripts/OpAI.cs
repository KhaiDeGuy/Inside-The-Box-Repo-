using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpAI : MonoBehaviour
{
    public delegate void Hits();
    public static event Hits MePunchLeft;
    public static event Hits MePunchRight;
    public static event Hits MePunchBlock;
    public delegate void Mult();
    public static event Mult EnAdd1;
    public static event Mult EnAdd2;
    public static event Mult EnAdd5;
    public float moveSpeed = 1;
    public Rigidbody rb;
    public float forceRight;
    public Camera cam;
    public float MoveTime = 1;
    private Vector3 positionChange;
    private Vector3 trueChange;
    public Animator leftAnimator;
    public Animator rightAnimator;
    public Animator leftAnimator2;
    public Animator rightAnimator2;
    public bool IsPunching = false;
    private bool IsPunchingAgain = false;
    public bool Right = false;
    public bool Left = false;
    public float turnSpeed;
    public GameObject player;
    public int knockBackMult = 100;
    public Animator EyeAnim;
    public bool turnNormal;
    private int turnCounter = 0;
    private int otherCounter = 0;
    public Collider hitboxLeft;
    public Collider hitboxRight;
    public Collider enemyHitbox;
    public bool isBlocking = false;
    public bool isBlocking2 = false;
    public bool canMove = true;
    public bool NOINPUTS = false;
    public bool ForceBlock = false;
    public bool ForceRightPunch = false;
    public bool ForcePunch = false;
    public bool breakBlock = false;
    public bool breakBlock2 = false;
    public int forcePunchCounter = 0;
    public float TIMEFROMBLOCK = 0;
    public AudioSource lefthits;
    public AudioSource righthits;
    public AudioSource swings;
    public AudioSource blockhits;
    public AudioSource lefthits2;
    public AudioSource righthits2;
    public AudioSource swings2;
    public AudioSource blockhits2;
    public int MODE;
    public bool nowIsBlocking = false;
    public int AHHHH = 0;
    public int WindingUp = 0;
    public int guyEightMult = 0;

    private void OnEnable()
    {
        Movement.OpPunchLeft += KnockbackLeft;
        Movement.OpPunchRight += KnockbackRight;
        Movement.Stop += STOPEVERYTHING;
        Movement.StartBackUp += BackAgain;
        Movement.NowBlocking += CurerenlyBlocking;
        Movement.NotBlocking += NonoBlock;
        Movement.PredictRight += rightAttack;
        UITRACK.Add50 += GuyEight;
    }

    private void OnDisable()
    {
        Movement.OpPunchLeft -= KnockbackLeft;
        Movement.OpPunchRight -= KnockbackRight;
        Movement.Stop -= STOPEVERYTHING;
        Movement.StartBackUp -= BackAgain;
        Movement.NowBlocking -= CurerenlyBlocking;
        Movement.NotBlocking -= NonoBlock;
        Movement.PredictRight -= rightAttack;
        UITRACK.Add50 -= GuyEight;
    }

    void GuyEight()
    {
        guyEightMult += 1;
    }
    void rightAttack()
    {
        AHHHH += 1;

        if (AHHHH > 5)
        {
            ForceBlock = true;
        }
        if (AHHHH > 2)
        {
            if (Random.Range(0,2) == 0) ForceBlock = true;
        }
    }

    void Start()
    {
        if (MODE == 1) moveSpeed = 6;
        else if (MODE == 2) moveSpeed = 7;
        Invoke(nameof(RemoveSlighly), 6);
    }

    void RemoveSlighly()
    {
        AHHHH = Mathf.Clamp(AHHHH - 1, 0, 999);
        Invoke(nameof(RemoveSlighly), 2.5f);
    }

    void CurerenlyBlocking()
    {
        nowIsBlocking = true;
    }
    void NonoBlock()
    {
        nowIsBlocking = false;
    }

    void STOPEVERYTHING()
    {
        NOINPUTS = true;
    }
    void BackAgain()
    {
        NOINPUTS = false;
    }
    void KnockbackLeft()
    {
        bool boolyes = !EyeAnim.GetBool("ah");
        EyeAnim.SetBool("ah", boolyes);
        EyeAnim.SetBool("ha", !boolyes);
        knockBackMult += 2;

        if (!isBlocking)
        {
            canMove = false;
            knockBackMult += 3;
            knockBackMult += 2;
            Invoke(nameof(CanMvoeAgain), 0.3f);
            rb.AddForce(cam.transform.forward * (forceRight / 5) * (knockBackMult / 100f), ForceMode.Impulse);
            EnAdd5();
            EnAdd2();
            if (MODE == 1)
            {
                if (Random.Range(0, 6) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH
                    ForceRightPunch = true;
                }
            }
            else if (MODE == 2)
            {
                if (Random.Range(0, 4) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH
                    ForceRightPunch = true;
                }
            }
            else if (MODE == 3)
            {
                if (Random.Range(0, 5) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH
                    ForceRightPunch = true;
                }
            }
            else if (MODE == 4)
            {
                if (Random.Range(0, 4) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH OR BLOCK
                    if (Random.Range(0, 2) == 0) ForceRightPunch = true;
                    else ForceBlock = true;
                }
            }
            else if (MODE == 6)
            {
                if (Random.Range(0, 5) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH
                    ForceRightPunch = true;
                }
            }
            else if (MODE == 7 || MODE == 9)
            {
                if (Random.Range(0, 4) == 0)
                {
                    // HIT WITH LEFT MULTIPLE TIMES, RETALIATES WITH RIGHT PUNCH
                    ForceRightPunch = true;
                }
            }
            lefthits.Play();
            Invoke(nameof(STOPLEFt), 0.4f);
        }
        else
        {
            if (MODE == 4) MePunchBlock();
            ForcePunch = true;
            forcePunchCounter += 1;
            Invoke(nameof(UnForcePunches), 6f);
            EnAdd2();
            blockhits.Play();
            Invoke(nameof(blockStop), 0.4f);
        }

        if (WindingUp == 1)
        {
            rightAnimator.SetBool("Wind", false);
            WindingUp = 0;
            IsPunching = false;
        }

        Invoke(nameof(NoKnock), 0.5f);
        turnNormal = false;
        TurnTime();
    }
    void KnockbackRight()
    {
        bool boolyes = !EyeAnim.GetBool("ha");
        EyeAnim.SetBool("ha", boolyes);
        EyeAnim.SetBool("ah", !boolyes);
        knockBackMult += 1;
        EnAdd1();
        
        if (!isBlocking && !isBlocking2)
        {
            canMove = false;
            Invoke(nameof(CanMvoeAgain), 0.8f);
            rb.AddForce(cam.transform.forward * forceRight * (knockBackMult / 100f), ForceMode.Impulse);
            righthits.Play();
            Invoke(nameof(STOPRIGht), 0.4f);
        }
        else if (!isBlocking2)
        {
            breakBlock = true;
            if (MODE == 1)
            {
                if (Random.Range(0, 2) == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else ForceRightPunch = true;
            }
            else if (MODE == 2)
            {
                if (Random.Range(0, 3) == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else ForceRightPunch = true;
            }
            else if (MODE == 3)
            {
                if (Random.Range(0, 5) == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else ForceRightPunch = true;
            }
            else if (MODE == 4)
            {
                MePunchBlock();
                int randoTemp = Random.Range(0, 3);
                if (randoTemp == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else if (randoTemp == 1)
                {
                    ForceRightPunch = true;
                }
            }
            else if (MODE == 5 || MODE == 6 || MODE == 7)
            {
                ForceRightPunch = true;
            }
            else if (MODE == 8)
            {
                if (Random.Range(0, 5) == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else ForceRightPunch = true;
            }
            else if (MODE == 9)
            {
                if (Random.Range(0, 6) == 0)
                {
                    // RIGHT PUNCH PUNISH AFTER BLOCKING RIGHT PUNCH
                    ForcePunch = true;
                    forcePunchCounter += 1;
                    Invoke(nameof(UnForcePunches), 3f);
                }
                else ForceRightPunch = true;
            }
            
            leftAnimator.SetBool("Block", false);
            rightAnimator.SetBool("Block", false);
            Invoke(nameof(UnBlock), 0.5f + Mathf.Clamp(0.5f - (Time.time - TIMEFROMBLOCK), 0, 0.5f));
            blockhits.Play();
            Invoke(nameof(blockStop), 0.4f);
        }
        else
        {
            breakBlock2 = false;
            ForceRightPunch = true;
            leftAnimator2.SetBool("Block", false);
            rightAnimator2.SetBool("Block", false);
            Invoke(nameof(UnBlock), 0.5f + Mathf.Clamp(0.5f - (Time.time - TIMEFROMBLOCK), 0, 0.5f));
            blockhits2.Play();
            Invoke(nameof(blockStop2), 0.4f);
        }

        if (WindingUp == 1)
        {
            rightAnimator.SetBool("Wind", false);
            WindingUp = 0;
            IsPunching = false;
        }

        Invoke(nameof(NoKnock), 0.5f);
        turnNormal = false;
        OtherTurnTime();
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

    void blockStop2()
    {
        blockhits2.Stop();
    }
    
    void UnForcePunches()
    {
        forcePunchCounter -= 1;
        if (forcePunchCounter == 0)
        {
            ForcePunch = false;
        }
    }
    void CanMvoeAgain()
    {
        canMove = true;
    }

    void TurnTime()
    {
        if (otherCounter != 5)
        {
            otherCounter += 1;
            transform.eulerAngles += new Vector3(0, -4, 0);
            Invoke(nameof(TurnTime), 0.015f);
        }
        else
        {
            otherCounter = 0;
            Invoke(nameof(TurnEnd), 0.2f);
        }
    }

    void OtherTurnTime()
    {
        if (otherCounter != 5)
        {
            otherCounter += 1;
            transform.eulerAngles += new Vector3(0, 6, 0);
            Invoke(nameof(OtherTurnTime), 0.015f);
        }
        else
        {
            otherCounter = 0;
            Invoke(nameof(TurnEnd), 0.3f);
        }
    }

    void TurnEnd()
    {
        turnCounter += 1;
        turnNormal = true;
        turnSpeed *= 5;
        Invoke(nameof(TurnDamper), 0.3f);
    }

    void TurnDamper()
    {
        turnCounter -= 1;
        if (turnCounter == 0) turnSpeed /= 5;
    }

    void NoKnock()
    {
        rb.linearVelocity = new Vector3(0,0,0);
    }
    void LeftPunch()
    {
        leftAnimator.SetBool("LeftPunch", true);
        swings.Play();
        if (MODE == 5) 
        {
            Invoke(nameof(leftHitbox), 0.2f);
            Invoke(nameof(NotPunch), 0.75f);
        }
        else 
        {
            Invoke(nameof(leftHitbox), 0.1f);
            Invoke(nameof(NotPunch), 0.5f);
        }
    }

    void LeftPunchAgain()
    {
        leftAnimator2.SetBool("LeftPunch", true);
        swings2.Play();
        Invoke(nameof(leftHitbox2), 0.1f);
        Invoke(nameof(NotPunchAgain), 0.5f);
    }

    void leftHitbox()
    {
        leftAnimator.SetBool("LeftPunch", false);
        rightAnimator.SetBool("RightPunch", false);
        if (hitboxLeft.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchLeft();

            if (guyEightMult > 0)
            {
                for (int i = 0; i < guyEightMult; i++)
                {
                    MePunchLeft();
                }
            }
        }
    }

    void rightHitbox()
    {
        leftAnimator.SetBool("LeftPunch", false);
        rightAnimator.SetBool("RightPunch", false);
        if (hitboxRight.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchRight();

            if (guyEightMult > 0)
            {
                for (int i = 0; i < Mathf.Clamp(guyEightMult, 0, 1); i++)
                {
                    MePunchRight();
                }
            }

            if (WindingUp == 2)
            {
                MePunchRight();
                MePunchRight();
                MePunchRight();
            }
        }
    }

    void leftHitbox2()
    {
        leftAnimator2.SetBool("LeftPunch", false);
        rightAnimator2.SetBool("RightPunch", false);
        if (hitboxLeft.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchLeft();
        }
    }

    void rightHitbox2()
    {
        leftAnimator2.SetBool("LeftPunch", false);
        rightAnimator2.SetBool("RightPunch", false);
        if (hitboxRight.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchRight();
        }
    }

    void NotPunch()
    {
        if (WindingUp == 2)
        {
            WindingUp = 0;
            rightAnimator.SetBool("Wind", false);
        }
        IsPunching = false;
    }

    void NotPunchAgain()
    {
        IsPunchingAgain = false;
    }

    void RightPunch()
    {
        rightAnimator.SetBool("RightPunch", true);
        swings.Play();
        if (MODE == 5) 
        {
            Invoke(nameof(rightHitbox), 0.2f);
            Invoke(nameof(NotPunch), 1.5f);
        }
        else 
        {
            Invoke(nameof(rightHitbox), 0.1f);
            Invoke(nameof(NotPunch), 1.2f);
        }
    }

    void RightPunchAgain()
    {
        rightAnimator2.SetBool("RightPunch", true);
        swings2.Play();
        Invoke(nameof(rightHitbox2), 0.1f);
        Invoke(nameof(NotPunchAgain), 1.2f);
    }

    void WindUpPunch()
    {
        if (WindingUp == 1)
        {
            rb.AddForce(-transform.forward * 100, ForceMode.Impulse);
            WindingUp = 2;
            rb.linearVelocity = positionChange.normalized * 20;
            swings.Play();
            Invoke(nameof(rightHitbox), 0.25f);
            Invoke(nameof(NotPunch), 1.5f);
        }
    }

    private void Block() {
        isBlocking = !isBlocking;
        leftAnimator.SetBool("Block", isBlocking);
        rightAnimator.SetBool("Block", isBlocking);
        if (isBlocking)
        {
            if (MODE == 6 && isBlocking2 == false) moveSpeed = 4;
            else moveSpeed = 2.2f;
            TIMEFROMBLOCK = Time.time;
        }
        else if(!breakBlock)
        {
            if (MODE == 6 && isBlocking == true) moveSpeed = 4;
            else moveSpeed = 6.6f;
            Invoke(nameof(UnBlock), 0.5f);
        }
        else
        {
            if (MODE == 6 && isBlocking == true) moveSpeed = 4;
            else moveSpeed = 6.6f;
            breakBlock = false;
        }
    }

    void UnBlock()
    {
        IsPunching = false;
    }

    private void Block2() {
        isBlocking2 = !isBlocking2;
        leftAnimator2.SetBool("Block", isBlocking2);
        rightAnimator2.SetBool("Block", isBlocking2);
        if (isBlocking2)
        {
            if (MODE == 6 && isBlocking == false) moveSpeed = 4;
            else moveSpeed = 2.2f;
            TIMEFROMBLOCK = Time.time;
        }
        else if(!breakBlock2)
        {
            if (MODE == 6 && isBlocking == true) moveSpeed = 4;
            else moveSpeed = 6.6f;
            Invoke(nameof(UnBlock2), 0.5f);
        }
        else
        {
            if (MODE == 6 && isBlocking == true) moveSpeed = 4;
            else moveSpeed = 6.6f;
            breakBlock2 = false;
        }
    }

    void UnBlock2()
    {
        IsPunchingAgain = false;
    }

    void FixedUpdate()
    {
        float wantedAngle = 90 - Mathf.Atan2(transform.position.z - player.transform.position.z, transform.position.x - player.transform.position.x) * Mathf.Rad2Deg;
        if (wantedAngle < 0) wantedAngle += 360;
        int posOrNegative;
        float curAngle = transform.eulerAngles.y;
        float trueTurn;

        if (Mathf.Abs(wantedAngle - curAngle) > 180) 
        {
            posOrNegative = -1; 
            trueTurn = 360 - Mathf.Abs(wantedAngle - curAngle);
        }
        else
        {
            posOrNegative = 1;
            trueTurn = Mathf.Abs(wantedAngle - curAngle);
        }

        trueTurn = Mathf.Clamp(turnSpeed * trueTurn, 0.1f, 9999);

        if (turnNormal)
        {
            if (wantedAngle - curAngle > trueTurn)
            {
                transform.eulerAngles = new Vector3(0, curAngle + trueTurn * posOrNegative, 0);
            }
            else if (curAngle - wantedAngle > trueTurn)
            {
                transform.eulerAngles = new Vector3(0, curAngle - trueTurn * posOrNegative, 0);
            }
            else if (Mathf.Abs(curAngle - wantedAngle) < trueTurn)
            {
                transform.eulerAngles = new Vector3(0, wantedAngle, 0);
            }
        }

        positionChange = player.transform.position - transform.position;
        if (positionChange != new Vector3(0,0,0) && canMove && !NOINPUTS)
        {
            // MOVEMENT BEHAVIOR
            if (MODE == 1)
            {
                if (Vector3.Distance(positionChange, Vector3.zero) < 9)
                {
                    //rb.linearVelocity = positionChange.normalized * moveSpeed;
                }
                else
                {
                    rb.linearVelocity = positionChange.normalized * moveSpeed;
                }
            }
            else if (MODE == 3)
            {
                if (Vector3.Distance(positionChange, Vector3.zero) < 10 && transform.position.x * transform.position.x + transform.position.z * transform.position.z < 100)
                {
                    //rb.linearVelocity = positionChange.normalized * moveSpeed;
                }
                else
                {
                    rb.linearVelocity = positionChange.normalized * moveSpeed;
                }
            }
            else if (MODE == 7 && WindingUp != 0)
            {
                if (WindingUp != 2 && transform.position.x * transform.position.x + transform.position.z * transform.position.z < 200) rb.linearVelocity = -positionChange.normalized * moveSpeed;
                else if (WindingUp != 2 && transform.position.x * transform.position.x + transform.position.z * transform.position.z > 220) rb.linearVelocity = positionChange.normalized * moveSpeed;
            }
            else
            {
                rb.linearVelocity = positionChange.normalized * moveSpeed;
            }
        }

        if (!NOINPUTS && !IsPunching)
        {
             // NORMAL HIT PROBABLITIES
            if (MODE == 1 && Vector3.Distance(player.transform.position, transform.position) < 13f + (Random.Range(0, 31) / 10))
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 13);
                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 20) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else if (randomNum == 0 || randomNum == 2 || randomNum == 3)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (randomNum == 1 && !ForcePunch)
                {
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 30) / 10);
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 2 && Vector3.Distance(player.transform.position, transform.position) < 9.5f)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 11);
                if (nowIsBlocking) randomNum = Mathf.Clamp(randomNum - 2, 0, 11);
                
                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(7, 12) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else if (randomNum == 0 || randomNum == 2 || randomNum == 3 || randomNum == 1)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (randomNum == 4 && !ForcePunch)
                {
                    if (Random.Range(0, knockBackMult) > 130)
                    {
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 20) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 3 && Vector3.Distance(player.transform.position, transform.position) < 10.5f)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 10);
                if (nowIsBlocking) randomNum += 3;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(15, 25) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else if (randomNum == 2)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0 || randomNum == 1) && !ForcePunch)
                {
                    if (Random.Range(0, transform.position.x * transform.position.x + transform.position.z * transform.position.z) < 200)
                    {
                        Block();
                        Invoke(nameof(Block), Random.Range(5, 20) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 4 && Vector3.Distance(player.transform.position, transform.position) < 9)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 6);
                if (nowIsBlocking) randomNum += 3;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 25) / 10);
                    }
                }
                else if (randomNum == 3|| randomNum == 4)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0 || randomNum == 1 || randomNum == 2) && !ForcePunch)
                {
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 25) / 10);
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 5 && Vector3.Distance(player.transform.position, transform.position) < 14)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 6);
                if (nowIsBlocking) randomNum += 1;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    ForceBlock = false;
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 15) / 10);
                }
                else if (randomNum == 1 || randomNum == 2 || randomNum == 3)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0) && !ForcePunch)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < 12)
                    {
                        ForcePunch = true;
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 15) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 6 && Vector3.Distance(player.transform.position, transform.position) < 11)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 7);
                if (nowIsBlocking) randomNum += 1;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && 1 == 0)
                {
                    ForceBlock = false;
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 15) / 10);
                }
                else if (randomNum == 1 || randomNum == 2)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (randomNum == 0)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < 12)
                    {
                        ForcePunch = true;
                        Block();
                        Invoke(nameof(Block), Random.Range(10,20) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 7 && Vector3.Distance(player.transform.position, transform.position) < 22)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 8);
                if (nowIsBlocking) randomNum += 3;

                if (Vector3.Distance(player.transform.position, transform.position) > 10)
                {
                    WindingUp = 1;
                    Invoke(nameof(WindUpPunch), 2);
                    rightAnimator.SetBool("Wind", true);
                }
                else if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(15, 20) / 10);
                    }
                }
                else if (randomNum == 2 || randomNum == 3 || randomNum == 1)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0) && !ForcePunch)
                {
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 15) / 10);
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 8 && Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 10);
                if (nowIsBlocking) randomNum += 3;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 15) / 10);
                    }
                }
                else if (randomNum == 1 || randomNum == 2 || randomNum == 3)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0) && !ForcePunch)
                {
                    Block();
                    Invoke(nameof(Block), Random.Range(10, 20) / 10);
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
            else if (MODE == 9 && Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                IsPunching = true;
                int randomNum = Random.Range(0, 8);
                if (nowIsBlocking) randomNum += 3;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if (ForceBlock && !ForcePunch)
                {
                    if (ForceBlock)
                    {
                        ForceBlock = false;
                        Block();
                        Invoke(nameof(Block), Random.Range(10, 15) / 10);
                    }
                }
                else if (randomNum == 1 || randomNum == 2)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunch();
                }
                else if ((randomNum == 0) && !ForcePunch)
                {
                    Block();
                    Invoke(nameof(Block), Random.Range(6, 16) / 10);
                }
                else
                {
                    //Left = false;
                    LeftPunch();
                }
            }
        }
        else if (!NOINPUTS && !IsPunchingAgain && MODE == 6 && Vector3.Distance(player.transform.position, transform.position) < 9)
        {
                IsPunchingAgain = true;
                int randomNum = Random.Range(0, 7);
                if (nowIsBlocking) randomNum += 1;

                if (ForceRightPunch)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunchAgain();
                }
                else if (ForceBlock && 1 == 0)
                {
                    ForceBlock = false;
                    Block2();
                    Invoke(nameof(Block2), Random.Range(10, 15) / 10);
                }
                else if (randomNum == 1 || randomNum == 2)
                {
                    ForceRightPunch = false;
                    //Right = false;
                    RightPunchAgain();
                }
                else if (randomNum == 0)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < 12)
                    {
                        ForcePunch = true;
                        Block2();
                        Invoke(nameof(Block2), Random.Range(10, 20) / 10);
                    }
                    else
                    {
                        IsPunching = false;
                    }
                }
                else
                {
                    //Left = false;
                    LeftPunchAgain();
                }
        }
    }
}
