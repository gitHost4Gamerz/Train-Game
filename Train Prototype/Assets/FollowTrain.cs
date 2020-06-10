using UnityEngine;

public class FollowTrain : MonoBehaviour
{

    public Transform train;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        transform.position = train.position + offset;
        transform.LookAt(train);
    }
}
