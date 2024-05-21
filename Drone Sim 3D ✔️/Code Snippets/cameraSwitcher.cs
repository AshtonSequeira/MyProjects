using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//This script is used to switch between the 3 virtual cameras

public static class cameraSwitcher
{
    static List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera _activeCamera = null;

    public static void SwitchCamera(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        _activeCamera = cam;

        foreach(CinemachineVirtualCamera c in _cameras) 
        {
            if (c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera cam)
    {
        _cameras.Add(cam);
    }

    public static void UnRegister(CinemachineVirtualCamera cam)
    {
        _cameras.Remove(cam);

    }
}
