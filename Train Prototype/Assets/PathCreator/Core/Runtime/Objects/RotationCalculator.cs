using UnityEngine;

namespace PathCreation
{
    public class RotationCalculator : MonoBehaviour
    {
        // Max amount of cars for now is 10
        public PathFollower[] trains = new PathFollower[20];
        public float rotationalAcceleration = 0;
        public float hillChange = 0.1f;
        public float bigHillChange = 0.2f;
        public float cRot = 0;
        public int numOfTrains = 0;

        // Update is called once per frame
        void Update()
        {
            numOfTrains = 0;
            rotationalAcceleration = 0;
            for (int i = 0; i < trains.Length; i++)
            {
                if (trains[i] != null)
                {

                    numOfTrains++;

                    cRot = trains[i].transform.localRotation.eulerAngles.x;

                    if (cRot >= 5 && cRot < 18)
                    {
                        rotationalAcceleration += (hillChange * Time.deltaTime);
                    }
                    if (cRot >= 18 && cRot < 60)
                    {
                        rotationalAcceleration += (bigHillChange * Time.deltaTime);
                    }
                    if (cRot >= 342 && cRot < 355)
                    {
                        rotationalAcceleration -= (hillChange * Time.deltaTime);
                    }
                    if (cRot >= 300 && cRot < 342)
                    {
                        rotationalAcceleration -= (bigHillChange * Time.deltaTime);
                    }
                }
            }
        }
    }
}