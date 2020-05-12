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
        public float maxSpeed = 7.5f;
        public float speedChange = 5f;
        public int timeConsistent = 0;

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
                timeConsistent = 0;
                if (!fueled)
                {
                    fueledSpeed = 0;
                }
                else
                {
                    fueledSpeed = 5f;
                }
            }

            // Calculate how long we've been fueled or unfueled
            if (fueled || FindObjectOfType<RotationCalculator>().rotationalAcceleration <= 0)
            {
                timeConsistent++;
            }

            //Calculate speedChange (WIP dynamic acceleration, aka the "simulated quadratic")
            if (timeConsistent >= 0 && timeConsistent < 175) //Small change at first
            {
                speedChange = 0.5f;
            }
            if (timeConsistent >= 250 && timeConsistent < 450) //Then a medium change after a time
            {
                speedChange = 2f;
            }
            if (timeConsistent >= 450) //Finally a large change in acceleration, this also caps timeConsistent so no memory leak
            {
                timeConsistent = 450;
                speedChange = 3f;
            }

            // Consistent frames (this might not be quite right)
            float amount = Mathf.Abs(speedChange * Time.deltaTime);

            // If we are self-propelling, accelerate until we hit max speed and then cap
            if (fueled && speed < fueledSpeed && !Input.GetKey(KeyCode.LeftShift))
            {
                speed += amount;
                if (speed > fueledSpeed)
                {
                    speed = fueledSpeed;
                }
            }

            // If we are not self-propelling, decelerate until we hit zero and then set to 0 (only if on a flat plane)
            if (!fueled && FindObjectOfType<RotationCalculator>().rotationalAcceleration == 0 && !Input.GetKey(KeyCode.LeftShift)) 
            {
                if (speed > 0)
                {
                    speed -= amount;
                    if (speed < 0.01)
                    {
                        speed = 0;
                    }
                }
                // Special case for sliding backwards when not self-propelled that makes us slow down
                else
                {
                    speed += amount;
                    if (speed > -0.01)
                    {
                        speed = 0;
                    }
                }
            }

            // Add rotational acceleration
            if (speed >= minSpeed && speed <= maxSpeed && !Input.GetKey(KeyCode.LeftShift) && (!fueled || speed > (fueledSpeed - 0.5)))
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

            // Speed control
            if (Input.GetKeyDown(KeyCode.DownArrow) && fueledSpeed > 0)
            {
                fueledSpeed--;
                // If this brings us to a halt, we are no longer fueled
                if (fueledSpeed == 0)
                {
                    fueled = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && fueledSpeed < 5)
            {
                fueledSpeed++;
                // If we are accelerate while stopped, we are now fueled
                if (!fueled)
                {
                    fueled = true;
                }
            }

            // Breaks system
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (speed != 0)
                {
                    speed = Mathf.Lerp(speed, 0, 0.01f);
                }

                if (speed <= 0.1 && speed >= -0.1)
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