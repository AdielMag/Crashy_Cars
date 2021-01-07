using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraViewChange : MonoBehaviour
{
    [SerializeField]
    private Slider _distanceY, _distanceZ, _fov;

    [SerializeField]
    private Text _distanceYTxt, _distanceZTxt, _fovTxt;

    [Space]
    [SerializeField]
    private CinemachineVirtualCamera _cam;

    private void Start()
    {
        SetInitialFOV();
        SetInitialDistanceY();
        SetInitialDistanceZ();
    }

    private void SetInitialFOV()
    {
        _fov.value = _cam.m_Lens.FieldOfView;

        _fovTxt.text = _cam.m_Lens.FieldOfView.ToString();
    }
    private void SetInitialDistanceY()
    {
        float target =
          _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y;

        _distanceY.value = target;

        _distanceYTxt.text = target.ToString();
    }
    private void SetInitialDistanceZ()
    {
        float target =
          _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;

        _distanceZ.value = -target;

        _distanceZTxt.text = (-target).ToString();
    }


    public void ChangeFOV(float value)
    {
        _fovTxt.text = value.ToString();

        _cam.m_Lens.FieldOfView = value;
    }
    public void ChangeDistanceY(float value)
    {
        _distanceYTxt.text = value.ToString();

        Vector3 follow = _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset
            = new Vector3(follow.x, value, follow.z);
    }
    public void ChangeDistanceZ(float value)
    {
        _distanceZTxt.text = value.ToString();

        Vector3 follow = _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        _cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset
            = new Vector3(follow.x, follow.y, -value);
    }

}
