using UnityEngine;

public class PushAway : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public Rigidbody rb;
    public Rigidbody evilRB;
    public float mult;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "bigCube")
        {
            float dist = Vector3.Distance(player.position, enemy.position);
            rb.AddForce(-enemy.forward * Mathf.Clamp((7 - dist) * 20, 1, 999), ForceMode.Impulse);
            evilRB.AddForce(player.forward * Mathf.Clamp((7 - dist) * 20, 1, 999), ForceMode.Impulse);
        }
    }
}
