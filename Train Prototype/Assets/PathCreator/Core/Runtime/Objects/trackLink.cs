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

        // Update is called once per frame
        void Update()
        {

            for (int i = 0; i < trainsOnTrack.Length; i++)
            {
                if (trainsOnTrack[i] != null)
                {
                    if (trainsOnTrack[i].distanceTravelledOnCurrentTrack >= gameObject.GetComponent<PathCreator>().trackLength)
                    {
                        Debug.Log("Swappin!");
                        trainsOnTrack[i].pathCreator = next;
                        trainsOnTrack[i].distanceTravelledOnCurrentTrack = 0;
                        trainsOnTrack[i].distanceTravelled = 0;
                        trainsOnTrack[i] = null;
                    }
                }
            }
    
        }
    }
}