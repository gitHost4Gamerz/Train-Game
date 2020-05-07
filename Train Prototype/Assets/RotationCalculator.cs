using UnityEngine;

namespace PathCreation.Examples
{
    public class RotationCalculator : MonoBehaviour
    {
        // Max amount of cars for now is 10
        public PathFollower[] trains = new PathFollower[10];
        public float rotationalAcceleration = 0;
        public float hillChange = 1;
        public float bigHillChange = 3;
        public float cRot = 0;
        // Update is called once per frame
        void Update()
        {
            //rotationalAcceleration = 0;
            for(int i = 0; i < trains.Length; i++)
            {
                if (trains[i] != null)
                {
                    //Uh oh stinky
                    cRot = trains[i].transform.rotation.x;
                    if (cRot >= 5 && cRot < 18)
                    {
                        rotationalAcceleration -= hillChange;
                        Debug.Log("Subtracting hillChange");
                    }
                    if (cRot >= 18 && cRot < 35)
                    {
                        rotationalAcceleration -= bigHillChange;
                        Debug.Log("Subtracting bigHillChange");
                    }
                    if (cRot >= -18 && cRot < -5)
                    {
                        rotationalAcceleration += hillChange;
                        Debug.Log("Adding hillChange");
                    }
                    if (cRot >= -35 && cRot < -18)
                    {
                        rotationalAcceleration += bigHillChange;
                        Debug.Log("Adding bigHillChange");
                    }
                }
            }
        }
    }
}