using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ListTrackedImages : MonoBehaviour
{
    [SerializeField]
    private TextMesh debugText;

    // Update is called once per frame
    void Update()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("Tracked images:");
        foreach (var trackedImage in TrackedImageInfoManager.Instance.ImagesOnScreen)
        {
            text.AppendLine(trackedImage.referenceImage.name);
        }
        debugText.text = text.ToString();
    }
}
