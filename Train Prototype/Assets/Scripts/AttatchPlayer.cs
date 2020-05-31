using UnityEngine;

public class AttatchPlayer : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            other.transform.parent = null;
        }
    }
}
