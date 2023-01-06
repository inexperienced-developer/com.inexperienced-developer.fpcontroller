using InexperiencedDeveloper.Controllers.FirstPerson;
using InexperiencedDeveloper.Controllers.Input;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace InexperiencedDeveloper.Core.FirstPerson
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject m_PlayerOffscreenPrefab;
        [SerializeField] private Camera m_Camera;
        public PlayerController Controller { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerOffscreen OffscreenPlayer { get; private set; }

        public void Init()
        {
            Input = GetComponent<PlayerInput>();
            Input.Init();
            Controller = GetComponent<PlayerController>();
            Controller.Init();
            OffscreenPlayer = Instantiate(m_PlayerOffscreenPrefab, Vector3.one * 5000, Quaternion.identity).GetComponent<PlayerOffscreen>();
            OffscreenPlayer.Init(this);
            m_Camera.GetUniversalAdditionalCameraData().cameraStack.Insert(0, OffscreenPlayer.Camera);
        }

        public void Init(Player mainPlayer)
        {

        }
    }
}

