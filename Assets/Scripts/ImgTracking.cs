using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
public class ImgTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] arObjectPrefabs;

    public List<GameObject> objList = new List<GameObject>();

    private List<GameObject> immutableList;

    public TextMesh debugger;

    private int listCount;

    private GameObject currentItem;

    private GameObject nextObj;


    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);

    private ARTrackedImageManager m_TrackedImageManager;

    //private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        // setup all game objects in dictionary
        foreach (GameObject arObject in arObjectPrefabs)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            newARObject.SetActive(false);
            objList.Add(newARObject);
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
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
           // debugger.text = trackedImage.referenceImage.name + "added";
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
            //debugger.text = trackedImage.referenceImage.name + "updated";
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {

            //May need to remove refrece iamge name thingy 

            foreach(GameObject obj in objList)
            {
                if(trackedImage.referenceImage.name == obj.name)
                {
                    obj.SetActive(false);
                }
            }
            //arObjects[trackedImage.name].SetActive(false);
           // debugger.text = trackedImage.referenceImage.name + "removed";
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        if (objList != null)
        {
            //if first obj, set as current, remove, and shuffle list
            if (listCount == objList.Count)
            {
                foreach (GameObject obj in objList)
                {
                    if (trackedImage.referenceImage.name == obj.name)
                    {
                        currentItem = obj;
                    }
                }
                objList.Remove(currentItem);
                objList = new List<GameObject>(ShuffleList(objList));
            }
            else
            {
                //if not first object and is next obj, set as current, and remove
                if (trackedImage.referenceImage.name == nextObj.name)
                {
                    foreach (GameObject obj in objList)
                    {
                        if (trackedImage.referenceImage.name == obj.name)
                        {
                            currentItem = obj;
                        }
                    }
                    objList.Remove(currentItem);
                }
            }

             nextObj = objList[0];

            debugger.text = currentItem.name + "->" + nextObj.name;

            //fix
           
                if (trackedImage.trackingState == TrackingState.Tracking)
                {

                    currentItem.SetActive(true);
                    currentItem.transform.position = trackedImage.transform.position;
                    currentItem.transform.rotation = trackedImage.transform.rotation;
                    currentItem.transform.localScale = scaleFactor;
                }
                else
                {
                    currentItem.SetActive(false);
                }
            

            /*string name = trackedImage.referenceImage.name;
            GameObject goARObject = arObjects[name];
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                goARObject.SetActive(true);
                goARObject.transform.position = trackedImage.transform.position;
                goARObject.transform.rotation = trackedImage.transform.rotation;
                goARObject.transform.localScale = scaleFactor;
            }
            else
            {
                goARObject.SetActive(false);
            }*/
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
