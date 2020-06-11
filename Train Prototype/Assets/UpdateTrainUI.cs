using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTrainUI : MonoBehaviour {

    public TrainController train;
    public string buttonIdentity;
    private Image m_Image;
    //Set in editor
    public Sprite onSprite;
    public Sprite offSprite;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (buttonIdentity)
        {
            case "High Speed":
                if (train.speedState == 2)
                {
                    m_Image.sprite = onSprite;               
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case "Low Speed":
                if (train.speedState == 1)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case "No Speed":
                if (train.speedState == 0)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case "Brakes":
                if (train.brakeState)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case "Left Turn":
                if (train.turnState)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case "Right Turn":
                if (!train.turnState)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

        }
    }
}
