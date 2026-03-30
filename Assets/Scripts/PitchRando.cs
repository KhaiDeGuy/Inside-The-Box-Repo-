using UnityEngine;

public class PitchRando : MonoBehaviour
{
    public AudioSource rando;
    public int MIN;
    public int MAX;
    public bool once = true;
    void FixedUpdate()
    {
        if (!rando.isPlaying)
        {
            if (once)
            {
                once = false;
                rando.pitch = Random.Range(MIN / 100f, MAX / 100f);
            }
        }
        else
        {
            once = true;
        }
    }
}
