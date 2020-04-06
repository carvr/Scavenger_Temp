using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerLogic : MonoBehaviour

{

    //Debug Text
    public TextMesh debugger;

    public TextMesh ListText; 

    //Random 
    //Random r = new Random(); 

    public List<GameObject> testingObjs = new List<GameObject>();

    private List<GameObject> immutableList;

    private int listCount;

    private GameObject firstItem; 

    // Start is called before the first frame update
    void Start()
    {
        immutableList = new List<GameObject>(testingObjs);
        listCount = immutableList.Count;
        firstItem = testingObjs[Random.Range(0, testingObjs.Count)];
    }

    // Update is called once per frame
    void Update()
    {

       
 
        if (Input.GetKeyDown("space"))
        {
         
            if(testingObjs.Count != 1)
            {

                testingObjs.Remove(firstItem);

                if (testingObjs.Count == listCount - 1)
                {
                    testingObjs = new List<GameObject>(ShuffleList(testingObjs));
                }


                GameObject nextObj = testingObjs[0];

                debugger.text = "You have found " + firstItem.name + " Now go to " + nextObj.GetComponent<ObjClue>().clue;

                firstItem = nextObj;

            }
            else
            {
                debugger.text = "You Win"; 
            }
          

        }

        ListText.text = ShowList(testingObjs);


    }

    private string ShowList<E>(List<E> inputList)
    {
        string result = "";
        foreach (GameObject temp in testingObjs)
        {
            result += temp.name + " ";

        }
        return result; 

    }


    //List Shuffler -> from http://www.vcskicks.com/randomize_array.php

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
