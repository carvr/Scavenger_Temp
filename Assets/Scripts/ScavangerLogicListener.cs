using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScavangerLogicListener : MonoBehaviour
{

    //cOMMENT 

    public List<GameObject> immutableList;
    public TextMesh debugger;

    private List<GameObject> mutableList = new List<GameObject>();

    private GameObject currentObj;

    private GameObject nextObj;

    //keeps track of if an image from the event is a scavanger image or not
    private bool isScavangerImage = false;

    private Dictionary<string, GameObject> objDictionary = new Dictionary<string, GameObject>();

    // 1) set up own stuff
    private void Awake()
    {
        foreach (GameObject Obj in immutableList)
        {
            // setup all gamee objs in mutableList
            mutableList.Add(Obj);
            // setup all game objects in dictionary
            objDictionary.Add(Obj.name, Obj);
        }
    }


    // 2) Whenever script is enabled (before start, after awake)
    // sets up listeners
    private void OnEnable()
    {
        TrackedImageInfoManager.Instance.onImageEnterScreen += ImageEnterScreen;
        TrackedImageInfoManager.Instance.onImageExitScreen += ImageExitScreen;
    }

    // gets rid of listeners
    private void OnDisable()
    {
        TrackedImageInfoManager.Instance.onImageEnterScreen -= ImageEnterScreen;
        TrackedImageInfoManager.Instance.onImageExitScreen -= ImageExitScreen;
    }


    void ImageEnterScreen(ARTrackedImage trackedImage)
    {
        //checks if it is an image for the scavanger game
        foreach (GameObject obj in immutableList)
        {
            if (trackedImage.referenceImage.name == obj.name)
            {
                isScavangerImage = true;
            }
        }

        if (isScavangerImage)
        {
            //  obj, set as current, remove, and shuffle list
            if (immutableList.Count == mutableList.Count)
            {
                currentObj = objDictionary[trackedImage.referenceImage.name];
                
                // not sure if thisll work since ths list just has game objects and not images
                mutableList.Remove(currentObj);
                //debugger.text = "List CountFirstPass is " + objList.Count.ToString();
                mutableList = new List<GameObject>(ShuffleList(mutableList));
            }

        }

    }


    void ImageExitScreen(ARTrackedImage trackedImage)
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
