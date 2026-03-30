using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpAITIMMY : MonoBehaviour
{
    public delegate void Hits();
    public static event Hits MePunchLeft;
    public static event Hits MePunchRight;
    public delegate void Mult();
    public static event Mult EnAdd1;
    public static event Mult EnAdd2;
    public static event Mult EnAdd5;
    public Rigidbody rb;
    public float forceRight;
    public Camera cam;
    private Vector3 positionChange;
    private Vector3 trueChange;
    public Animator leftAnimator;
    public Animator rightAnimator;
    private bool IsPunching = false;
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
    public bool NOINPUTS = false;
    public bool ForceBlock = false;
    public bool ForceRightPunch = false;
    public bool ForcePunch = false;
    public bool breakBlock = false;
    public int forcePunchCounter = 0;
    public float TIMEFROMBLOCK = 0;
    public bool CanLeftPunch = true;
    public AudioSource lefthits;
    public AudioSource righthits;
    public AudioSource swings;
    public AudioSource painSounds;
    public AudioClip[] pains;
    public int LastPainNum = 0;
    public AudioClip[] ahhhhhhhs;
    public int LastAHHHNum = 0;
    public AudioClip[] takes;
    public int TakeNum = 0;
    public bool Onoff = true;
    public AudioClip[] hiiiii;
    public int helloNum = 0;
    public int LevelSay = 0;
    public AudioClip boxin;

    private void OnEnable()
    {
        MovementTIMMY.OpPunchLeft += KnockbackLeft;
        MovementTIMMY.OpPunchRight += KnockbackRight;
        UITRACKTIMMY.CanBlockNow += LeftPunch;
        MovementTIMMY.MCAdd2 += Add2MC;
        UITRACKTIMMY.newLine += NewLevelSay;
    }

    private void OnDisable()
    {
        MovementTIMMY.OpPunchLeft -= KnockbackLeft;
        MovementTIMMY.OpPunchRight -= KnockbackRight;
        UITRACKTIMMY.CanBlockNow -= LeftPunch;
        MovementTIMMY.MCAdd2 -= Add2MC;
        UITRACKTIMMY.newLine -= NewLevelSay;
    }

    void Start()
    {
        Invoke(nameof(Resay), 0.5f);
    }

    void Resay()
    {
        if (LevelSay == 0 && helloNum != 3)
        {
            painSounds.clip = hiiiii[helloNum];
            painSounds.Play();
            helloNum += 1;
            Invoke(nameof(Resay), 4);
        }
    }

    void NewLevelSay()
    {
        LevelSay = 1;

        painSounds.Stop();
        painSounds.clip = boxin;
        painSounds.Play();
    }

    void Add2MC()
    {
        CanLeftPunch = false;
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
        knockBackMult += 7;
        EnAdd5();
        EnAdd2();
        Invoke(nameof(NoKnock), 0.5f);
        turnNormal = false;
        TurnTime();
        lefthits.Play();
        Invoke(nameof(STOPLEFt), 0.4f);
        painSounds.Stop();
        int rando = Random.Range(0, 5);
        while (rando == LastPainNum)rando = Random.Range(0, 5);
        painSounds.clip = pains[rando];
        LastPainNum = rando;
        painSounds.pitch = 1f + Random.Range(0, 11) / 100f;
        painSounds.Play();
    }

    void STOPLEFt()
    {
        lefthits.Stop();
    }
    void KnockbackRight()
    {
        bool boolyes = !EyeAnim.GetBool("ha");
        EyeAnim.SetBool("ha", boolyes);
        EyeAnim.SetBool("ah", !boolyes);
        knockBackMult += 1;
        EnAdd1();
        
        rb.AddForce(cam.transform.forward * forceRight * (knockBackMult / 100f), ForceMode.Impulse);
        //rb.AddTorque(cam.transform.forward * forceRight/10, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(10, 20), Random.Range(10, 20), Random.Range(10, 20)), ForceMode.Impulse);
        rb.useGravity = true;
        Invoke(nameof(EXtraGrav), 0.3f);
        Invoke(nameof(resettuuu), 1.2f);
        turnNormal = false;
        righthits.Play();
        //OtherTurnTime();
        Invoke(nameof(STOPRIGHt), 0.4f);
        painSounds.Stop();
        int rando = Random.Range(0, 3);
        while (rando == LastAHHHNum)rando = Random.Range(0, 3);
        painSounds.clip = ahhhhhhhs[rando];
        LastAHHHNum = rando;
        painSounds.pitch = 1f + Random.Range(0, 11) / 100f;
        painSounds.Play();
    }

    void STOPRIGHt()
    {
        righthits.Stop();
    }

    void EXtraGrav()
    {
        rb.linearVelocity = new Vector3(0, -10, 0);
    }

    void resettuuu()
    {
        turnNormal = true;
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
        if (!CanLeftPunch) return;
        swings.Play();
        leftAnimator.SetBool("LeftPunch", true);
        Invoke(nameof(leftHitbox), 0.1f);
        if (Onoff)
        {
            painSounds.Stop();
            painSounds.clip = takes[TakeNum];
            TakeNum += 1;
            if (TakeNum == 5) TakeNum = 1;
            painSounds.pitch = 1f + Random.Range(0, 11) / 100f;
            painSounds.Play();
        }
        Onoff = !Onoff;
    }

    void leftHitbox()
    {
        leftAnimator.SetBool("LeftPunch", false);
        if (hitboxLeft.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchLeft();
        }
        Invoke(nameof(LeftPunch), 1);
    }

    void rightHitbox()
    {
        if (hitboxRight.bounds.Intersects(enemyHitbox.bounds))
        {
            MePunchRight();
        }
        leftAnimator.SetBool("LeftPunch", false);
        rightAnimator.SetBool("RightPunch", false);
    }

    void NotPunch()
    {
        IsPunching = false;
    }

    void RightPunch()
    {
        IsPunching = true;
        rightAnimator.SetBool("RightPunch", true);
        Invoke(nameof(rightHitbox), 0.1f);
        Invoke(nameof(NotPunch), 1.2f);
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
    }
}
