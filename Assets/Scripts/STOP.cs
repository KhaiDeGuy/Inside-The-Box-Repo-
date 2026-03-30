using UnityEngine;
using UnityEngine.SceneManagement;

public class STOP : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        Invoke(nameof(NEWSCENE), 34);
    }
    void NEWSCENE()
    {
        SceneManager.LoadScene("OpMenu");
    }
    void LateUpdate()
    {
        transform.position = new Vector3(0, 0.75f, 1.25f);
    }
}
