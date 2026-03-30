using UnityEngine;

public class CONNECTINS : MonoBehaviour
{
    public Transform thing1;
    public Transform thing2;
    public Transform thing3;
    void Update()
    {
        thing3.position = (thing1.position + thing2.position) / 2;
        thing3.localScale = new Vector3(thing3.localScale.x, Vector3.Distance(thing1.position, thing2.position) / 2, thing3.localScale.z);
        thing3.rotation = Quaternion.LookRotation(thing1.position, thing2.position);
    }
}
