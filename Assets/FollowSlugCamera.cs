using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowSlugCamera : MonoBehaviour
{
    [SerializeField] GameObject slugsParent;
    CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.enabled = false;
    }

    private void Update()
    {
        if(slugsParent.transform.childCount > 0 && !virtualCamera.isActiveAndEnabled)
        {
            //virtualCamera.Follow = slugsParent.transform.GetChild(0);
            virtualCamera.enabled = true;
        }
        else if(slugsParent.transform.childCount == 0 && virtualCamera.isActiveAndEnabled)
        {
            //virtualCamera.Follow = null;
            virtualCamera.enabled = false;
        }
    }
}
