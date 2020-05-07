using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5f;
        public float fueledSpeed = 5f;
        public float minSpeed = -5f;
        public float maxSpeed = 10f;
        public float speedChange = 5f;
        public int car = 1;
        public bool fueled = true;
        float distanceTravelled;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            // Check to see if train is fueled (propelling itself)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fueled = !fueled;
            }

            // Consistent frames
            float amount = Mathf.Abs(speedChange * Time.deltaTime);

            // If we are self-propelling, accelerate until we hit max speed and then cap
            if (fueled && speed < fueledSpeed)
            {
                speed += amount;
                if (speed > fueledSpeed)
                {
                    speed = fueledSpeed;
                }
            }

            // If we are not self-propelling, decelerate until we hit zero and then set to 0 (only if on a flat plane)
            if (!fueled && speed > 0 && FindObjectOfType<RotationCalculator>().rotationalAcceleration == 0) 
            {
                speed -= amount;
                if (speed < 0.01)
                {
                    speed = 0;
                }
            }

            // Add rotational acceleration
            if (speed >= minSpeed && speed <= maxSpeed)
            {
                speed += FindObjectOfType<RotationCalculator>().rotationalAcceleration;
            }

            // If we are going faster than the fueledSpeed
            if (speed >= fueledSpeed)
            {
                // If we are going faster than the maxSpeed, set to maxSpeed
                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
                // If we are level, set back to fueledSpeed after a second
                if (FindObjectOfType<RotationCalculator>().rotationalAcceleration == 0)
                {
                    speed -= amount;
                    if (speed < fueledSpeed)
                    {
                        speed = fueledSpeed;
                    }
                }
            }

            // If we are going slower than the minSpeed set back to minSpeed
            if (speed <= minSpeed)
            {
                speed = minSpeed;
            }

            // Special case for sliding backwards when not self-propelled that makes us slow down
            if (!fueled && speed <= 0 && FindObjectOfType<RotationCalculator>().rotationalAcceleration == 0)
            {
                speed += amount;
                if (speed > -0.01)
                {
                    speed = 0;
                }
            }


            //follow path
            if (pathCreator != null)
            {
                distanceTravelled += (speed * Time.deltaTime);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled - car, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled - car, endOfPathInstruction);
            }

            // Setting each train in the whole train array
            if (FindObjectOfType<RotationCalculator>() != null)
            {
                FindObjectOfType<RotationCalculator>().trains[car - 1] = this;
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}