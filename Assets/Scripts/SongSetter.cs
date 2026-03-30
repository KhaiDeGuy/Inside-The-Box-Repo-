using UnityEngine;

public class SongSetter : MonoBehaviour
{
    public AudioSource music;
    public Transform playerInp;
    void Start()
    {
        playerInp = GameObject.Find("PlayerInfo").GetComponent<Transform>();
        music.time = playerInp.localScale.x;
        music.pitch = 1;
        Time.timeScale = 1;
        playerInp.localScale = Vector3.zero;
    }
}
