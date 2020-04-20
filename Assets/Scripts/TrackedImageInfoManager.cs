using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoManager : Singleton<TrackedImageInfoManager>
{
    //new stuffs for listening, creates events to be pinged for listeners to hear
    public event Action<ARTrackedImage> onImageEnterScreen;
    public event Action<ARTrackedImage> onImageExitScreen;

    private HashSet<ARTrackedImage> _imagesOnScreen = new HashSet<ARTrackedImage>();
    public IReadOnlyCollection<ARTrackedImage> ImagesOnScreen { get { return _imagesOnScreen; } } // Note: this is still mutable if someone casts it :\


    // AR Foundation subsystem
    private ARTrackedImageManager trackedImageManager;

    // Dictionary for keeping trackedImage of last TrackingState (helps for only calling these event once)
    private Dictionary<string, TrackingState> lastStateDict = new Dictionary<string, TrackingState>();

    new protected void Awake()
    {
        base.Awake();

        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // setup all game objects in lastStateDictionary
        for (int i = 0; i < trackedImageManager.referenceLibrary.count; i++)
        {
            XRReferenceImage referenceImage = trackedImageManager.referenceLibrary[i];
            lastStateDict.Add(referenceImage.name, TrackingState.None);
        }
    }
    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //look at ARTrackedImageManager.cs Code if confused about how listeners work and what these parameters are
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // Will be handled by ARImgTransformListener
            // arObjects[trackedImage.name].SetActive(false);

            // TODO: handle this case?? go off screen or something
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            // sends out ping for imageOnScreen to all listeners and sends image name 
            // only does so if it was not being tracked before this (just came on screen)
            if (lastStateDict[imageName] != TrackingState.Tracking)
            {
                if (onImageEnterScreen != null)
                {
                    _imagesOnScreen.Add(trackedImage);
                    onImageEnterScreen(trackedImage);
                }
            }

            // Note: setting active and updating transform position is now handled by listeners, namely ARImgTransformListener
        }
        else
        {
            // sends out ping for imageOffScreen to all listeners and sends image name
            // only does so if it was being tracked before this (just went off screen)
            if (lastStateDict[imageName] == TrackingState.Tracking)
            {
                if (onImageExitScreen != null)
                {
                    _imagesOnScreen.Remove(trackedImage);
                    onImageExitScreen(trackedImage);
                }
            }

            // Note: setting inactive is now handled by listeners, namely ARImgTransformListener
        }

        // sets this image's last tracked state
        lastStateDict[imageName] = trackedImage.trackingState;
    }
}