using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
// using FirebaseAnalytics;
// using Firebase.Analytics.FirebaseAnalytics;
using Firebase.Analytics;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Text loginEmailMessage;
    public Text loginPasswordMessage;

    //Register variables
    [Header("Register")]
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    //public InputField passwordRegisterVerifyField;
    public Text registerNameMessage;
    public Text registerEmailMessage;
    public Text registerPasswordMessage;
    public Button passwordVisiblity;
    public Button login_passwordVisiblity;
    public Sprite[] visibleIcon;
    private bool isPasswordVisible = false;
    private bool isloginPasswordVisible = false;

    [Header("InputField Sprite")]
    public Sprite correctInputFieldBG;
    public Sprite invalidInputFieldBG;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        defaultregiste_password_visible();
        defaultlogin_password_visible();
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        //    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //});
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void Start()
    {
        emailLoginField.onValueChanged.AddListener(delegate { ClearInputfield(emailLoginField, loginEmailMessage); });
        passwordLoginField.onValueChanged.AddListener(delegate { ClearInputfield(passwordLoginField, loginPasswordMessage); });
        usernameRegisterField.onValueChanged.AddListener(delegate { ClearInputfield(usernameRegisterField, registerNameMessage); });
        emailRegisterField.onValueChanged.AddListener(delegate { ClearInputfield(emailRegisterField, registerEmailMessage); });
        passwordRegisterField.onValueChanged.AddListener(delegate { ClearInputfield(passwordRegisterField, registerPasswordMessage); });
    }

    public void ClearInputfield(InputField input, Text warning)
    {
        input.image.sprite = correctInputFieldBG;
        warning.text = "";
    }


    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;

       Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
    }


    //Function for the login button
    public void LoginButton()
    {
       
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
       
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //UIManager.instance.loading.SetActive(true);
        UIManager.instance.loginloading.SetActive(true);
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            Debug.Log("Login errorCode" + errorCode);
            UIManager.instance.errorText.text =  "Login Failed!";
            UIManager.instance.loginloading.SetActive(false);
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    loginEmailMessage.text = "Missing Email";
                    emailLoginField.image.sprite = invalidInputFieldBG;
                    break;
                case AuthError.MissingPassword:
                    loginPasswordMessage.text = "Missing Password";
                    passwordLoginField.image.sprite = invalidInputFieldBG;
                    break;
                case AuthError.WrongPassword:
                    loginPasswordMessage.text = "Wrong Password";
                    passwordLoginField.image.sprite = invalidInputFieldBG;
                    break;
                case AuthError.InvalidEmail:
                    loginEmailMessage.text = "Invalid Email";
                    emailLoginField.image.sprite = invalidInputFieldBG;
                    break;
                case AuthError.UserNotFound:
                    UIManager.instance.ErrorPopupMessage("Account does not exist", "Click here to register");
                    UIManager.instance.ErrorButtonToRegister();
                    break;
            }
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            //UIManager.instance.successPopup.SetActive(false);
            //UIManager.instance.successText.text = "Logged In";
            Debug.Log("login email" + _email + "User.Email" + User.Email);
           
           if( User.IsEmailVerified){
                Firebase.Analytics.FirebaseAnalytics.LogEvent("log_in_event", new Parameter("email", User.Email));

                //RestAPIModule.RestAPIManager.Instance.Savelogindetail(User.Email, (response) =>
                //           {
                //               RestAPIModule.RestAPIManager.Instance.qr_list = response;
                //               loadScene();
                //           //Successfull
                //       }, (response) =>
                //       {
                //           //Failed
                //       });
                //Savelogindetail()
                //loadScene();
                loadScene();
            }
            else
            {
       
                //UIManager.instance.Authverification(false, User.Email, "Email is not Verified ");
                StartCoroutine(Sendverificationmail());
                Debug.LogFormat("email is not verified");
            }
           
        }
        //UIManager.instance.loading.SetActive(false);
    }

    public void loadScene()
    {
        //Game.Setgoogleid(GameVariables.googleid, "g07951427400425455435");
        //PlayerPrefs.DeleteKey(GameVariables.googleid);
        SceneManager.LoadScene("SoftiSceneVideo");
        //SceneManager.LoadScene("SoftiScene");
    }


    private IEnumerator Register(string _email, string _password, string _username)
    {
        UIManager.instance.registerloading.SetActive(true);
        //UIManager.instance.loading.SetActive(true);
        if (_username == "")
        {
            //If the username field is blank show a warning
            registerEmailMessage.text = "Missing Name";
            usernameRegisterField.image.sprite = invalidInputFieldBG;
        }
        //else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        //{
        //    //If the password does not match show a warning
        //    warningRegisterText.text = "Password Does Not Match!";
        //}
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                UIManager.instance.errorText.text = "Register Failed!";
                Debug.Log("RegisterTask errorCode" + errorCode);
                UIManager.instance.registerloading.SetActive(false);
                switch (errorCode)
                {
                    case AuthError.InvalidEmail:
                        registerEmailMessage.text = "Invalid Email";
                        emailRegisterField.image.sprite = invalidInputFieldBG;
                        break;
                    case AuthError.MissingEmail:
                        registerEmailMessage.text = "Missing Email";
                        emailRegisterField.image.sprite = invalidInputFieldBG;
                        break;
                    case AuthError.MissingPassword:
                        registerPasswordMessage.text = "Missing Password";
                        passwordRegisterField.image.sprite = invalidInputFieldBG;
                        break;
                    case AuthError.WeakPassword:
                        registerPasswordMessage.text = "Weak Password";
                        passwordRegisterField.image.sprite = invalidInputFieldBG;
                        break;
                    case AuthError.EmailAlreadyInUse:
                        UIManager.instance.ErrorPopupMessage("Email Already In Use", "Click here to try again");
                        break;
                }
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        UIManager.instance.registerloading.SetActive(false);
                        UIManager.instance.ErrorPopupMessage("Username Set Failed!", "Click here to try again");

                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        //RestAPIModule.RestAPIManager.Instance.Saveregisterdetail(User.Email, _username , (response) =>
                        //{
                        //    RestAPIModule.RestAPIManager.Instance.qr_list = response;
                        //    clearlogintext();
                        //    StartCoroutine(Sendverificationmail());
                        //    //Successfull
                        //}, (response) =>
                        //{
                        //    //noInternetPopup.SetNoInternetPopup("Try again", GameVariables.ALERT_NO_INTERNET, () =>
                        //    //{
                        //    //    SceneManager.LoadScene("loading");
                        //    //});
                        //    //Failed
                        //});
                        StartCoroutine(Sendverificationmail());
                        Debug.Log("register _username" + _username + "email"+ _email);
                        Firebase.Analytics.FirebaseAnalytics.LogEvent("register_event", new Parameter("username", _username), new Parameter("email", _email));

                    }
                }
            }
        }

        //UIManager.instance.loading.SetActive(false);
       
    }    

    public void clearlogintext()
    {
        Debug.Log("clearlogintext");
        emailLoginField.text = "";
        passwordLoginField.text = "";
        ClearInputfield(emailLoginField, loginEmailMessage);
        ClearInputfield(passwordLoginField, loginPasswordMessage);
        defaultlogin_password_visible();
    }


    public void clearregistertext()
    {
        Debug.Log("clearregistertext");
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        ClearInputfield(usernameRegisterField, registerNameMessage);
        ClearInputfield(emailRegisterField, registerEmailMessage);
        ClearInputfield(passwordRegisterField, registerPasswordMessage);
        //passwordRegisterVerifyField.text = "";
        defaultregiste_password_visible();
    }

    public void defaultregiste_password_visible()
    {
        passwordVisiblity.image.sprite = visibleIcon[0];
        passwordRegisterField.contentType = InputField.ContentType.Password;
        isPasswordVisible = false;

    }


    public void defaultlogin_password_visible()
    {
        login_passwordVisiblity.image.sprite = visibleIcon[0];
        passwordLoginField.contentType = InputField.ContentType.Password;
        isloginPasswordVisible = false;

    }


    public void ShowHidePassword()
    {
        if (!isPasswordVisible)
        {
            passwordVisiblity.image.sprite = visibleIcon[1];
            passwordRegisterField.contentType = InputField.ContentType.Standard;
            isPasswordVisible = true;            
        }
        else
        {
            passwordVisiblity.image.sprite = visibleIcon[0];
            passwordRegisterField.contentType = InputField.ContentType.Password;
            isPasswordVisible = false;
        }
        passwordRegisterField.ActivateInputField();
    }


    public void ShowloginHidePassword()
    {
        if (!isloginPasswordVisible)
        {
            login_passwordVisiblity.image.sprite = visibleIcon[1];
            passwordLoginField.contentType = InputField.ContentType.Standard;
            isloginPasswordVisible = true;
        }
        else
        {
            login_passwordVisiblity.image.sprite = visibleIcon[0];
            passwordLoginField.contentType = InputField.ContentType.Password;
            isloginPasswordVisible = false;
        }
        passwordLoginField.ActivateInputField();
    }

    private IEnumerator Sendverificationmail()
    {
        if(User != null)
        {
            Debug.Log("Sendverificationmail user is not null");
            var emailTask = User.SendEmailVerificationAsync();
            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);
           
            if(emailTask.Exception != null)
            {
                FirebaseException firebaseEx = emailTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Unknown Error Try Again !";
                clearregistertext();
                switch (errorCode)
                {
                    case AuthError.Cancelled:
                        message = "Verification Task was cancelled";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        registerEmailMessage.text = "Invalid Email";
                        emailRegisterField.image.sprite = invalidInputFieldBG;
                        message = "Invalid Mail ID";
                        break;
                    case AuthError.TooManyRequests:
                        message = "To many Requests";
                        break;
                   
                }
                UIManager.instance.loginloading.SetActive(false);
                UIManager.instance.registerloading.SetActive(false);
                //warningRegisterText.text = message;
                Debug.Log("email verification message" + message);                
                UIManager.instance.ErrorPopupMessage(message, "Click here to try again");
            }
            else
            {
                clearregistertext();
                UIManager.instance.loginloading.SetActive(false);
                UIManager.instance.registerloading.SetActive(false);
                UIManager.instance.SuccessPopupMessage("Please check your email to verify.");
                Debug.Log("Email sent succesfully");
            }
        }
        else
        {
            Debug.Log("Sendverificationmail user is null");
        }

        //UIManager.instance.loading.SetActive(false);
       
    }

}
