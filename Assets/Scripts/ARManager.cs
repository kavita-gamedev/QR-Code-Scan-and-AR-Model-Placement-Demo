using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;
public class ARManager : MonoBehaviour
{
    public static ARManager instance;
    public ARSessionOrigin aRSessionOrigin;
    public ARSession aRSession;
    public ARTrackedImageManager trackedImageManager;

    public GameObject QRBox;
    private bool isTracked = false;
    public GameObject Loderfiller;
    bool startfiller = false;
    public Slider _slider;

    public VideoPlayer videoPlayer;
    public GameObject videoScreen;

    private void Awake()
    {
        instance = this;
        _slider.maxValue = 1;
        _slider.minValue = 0;
        //startfiller = true;
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

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    public void StartAR()
    {
        aRSession.gameObject.SetActive(true);
        // aRSessionOrigin.gameObject.SetActive(true);
    }
    public void StopAR()
    {
        aRSession.gameObject.SetActive(false);
        // aRSessionOrigin.gameObject.SetActive(false);
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
                if (!videoPlayer.isPlaying)
                {
                    videoScreen.transform.SetParent(trackedImage.transform);
                    InitialPlacement();
                }
            }
            else
            {
                if (!videoPlayer.isPlaying)
                {
                    videoScreen.transform.SetParent(trackedImage.transform);
                    InitialPlacement();
                }
            }
        }
        else
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
                videoScreen.transform.parent = null;
                videoScreen.SetActive(false);
            }
        }
    }

    public void InitialPlacement()
    {
        videoPlayer.Play();        
        videoScreen.transform.localPosition = Vector3.zero;
        videoScreen.transform.localRotation = Quaternion.identity;
        videoScreen.SetActive(true);
    }
}
