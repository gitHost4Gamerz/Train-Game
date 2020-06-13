using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTrainUI : MonoBehaviour {

    public TrainController train;
    public ButtonIdentity buttonIdentity;
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
            case ButtonIdentity.HighSpeed:
                if (train.speedState == 2)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case ButtonIdentity.LowSpeed:
                if (train.speedState == 1)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case ButtonIdentity.NoSpeed:
                if (train.speedState == 0)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case ButtonIdentity.Brakes:
                if (train.brakeState)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case ButtonIdentity.LeftTurn:
                if (train.turnState)
                {
                    m_Image.sprite = onSprite;
                }
                else
                {
                    m_Image.sprite = offSprite;
                }
                break;

            case ButtonIdentity.RightTurn:
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
