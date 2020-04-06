using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScavangerLogicListener : MonoBehaviour
{

    //cOMMENT 

    // is listening to the imageTracking Script
    [SerializeField]
    private TrackedImageInfoMultipleManager m_ImageTrackingScript;


    // 1) set up own stuff
    private void Awake()
    {
        m_ImageTrackingScript = GetComponent<TrackedImageInfoMultipleManager>();
        
    }


    // 2) Whenever script is enabled (before start, after awake)
    // sets up listeners
    private void OnEnable()
    {
        m_ImageTrackingScript.imageOnScreen += ImageOnScreen;
        m_ImageTrackingScript.imageOffScreen += ImageOffScreen;
    }

    // gets rid of listeners
    private void OnDisable()
    {
        m_ImageTrackingScript.imageOnScreen -= ImageOnScreen;
        m_ImageTrackingScript.imageOffScreen -= ImageOffScreen;
    }
    

    void ImageOnScreen(ARTrackedImage trackedImage) {

    }


    void ImageOffScreen(ARTrackedImage trackedImage)
    {

    }

    // Start is called before the first frame update
    // 3) used for talking to other scripts
    void Start()
    {
        
    }


    // 4++) Update is called once per frame
    void Update()
    {
        
    }
}
