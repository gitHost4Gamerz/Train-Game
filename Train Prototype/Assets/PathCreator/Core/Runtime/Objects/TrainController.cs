using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    // Here are our two variables - very easy to explain!
    // Turn state is a boolean because we can either go left or right. Left is true. Right is false.
    public bool turnState;
    // Speed state is an integer between 0 and 2. 0 represents no speed. 1 represents low speed. 2 represents high speed.
    public int speedState;
    // This is whether or not we're breaking. Real simple.
    public bool brakeState;

    // Start is called before the first frame update
    void Start()
    {
        turnState = true;
        brakeState = false;
        speedState = 0;
    }

    // Update is called once per frame
    void Update()
    {
    
       // Change the train's desired turn from left to right.
       if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            turnState = true;
        }

       if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            turnState = false;
        }

       // Change the train's desired speed.
       if (Input.GetKeyDown(KeyCode.UpArrow) && speedState < 2)
        {
            speedState++;
        }

       if (Input.GetKeyDown(KeyCode.DownArrow) && speedState > 0)
        {
            speedState--;
        }

       if (Input.GetKey(KeyCode.RightShift))
        {
            brakeState = true;
        }
       else
        {
            brakeState = false;
        }

    }
}
