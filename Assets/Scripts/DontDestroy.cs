using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in objectsWithTag)
        {
            if (obj != gameObject) Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1;

        // x = In Game Position
        // y = Last Oponnent Beaten
        // z = Currently Selected
    }
}
