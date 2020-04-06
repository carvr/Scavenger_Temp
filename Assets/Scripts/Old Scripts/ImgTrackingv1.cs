using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
public class ImgTrackingv1 : MonoBehaviour
{
    [SerializeField]
    //  internal immutable object list
    private GameObject[] arObjectPrefabs;

    // public, mutable list to drag objects
    public List<GameObject> objList = new List<GameObject>();

    private List<GameObject> immutableList;

    public TextMesh debugger;

    public TextMesh debugger2;

    private int listCount;

    private GameObject currentObj;

    private GameObject nextObj;


    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);

    private ARTrackedImageManager m_TrackedImageManager;

    //immutable dictionary
    private Dictionary<string, GameObject> objDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        
        foreach (GameObject arObject in arObjectPrefabs)
        {
            //setup all gmae objs in list
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            newARObject.SetActive(false);
            objList.Add(newARObject);

            // setup all game objects in dictionary
            objDictionary.Add(newARObject.name, newARObject);            
        }



        immutableList = new List<GameObject>(objList);

        listCount = immutableList.Count;
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        var tempAllThings = new List<ARTrackedImage>();
        tempAllThings.AddRange(eventArgs.added);
        tempAllThings.AddRange(eventArgs.updated);
        debugger2.text = "";
        foreach (var tracked in tempAllThings)
        {
            debugger2.text += $"{tracked.referenceImage.name} is now {tracked.trackingState}\n";
        }

        /*
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
           // debugger.text = trackedImage.referenceImage.name + "added";
        }
        */

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                UpdateARImage(trackedImage);
            }
            //put the verify ifcorrect image in new function
            //debugger.text = trackedImage.referenceImage.name + "updated";
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {

            objDictionary[trackedImage.referenceImage.name].SetActive(false);

        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        if (objList != null)
        {
            //if first obj, set as current, remove, and shuffle list
            if (listCount == objList.Count)
            {

                currentObj = objDictionary[trackedImage.referenceImage.name];
                objList.Remove(currentObj);
                //debugger.text = "List CountFirstPass is " + objList.Count.ToString();
                objList = new List<GameObject>(ShuffleList(objList));
                nextObj = objList[0];
            }
            else
            {
                //if not first object and is next obj, set as current, and remove
                if (trackedImage.referenceImage.name == nextObj.name)
                {
                    currentObj = nextObj;
                    objList.Remove(currentObj);
                    nextObj = objList[0];
                    //debugger.text = "List Count is " + objList.Count.ToString(); 
                }
            }

            

            debugger.text = currentObj.name + "->" + nextObj.name;


            currentObj.SetActive(true);
            /*debugger2.text = trackedImage.referenceImage.name; */
            currentObj.transform.position = trackedImage.transform.position;
            currentObj.transform.rotation = trackedImage.transform.rotation;
            currentObj.transform.localScale = scaleFactor;
            if (trackedImage.trackingState != TrackingState.Tracking)
            {

                currentObj.SetActive(false);

            }
        }

    }

    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = Random.Range(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

}
