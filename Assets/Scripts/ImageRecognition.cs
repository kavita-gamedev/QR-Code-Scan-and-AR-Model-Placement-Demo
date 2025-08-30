using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;
public class ImageRecognition : MonoBehaviour
{
    public static ImageRecognition instance;
    public ARSessionOrigin aRSessionOrigin;
    public ARSession aRSession;
    public ARTrackedImageManager trackedImageManager;

    public GameObject characterModel;
    public GameObject QRBox;
    public Animator characterAnimator;
    private bool isTracked=false;
    public GameObject Loderfiller;
    bool startfiller = false;
    public Slider _slider;

    private void Awake()
    {
        instance = this;
        _slider.maxValue = 1;
        _slider.minValue = 0;
        //startfiller = true;
        characterAnimator.enabled = false;
        characterModel.SetActive(false);
    }


 
    float timer = 3f;
    float currenttime;
    public void Update()
    {
        if (startfiller)
        {
            if (_slider.value == _slider.maxValue)
            {
                startfiller = false;
                Loderfiller.SetActive(false);
                isTracked = true;

                InitialPlacement();
            }
            Debug.Log("_slider.value" + _slider.value);
            _slider.value += 0.1f + Time.deltaTime;
        }

    }


    //public Action m_action;

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        Debug.Log("image Trackingg changed");
        foreach (ARTrackedImage trackedImage in obj.added)
        {
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in obj.updated)
        {
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in obj.removed)
        {
            UpdateTrackedImage(trackedImage);
        }
    }

   
    public void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        Debug.Log("image Trackingg state " + trackedImage.trackingState);
        if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            if (!isTracked)
            {
                QRBox.SetActive(false);
                Loderfiller.SetActive(true);
                startfiller = true;
                characterModel.transform.position = trackedImage.transform.position;
             
            }
            else
            {
                characterModel.transform.position = trackedImage.transform.position;
                InitialPlacement();
            }
        }
        else
        {
            characterAnimator.enabled = false;
            characterModel.GetComponent<PlayVoiceover>().StopVoiceOverAudio();
            characterModel.SetActive(false);
        }
    }

    public void InitialPlacement()
    {
       
        characterModel.SetActive(true);
        characterAnimator.enabled = true;
        characterAnimator.Play("BearAnim");
    }

}