using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class UITRACKTIMMY : MonoBehaviour
{
    public delegate void Enable();
    public static event Enable CanLeftPunch;
    public static event Enable CanBlockNow;
    public static event Enable CanRightPunch;
    public static event Enable newLine;
    public GameObject plr;
    public GameObject enemy;
    public Rigidbody rb;
    public RectTransform plr2;
    public RectTransform enemy2;
    private int PlayerMult = 100;
    private int EnemyMult = 100;
    public Sprite[] Numbers;
    public Image PlayerFirst;
    public Image PlayerSecond;
    public Image PlayerThird;
    public Image EnemyFirst;
    public Image EnemySecond;
    public Image EnemyThird;
    public int CurrentStep = 0;
    public TMP_Text infoText;
    public TMP_Text goalText;
    public Collider hit1;
    public Collider hit2;
    public int count = 0;
    public bool thing;
    public AudioSource buybye;
    public AudioSource music;
    public Image darknes;
    public bool canGo = true;
    private void OnEnable()
    {
        MovementTIMMY.OpPunchLeft += CountingUp;
        MovementTIMMY.OpPunchRight += lastStage;
        MovementTIMMY.MCAdd2 += Add2MC;
        MovementTIMMY.MCAdd7 += Add7MC;

        OpAITIMMY.EnAdd1 += Add1En;
        OpAITIMMY.EnAdd2 += Add2En;
        OpAITIMMY.EnAdd5 += Add5En;
    }

    private void OnDisable()
    {
        MovementTIMMY.OpPunchLeft -= CountingUp;
        MovementTIMMY.OpPunchRight -= lastStage;
        MovementTIMMY.MCAdd2 -= Add2MC;
        MovementTIMMY.MCAdd7 -= Add7MC;

        OpAITIMMY.EnAdd1 -= Add1En;
        OpAITIMMY.EnAdd2 -= Add2En;
        OpAITIMMY.EnAdd5 -= Add5En;
    }

    void Awake()
    {
        darknes.color = new Color(1, 1, 1, 1);
    }

    void Start()
    {
        Lighten();
        Invoke(nameof(Undun), 1.1f);
    }

    void Lighten()
    {
        if (canGo)
        {
            darknes.color = new Color(1, 1, 1, Mathf.Clamp(darknes.color.a - 0.01f, 0, 1));
            Invoke(nameof(Lighten), 0.01f);
        }
    }

    void Undun()
    {
        canGo = false;
        darknes.color = new Color(0, 0, 0, 0);
    }

    void lastStage()
    {
        if (CurrentStep == 4)
        {
            CurrentStep = 5;
            infoText.text = "Play around with the controls until you're ready to continue.";
            goalText.text = "Exit the box to begin the game.";
        }

        thing = false;
        Invoke(nameof(OnceMore), 1.2f);
    }
    void CountingUp()
    {
        if (CurrentStep == 2)
        {
            count += 1;
            if (count == 5)
            {
                CurrentStep = 3;
                infoText.text = "Blocking decreases how much knockback percentage you gain and prevents you from taking knockback. But it makes you much slower. Use R or Middle Mouse Button to toggle block.";
                goalText.text = "Block one of Little Timmy's punches.";
                Invoke(nameof(unBlocks), 1);
            }
            else goalText.text = "Punch Timmy a few times (" + count + " / 5)";
        }
    }
    void unBlocks()
    {
        CanBlockNow();
    }
    void Add1MC()
    {
        PlayerMult += 1;
        ChangeNumbers();
    }
    void Add2MC()
    {
        PlayerMult += 2;
        ChangeNumbers();

        if (CurrentStep == 3)
        {
            CurrentStep = 4;
            infoText.text = "Right punches are slow but they greatly knockback your opponent. Use E or Right Click to Right Punch.";
            goalText.text = "Right Punch Timmy.";
            CanRightPunch();
        }
    }
    void Add7MC()
    {
        PlayerMult += 7;
        ChangeNumbers();
    }

    void ChangeNumbers()
    {
        int hundreds = PlayerMult / 100;
        int tens = PlayerMult % 100 / 10;
        int ones = PlayerMult % 10;
        
        PlayerFirst.sprite = Numbers[hundreds];
        PlayerSecond.sprite = Numbers[tens];
        PlayerThird.sprite = Numbers[ones];
    }
    void Add1En()
    {
        EnemyMult += 1;
        ChangeOtherNumbers();
    }
    void Add2En()
    {
        EnemyMult += 2;
        ChangeOtherNumbers();
    }
    void Add5En()
    {
        EnemyMult += 5;
        ChangeOtherNumbers();
    }
    void ChangeOtherNumbers()
    {
        if (EnemyMult > 999) 
        {
            EnemyFirst.sprite = Numbers[9];
            EnemySecond.sprite = Numbers[9];
            EnemyThird.sprite = Numbers[9];
            return;
        }
        int hundreds = EnemyMult / 100;
        int tens = EnemyMult % 100 / 10;
        int ones = EnemyMult % 10;

        EnemyFirst.sprite = Numbers[hundreds];
        EnemySecond.sprite = Numbers[tens];
        EnemyThird.sprite = Numbers[ones];
    }
    void FixedUpdate()
    {
        plr2.anchoredPosition = new Vector2(plr.transform.position.x * 40f / 30f, plr.transform.position.z * 40f / 30f - 30);
        enemy2.anchoredPosition = new Vector2(enemy.transform.position.x * 40f / 30f, enemy.transform.position.z * 40f / 30f - 30);

        if (Mathf.Abs(plr.transform.position.x) < 22 && Mathf.Abs(plr.transform.position.z) < 22 && CurrentStep == 0)
        {
            CurrentStep = 1;
            infoText.text = "stay inside the box. you win by pushing your opponent outside the box. you lose if you exit the box.";
            goalText.text = "walk up to your little brother Timmy.";
            newLine();
        }

        if (hit1.bounds.Intersects(hit2.bounds) && CurrentStep == 1)
        {
            CurrentStep = 2;
            infoText.text = "The top left number is your knockback percentage, the top right number is your opponent's knockback percentage. The higher your opponent's number is, the more knockback they recieve. Left Punches greatly increase your opponent's percentage. Use Q or Left Click to Left Punch.";
            goalText.text = "Punch Timmy a few times (0 / 5)";
            CanLeftPunch();
        }

        if ((Mathf.Abs(plr.transform.position.x) > 22 || Mathf.Abs(plr.transform.position.z) > 22) && CurrentStep == 5)
        {
            CurrentStep = 6;
            buybye.Play();
            InvokeRepeating(nameof(Dark), 0.5f, 0.02f);
            Invoke(nameof(LIGHTno), 3.5f);
        }
    }

    void Dark()
    {
        darknes.color = new Color(0, 0, 0, Mathf.Clamp(darknes.color.a + 0.01f, 0, 1));
        music.volume -= 0.0015f;
    }

    void LIGHTno()
    {
        SceneManager.LoadScene("Cutscene");
    }

    void OnceMore()
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        enemy.transform.position = new Vector3(-14.5f ,2.76f,0);
        enemy.transform.eulerAngles = new Vector3(0,270,0);
        thing = true;
        Invoke(nameof(TwiceMore), 0.1f);
    }

    void TwiceMore()
    {
        if (enemy.transform.position != new Vector3(-14.5f, 2.76f, 0) && thing) 
        {
            enemy.transform.position = new Vector3(-14.5f, 2.76f, 0);
            enemy.transform.eulerAngles = new Vector3(0,270,0);
            Invoke(nameof(TwiceMore), 0.1f);
        }
    }
}
