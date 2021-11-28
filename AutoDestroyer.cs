using UnityEngine;
public class AutoDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}