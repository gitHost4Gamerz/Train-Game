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

            if (gameObject.GetComponent<PathCreator>().atEnd)
            {
                Debug.Log("AtEnd");

                for (int I = 0; I < trainsOnTrack.Length; I++)
                {
                    // Once we find our first cart, we're going to set his pathCreator to "next," so he'll move to the next track.
                    if (trainsOnTrack[I] != null)
                    {
                        trainsOnTrack[I].pathCreator = next;
                        //remove that cart from the array
                        trainsOnTrack[I] = null;
                        break;
                    }
                }

            }
    
        }
    }
}