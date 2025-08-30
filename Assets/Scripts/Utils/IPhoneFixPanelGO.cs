using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IPHONE || UNITY_IOS
using UnityEngine.iOS;

#endif

public class IPhoneFixPanelGO : MonoBehaviour
{


    //Panel Gamobjects
    public float offsetPosX;
    public float offsetPosY;

    public bool movePosX;
    public bool movePosY;

    void Start()
    {

        //#if UNITY_IPHONE || UNITY_IOS
        if ((Screen.width == 1125 && Screen.height == 2436))
        {
            //for re-positioning gameobject in panel in x
            if (movePosX)
            {
                offsetPosX = gameObject.GetComponent<RectTransform>().localPosition.x + offsetPosX;
            }
            else
            {
                offsetPosX = gameObject.GetComponent<RectTransform>().localPosition.x;
            }

            //for re-positioning gameobject in panel in y
            if (movePosY)
            {
                offsetPosY = gameObject.GetComponent<RectTransform>().localPosition.y + offsetPosY;
            }
            else
            {
                offsetPosY = gameObject.GetComponent<RectTransform>().localPosition.y;
            }

            Debug.Log("iPhone X detected!");

            //for re-positioning gameobject in panel
            gameObject.GetComponent<RectTransform>().localPosition = new Vector2(offsetPosX, offsetPosY);
        }
        //#endif
    }

}
