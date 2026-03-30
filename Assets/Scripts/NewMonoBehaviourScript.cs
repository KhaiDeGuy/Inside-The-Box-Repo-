using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int Modes = 0;
    public Image darknes;
    public bool InMenu = false;
    public Transform playerInp;
    public float NumberTrack;
    public string[] Names;
    public string[] Descrips;
    public Sprite[] OpImages;
    public Sprite[] BackGrounds;
    public TextMeshProUGUI[] NameSet;
    public TextMeshProUGUI DescripSet;
    public Image Imageset;
    public Image Background;
    public Animator slider;
    public int extra;
    public Image remove;
    public bool removable = false;
    public AudioSource music;
    public GameObject inactive;

    void Start()
    {
        playerInp = GameObject.Find("PlayerInfo").GetComponent<Transform>();

        if (playerInp.position.x != -1 && removable)
        {
            remove.enabled = false;
        }

        if (InMenu) 
        {
            if (playerInp.position.z != 0) NumberTrack = playerInp.position.z;
            else NumberTrack = playerInp.position.y + 1;

            if (NumberTrack == 10)
            {
                inactive.SetActive(true);
                playerInp.position = new Vector3(-1, 0, 0);
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                NameSet[i].text = Names[Mathf.RoundToInt(NumberTrack) - 1];

                if (NumberTrack > 2) 
                {
                    NameSet[i].fontSize = 70;
                    if (NumberTrack == 5 || NumberTrack == 6) NameSet[i].fontSize = 60;
                }
            }
            DescripSet.text = Descrips[Mathf.RoundToInt(NumberTrack) - 1];
            if (NumberTrack == 8) DescripSet.fontSize = 18;
            Imageset.sprite = OpImages[Mathf.RoundToInt(NumberTrack) - 1];

            if (NumberTrack > 6) 
            {
                Background.sprite = BackGrounds[1];
            }
            else if (NumberTrack > 3) 
            {
                Background.sprite = BackGrounds[0];
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ACTVATON()
    {
        if (InMenu)
        {
            if (NumberTrack == 1) SceneManager.LoadScene("TriBox1");
            else if (NumberTrack == 2) SceneManager.LoadScene("TriBox2");
            else if (NumberTrack == 3) SceneManager.LoadScene("TriBox3");
            else if (NumberTrack == 4) SceneManager.LoadScene("CircBox1");
            else if (NumberTrack == 5) SceneManager.LoadScene("CircBox2");
            else if (NumberTrack == 6) SceneManager.LoadScene("CircBox3");
            else if (NumberTrack == 7) SceneManager.LoadScene("BoxBox1");
            else if (NumberTrack == 8) SceneManager.LoadScene("BoxBox2");
            else if (NumberTrack == 9) SceneManager.LoadScene("BoxBox3");
        }
        else
        {
            if (Modes == 1)
            {
                darknes.enabled = true;
                InvokeRepeating(nameof(Lighten), 0.01f, 0.01f);
                Invoke(nameof(NewScene1), 1.5f);
            }
            else if (Modes == 2)
            {
                slider.SetInteger("mode", 0);
            }
            else if (Modes == 3)
            {
                slider.SetInteger("mode", 1);
            }
            else if (Modes == 4)
            {
                slider.SetInteger("mode", 2);
            }
            else if (Modes == 5)
            {
                slider.SetInteger("mode", 3);
            }
            else if (Modes == 6)
            {
                playerInp.position = new Vector3(playerInp.position.x, playerInp.position.y, extra);
                playerInp.localScale = new Vector3(music.time,0,0);
                SceneManager.LoadScene("OpMenu");
            }
            else if (Modes == 7) 
            {
                playerInp.localScale = new Vector3(music.time,0,0);
                playerInp.position = new Vector3(playerInp.position.x, playerInp.position.y, 0);
                SceneManager.LoadScene("StartMenu");
            }
        }
    }

    void Lighten()
    {
        darknes.color = new Color(1, 1, 1, Mathf.Clamp(darknes.color.a + 0.01f, 0, 1));
    }

    void NewScene1()
    {
        if (playerInp.position.x == -1)
        {
            playerInp.position = new Vector3(0, 0, 0);
            SceneManager.LoadScene("Timmy");
        }
        else
        {
            playerInp.localScale = new Vector3(music.time,0,0);
            SceneManager.LoadScene("OpMenu");
        }
    }
}
