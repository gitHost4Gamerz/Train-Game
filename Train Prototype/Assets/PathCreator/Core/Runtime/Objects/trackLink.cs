﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation
{
    public class trackLink : MonoBehaviour
    {
        // This simply stores our two track endpoints.
        public PathCreator optionA;
        public PathCreator optionB;
        bool trackChange = false;
        public PathCreator previous;
        public PathCreator next;
        public PathFollower[] trainsOnTrack = new PathFollower[20];
        public float distanceOffset = 0;

        // Happens first - setup
        void Start()
        {
            next = optionA;
        }

        // Update is called once per frame
        void Update()
        {
            // This code gives us the ability to swap between next tracks if we are not in the process of changing tracks and we have a track to change to
            if (trainsOnTrack[0] != null)
            // Consider adding "&& optionB != null." For now, we will have optionA and optionB be the same thing for tracks without branching paths.
            {
                //If we left click, we change where we're going.
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    trackChange = !trackChange;
                }

                //By default trackChange is set to false and our initial next is always set to optionA. Clicking will swap that to optionB as long as the train is currently swapping. Currently this will work for all tracks.
                if (trackChange)
                {
                    next = optionB;
                }
                else
                {
                    next = optionA;
                }
            }

            // First let's look through all the carts on the track
            for (int i = 0; i < trainsOnTrack.Length; i++)
            {
                // If we have a cart AND the track has length (to prevent instant switching on runtime), we can continue!
                if (trainsOnTrack[i] != null && gameObject.GetComponent<PathCreator>().trackLength != 0)
                {
                    if (trainsOnTrack[i].speed > 0)
                    {
                        // If this cart has travelled the distance of our track length, swap to the next track, set distance travelled to 0, and remove it from this fine array.
                        if (trainsOnTrack[i].distanceTravelled + (trainsOnTrack[i].moveCheck) >= trainsOnTrack[i].pathCreator.trackLength)
                        {
                            trainsOnTrack[i].swapTracks = true;
                        }
                    }
                    if (trainsOnTrack[i].speed < 0)
                    { 
                        if (trainsOnTrack[i].distanceTravelled + (trainsOnTrack[i].moveCheck) <= 0)
                        {
                            trainsOnTrack[i].swapTracks = true;
                        }
                    }
                }

            }
        }
    }
}