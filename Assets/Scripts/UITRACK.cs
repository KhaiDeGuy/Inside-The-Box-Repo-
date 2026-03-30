using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITRACK : MonoBehaviour
{
    public delegate void Hits();
    public static event Hits Add50;
    public GameObject plr;
    public GameObject enemy;
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
    public Image center;
    public Image CenterISH;
    public Sprite winner;
    public Sprite looooser;
    public PlayerInput plrs;
    public int CurrentNumber;
    public Transform playerInp;
    public bool DoTHingOneTime = true;
    public int isGuyEight = 0;
    private void OnEnable()
    {
        Movement.MCAdd1 += Add1MC;
        Movement.MCAdd2 += Add2MC;
        Movement.MCAdd5 += Add5MC;

        OpAI.EnAdd1 += Add1En;
        OpAI.EnAdd2 += Add2En;
        OpAI.EnAdd5 += Add5En;
    }

    private void OnDisable()
    {
        Movement.MCAdd1 -= Add1MC;
        Movement.MCAdd2 -= Add2MC;
        Movement.MCAdd5 -= Add5MC;

        OpAI.EnAdd1 -= Add1En;
        OpAI.EnAdd2 -= Add2En;
        OpAI.EnAdd5 -= Add5En;
    }
    void Start()
    {
        CenterISH.enabled = false;
        Invoke(nameof(GoTo2CenterNum), 1f);
        playerInp = GameObject.Find("PlayerInfo").GetComponent<Transform>();
    }

    void GoTo2CenterNum()
    {
        center.sprite = Numbers[2];
        Invoke(nameof(GoTo1CenterNum), 1);
    }

    void GoTo1CenterNum()
    {
        center.sprite = Numbers[1];
        Invoke(nameof(ERASETHECENTER), 1);
    }

    void ERASETHECENTER()
    {
        center.enabled = false;
        plrs.enabled = true;
        plr.GetComponent<Movement>().enabled = true;
        enemy.GetComponent<OpAI>().enabled = true;
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
    }
    void Add5MC()
    {
        PlayerMult += 5;
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
        int hundreds = EnemyMult / 100;
        int tens = EnemyMult % 100 / 10;
        int ones = EnemyMult % 10;

        EnemyFirst.sprite = Numbers[hundreds];
        EnemySecond.sprite = Numbers[tens];
        EnemyThird.sprite = Numbers[ones];

        if (isGuyEight != 0)
        {
            if (isGuyEight * 50 + 99 < EnemyMult)
            {
                isGuyEight += 1;
                Add50();
            }
        }
    }
    void FixedUpdate()
    {
        plr2.anchoredPosition = new Vector2(plr.transform.position.x * 40f / 30f, plr.transform.position.z * 40f / 30f - 30);
        enemy2.anchoredPosition = new Vector2(enemy.transform.position.x * 40f / 30f, enemy.transform.position.z * 40f / 30f - 30);

        if (Mathf.Abs(enemy.transform.position.x) > 22 || Mathf.Abs(enemy.transform.position.z) > 22)
        {
            CenterISH.sprite = winner;
            CenterISH.enabled = true;
            Time.timeScale = 0.5f;
            plr.GetComponent<Movement>().enabled = false;
            enemy.GetComponent<OpAI>().enabled = false;
            plrs.enabled = false;
            Invoke(nameof(SendBack), 2);

            if (playerInp.position.z != 1 && DoTHingOneTime)
            {
                playerInp.position += new Vector3(0,1,0);
                DoTHingOneTime = false;
            }
        }
        else if (Mathf.Abs(plr.transform.position.x) > 22 || Mathf.Abs(plr.transform.position.z) > 22)
        {
            CenterISH.sprite = looooser;
            CenterISH.enabled = true;
            Time.timeScale = 0.5f;
            plr.GetComponent<Movement>().enabled = false;
            enemy.GetComponent<OpAI>().enabled = false;
            plrs.enabled = false;

            Invoke(nameof(SendBack), 2);
        }
    }

    void SendBack()
    {
        Time.timeScale = 1;
        if (playerInp.position.z == 0) SceneManager.LoadScene("OpMenu");
        else
        {
            playerInp.position = new Vector3(playerInp.position.x, playerInp.position.y, 0);
            SceneManager.LoadScene("StartMenu");
        }
    } 
}
