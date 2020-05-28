using UnityEngine;

namespace PathCreation
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public PathCreator currentPath;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 1f;
        public float fueledSpeed = 1f;
        public float minSpeed = -1f;
        public float maxSpeed = 1.5f;
        public float speedChange;
        public float speedChange1 = 0.1f;
        public float speedChange2 = 0.2f;
        public float speedChange3 = 0.3f;
        public float timeConsistent = 0;
        public RotationCalculator wholeTrain;
        public int car;
        public bool initialized = false;
        public bool swapTracks = false;
        public float amount;
        public float moveCheck = 0;
        public float distanceOffset = 0;

        public float distanceTravelled = 0;

        public bool fueled = true;
        public float currentTrackLength;

        void Start()
        {
            // Set ourselves in our wholeTrain's trains array at the start - asap
            wholeTrain.trains[car] = this;

            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

        }

        void Update()
        {

            // Initial Update - sets our trains in their appropriate positions as soon as possible.
            if (!initialized)
            {
                // Tragically, I haven't come up with a way for numOfTrains to be filled before this script ever runs.
                // So this is how we do it. As soon as numOfTrains is filled, we put the carts where they need to go on the track. This causes a one frame delay, but I have no other ideas. 
                if (wholeTrain.numOfTrains != 0)
                {
                    //Debug.Log("Number of trains: " + wholeTrain.numOfTrains);
                    //Debug.Log("Car Number: " + car);
                    distanceTravelled = wholeTrain.numOfTrains - (car + 1);
                    //Debug.Log("Distance travelled for car " + car + ": " + distanceTravelled);
                    initialized = true;
                }
            }

            // This code swaps us to another track if specified in trackLink (scary, hope it works)!
            if (swapTracks)
            {
                if (speed > 0)
                {
                    Debug.Log("Swappin!");
                    swapTracks = false;
                    distanceOffset = distanceTravelled - pathCreator.trackLength;
                    pathCreator.GetComponent<trackLink>().trainsOnTrack[car] = null;
                    pathCreator = pathCreator.GetComponent<trackLink>().next;
                    distanceTravelled = distanceOffset;

                }
                if (speed < 0)
                {
                    Debug.Log("Swappin... backwards!");
                    swapTracks = false;
                    distanceOffset = pathCreator.GetComponent<trackLink>().previous.trackLength + distanceTravelled;
                    pathCreator.GetComponent<trackLink>().trainsOnTrack[car] = null;
                    pathCreator = pathCreator.GetComponent<trackLink>().previous;
                    distanceTravelled = distanceOffset;
                }
            }

            // Not 100% sure why this code is here... great. Looks important, though.
            if (pathCreator != null)
            {
                currentTrackLength = pathCreator.trackLength;
            }

            //Check to see if we have changed paths
            if (currentPath != pathCreator)
            {
                pathCreator.GetComponent<trackLink>().trainsOnTrack[car] = this;
                currentPath = pathCreator;
            }


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
            if (fueled || wholeTrain.rotationalAcceleration <= 0)
            {
                timeConsistent += Time.deltaTime;
            }

            //Calculate speedChange (WIP dynamic acceleration, aka the "simulated quadratic")
            if (timeConsistent >= 0 && timeConsistent < 3) //Small change at first
            {
                speedChange = speedChange1;
            }
            if (timeConsistent >= 3 && timeConsistent < 7) //Then a medium change after a time
            {
                speedChange = speedChange2;
            }
            if (timeConsistent >= 7) //Finally a large change in acceleration, this also caps timeConsistent so no memory leak
            {
                timeConsistent = 7;
                speedChange = speedChange3;
            }

            // Consistent frames (this might not be quite right)
            amount = Mathf.Abs(speedChange * Time.deltaTime);

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
            if (!fueled && wholeTrain.rotationalAcceleration == 0 && !Input.GetKey(KeyCode.LeftShift))
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
            else if (speed >= minSpeed && speed <= maxSpeed && !Input.GetKey(KeyCode.LeftShift) && (!fueled || speed > (fueledSpeed - 0.1)))
            {
                speed += wholeTrain.rotationalAcceleration;
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
                if (wholeTrain.rotationalAcceleration == 0)
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
            if (pathCreator != null && initialized)
            {
                moveCheck = (speed * Time.deltaTime);
                distanceTravelled += (moveCheck);

                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

            }

        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            //distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);

        }
    }
}