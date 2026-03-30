using UnityEngine;

public class thing : MonoBehaviour
{
    public AudioSource strum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strum.time = 6.3f;
        strum.Play();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
