using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;


public class IPhoneFixPanel : MonoBehaviour
{
    public float offsetMinX;
    public float offsetMinY;
    public float offsetMaxX;
    public float offsetMaxY;
     void OnEnable()
    {
        ResetPanel();
    }


    public void ResetPanel()
    {
		#if UNITY_ANDROID
			return;
		#endif
		if ((Screen.width == 1125 && Screen.height == 2436) || (Mathf.Approximately(Camera.main.aspect, (float)(9.0f / 19.5f))))
        {
            gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(offsetMaxX, offsetMaxY);
            gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(offsetMinX, offsetMinY);

        }
    }
}
