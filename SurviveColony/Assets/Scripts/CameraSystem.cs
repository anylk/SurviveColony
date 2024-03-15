using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem instance;

    private const int enableCameraPriority = 1;
    private const int disableCameraPriority = -1;

    [SerializeField]
    private CinemachineVirtualCamera topDownCamera;
    [SerializeField]
    private CinemachineVirtualCamera topDownAimCamera;

    private List<CinemachineVirtualCamera> cameras;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cameras = new List<CinemachineVirtualCamera>();

        cameras.Add(topDownCamera);
        cameras.Add(topDownAimCamera);

    }

    public void EnableCamera(CinemachineVirtualCamera _camera)
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            if (camera == _camera)
            {
                camera.Priority = enableCameraPriority;
                continue;
            }
            camera.Priority = disableCameraPriority;
        }
    }

    public void EnableAnimCamera(bool isEnable)
    {
        if (isEnable)
        {
            EnableCamera(topDownAimCamera);
        }
        else
        {
            EnableCamera(topDownCamera);
        }
    }
}
