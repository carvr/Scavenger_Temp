using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImgTransformListener : MonoBehaviour
{
    // Editor settings
    [SerializeField] private string targetImageName;

    // TODO: how should we handle scaling?
    // [SerializeField] private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);


    // State
    private Transform trackedImageTransform;
    
    private void Start()
    {
        TrackedImageInfoManager.Instance.onImageEnterScreen += (ARTrackedImage trackedImage) =>
        {
            if (trackedImage.name == targetImageName)
            {
                this.trackedImageTransform = trackedImage.transform;
                gameObject.SetActive(true);
            }
        };
        TrackedImageInfoManager.Instance.onImageExitScreen += (ARTrackedImage trackedImage) =>
        {
            if (trackedImage.name == targetImageName)
            {
                this.trackedImageTransform = null;
                gameObject.SetActive(false);
            }
        };

        // gameObject.SetActive(false);
    }

    private void Update()
    {
        if (trackedImageTransform != null)
        {
            transform.position = trackedImageTransform.position;
            transform.rotation = trackedImageTransform.rotation;
        }
    }
}
