using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject successPopup;
    public GameObject errorPopup;
    public GameObject loading;

    public Button errorButton;
    public Text errorText;
    public Text errorButtonText;
    public Text successText;
    //public Text verificationtext;
    public GameObject loginloading;
    public GameObject registerloading;
    //bool startfiller;

    private void Awake()
    {
        //_slider.maxValue = 1;
        //_slider.minValue = 0;
        //startfiller = true;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {       
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        successPopup.SetActive(false);
        errorPopup.SetActive(false);
        loading.SetActive(false);
        loginloading.SetActive(false);
        registerloading.SetActive(false);
        AuthManager.instance.clearregistertext();
    }

    public void RegisterScreen() // Regester button
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        successPopup.SetActive(false);
        errorPopup.SetActive(false);
        loading.SetActive(false);
        loginloading.SetActive(false);
        registerloading.SetActive(false);
        AuthManager.instance.clearlogintext();
        errorButton.onClick.RemoveListener(RegisterScreen);
    }
    public void ClosePopupBox() // Okay button
    {
        successPopup.SetActive(false);
        errorPopup.SetActive(false);
        loading.SetActive(false);
        loginloading.SetActive(false);
        registerloading.SetActive(false);
    }
    //public Slider _slider;
    //float timer = 3f;
    //float currenttime;
    //public void Update()
    //{
    //    if (startfiller)
    //    {
    //        if(_slider.value == _slider.maxValue)
    //        {
    //            startfiller = false;
    //        }
    //        Debug.Log("_slider.value" + _slider.value);
    //        _slider.value += 0.1f + Time.deltaTime;
    //    }
        
    //}


    public void SuccessPopupMessage(string successMessage)
    {
        successPopup.SetActive(true);
        successText.text = successMessage;
    }

    public void ErrorPopupMessage(string successMessage, string buttonMessage)
    {
        errorPopup.SetActive(true);
        errorText.text = successMessage;
        errorButtonText.text = buttonMessage;
    }

    //public void Authverification(bool _emailsent , string _email , string _output)
    //{
    //    loginUI.SetActive(false);
    //    registerUI.SetActive(false);
    //    successPopup.SetActive(true);
    //    errorPopup.SetActive(false);

    //    if (_emailsent)
    //    {
    //        successText.text = "Please check your email to verify.\n"  + _email;
    //    }
    //    else
    //    {
    //        successText.text = _output;
    //    }
    //}

    public void SuccessOkayClick()
    {
        LoginScreen();
    }

    public void ErrorOkayClick()
    {
        ClosePopupBox();
    }

    public void ErrorButtonToRegister()
    {
        errorButton.onClick.AddListener(RegisterScreen);        
    }
       
}
