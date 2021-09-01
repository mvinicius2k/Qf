using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CMCameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera cmVirtualCam;
        private CinemachineComposer cmComposer;
        private CinemachineTrackedDolly cmTrackedDolly;

        public CinemachineComposer CmComposer { get => cmComposer;}
        

        public void SetDeadZoneHeight(float newHeight)
        {
            cmComposer.m_DeadZoneHeight = newHeight;
            
        }

        public void SetDeadZoneWidth(float newWidth)
        {
            cmComposer.m_DeadZoneWidth = newWidth;

        }

        public void SetXDamping(float value)
        {
            cmTrackedDolly.m_XDamping = value;
        }

        public void SetAutoDollyEnabled(bool value)
        {
            cmTrackedDolly.m_AutoDolly.m_Enabled = value;
        }

        public void SetDollyTrack(CinemachineSmoothPath cmSmoothPath)
        {
            cmTrackedDolly.m_Path = cmSmoothPath;
        }

        public void Awake()
        {
            cmComposer = cmVirtualCam.GetCinemachineComponent<CinemachineComposer>();
            cmTrackedDolly = cmVirtualCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        }
    }
}
