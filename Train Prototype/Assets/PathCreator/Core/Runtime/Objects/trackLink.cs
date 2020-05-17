using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation
{
    public class trackLink : MonoBehaviour
    {
        // This simply stores our two track endpoints.
        public PathCreator previous;
        public PathCreator next;
        public PathFollower[] trainsOnTrack = new PathFollower[20];
        public bool swapTracks = false;

        // Update is called once per frame
        void Update()
        {
            // First let's look through all the carts on the track
            for (int i = 0; i < trainsOnTrack.Length; i++)
            {
                // If we have a cart AND the track has length (to prevent instant switching on runtime), we can continue!
                if (trainsOnTrack[i] != null && gameObject.GetComponent<PathCreator>().trackLength != 0)
                {
                    // If this cart has travelled the distance of our track length, swap to the next track, set distance travelled to 0, and remove it from this fine array.
                    if (trainsOnTrack[i].distanceTravelled >= gameObject.GetComponent<PathCreator>().trackLength)
                    {
                        Debug.Log("Swappin!");
                        trainsOnTrack[i].pathCreator = next;
                        trainsOnTrack[i].distanceTravelled = 0;
                        trainsOnTrack[i] = null;
                        swapTracks = false;
                        break;  

                    }
                }
            }  
        }
    }
}