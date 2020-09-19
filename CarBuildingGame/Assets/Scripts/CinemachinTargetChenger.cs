using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachinTargetChenger : MonoBehaviour
{
    [SerializeField] private EditManager edit;
    [SerializeField] private CinemachineVirtualCamera camera;

    private void Start()
    {
        edit.OnCarChenged += (car) => camera.Follow = car.transform;
    }
}
