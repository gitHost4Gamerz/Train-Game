using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float maxSpeed = 5;
        public float speedChange = 5f;
        public int car = 1;
        public bool fueled = true;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            //check to see if train is fueled (propelling itself)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fueled = !fueled;
            }

            //consistent frames
            float amount = Mathf.Abs(speedChange * Time.deltaTime);

            //if we are self-propelling, accelerate until we hit max speed and then cap
            if (fueled && speed < maxSpeed)
            {
                speed += amount;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }

            //if we are not self-propelling, decelerate until we hit zero and then set to 0
            if (!fueled && speed > 0)
            {
                speed -= amount;
                if (speed < 0.01)
                {
                    speed = 0;
                }
            }

            //follow screen
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
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}