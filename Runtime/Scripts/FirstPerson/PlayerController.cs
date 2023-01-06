using InexperiencedDeveloper.Controllers.Input;
using InexperiencedDeveloper.Core.FirstPerson;
using InexperiencedDeveloper.Utils;
using UnityEngine;

namespace InexperiencedDeveloper.Controllers.FirstPerson
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private Player m_Player;
        private CharacterController m_Controller;

        [Header("Camera settings")]
        public bool Invert;
        public Camera Camera { get; private set; }
        [SerializeField] private Vector2 m_Sensitivity = Vector2.one;
        [SerializeField] private float m_LookSpeed = 100;
        [SerializeField] private float[] m_MinMaxLook = new float[2] { -40f, 35f };

        private float m_MoveSpeed;

        [Header("Gravity")]
        //Calculations
        [SerializeField] private Transform m_GroundCheck;
        [SerializeField] private float m_GroundCheckRadius = 0.01f;
        [SerializeField] private LayerMask m_GroundLayer;
        //Variables
        public bool Grounded
        {
            get
            {
                GroundCheckColliders = Physics.OverlapSphere(m_GroundCheck.position, m_GroundCheckRadius, m_GroundLayer, QueryTriggerInteraction.Ignore);
                return GroundCheckColliders.Length > 0;
            }
        }

        public Collider[] GroundCheckColliders { get; private set; }
        private float m_YVel;

        [Header("Jump Settings")]
        [SerializeField] private float m_JumpForce = 20f;

        public void Init()
        {
            m_Player = GetComponent<Player>();

            //Camera
            Camera = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Controller
            m_Controller = GetComponent<CharacterController>();

            //Gravity
            m_GroundCheck = m_GroundCheck == null ? GameObject.Find("Ground_Check").transform : m_GroundCheck;
        }


        private void Update()
        {
            if (m_Player.Input == null)  return;
            m_MoveSpeed = m_Player.Input.Sprint ? Constants.SPRINT_SPEED : Constants.RUN_SPEED;
            ProcessGravity();
            if (m_Player.Input.Move != Vector2.zero)
            {
                Move();
            }
            if (m_Player.Input.Look != Vector2.zero)
            {
                Look();
            }
        }

        private void Move()
        {
            Vector3 move = transform.forward * m_Player.Input.Move.y + transform.right * m_Player.Input.Move.x;
            m_Controller.Move(move * Time.deltaTime * m_MoveSpeed);
        }

        private void Look()
        {
            Vector3 look = m_Player.Input.Look;
            float invert = Invert ? -1 : 1;
            Vector3 lookRotation = new Vector3((-look.y * invert) * m_Sensitivity.x, look.x * m_Sensitivity.y, 0) * m_LookSpeed * Time.deltaTime;
            //Move Player
            Vector3 currRot = transform.eulerAngles;
            currRot.y += lookRotation.y;
            currRot.y = currRot.y >= 360 ? currRot.y - 360 : currRot.y;
            transform.rotation = Quaternion.Euler(currRot);

            //Move Camera
            Vector3 camRot = Camera.transform.localEulerAngles;
            camRot.x += lookRotation.x;
            camRot.x = camRot.x >= 180 ? camRot.x - 360 : camRot.x;
            camRot.x = Mathf.Clamp(camRot.x, m_MinMaxLook[0], m_MinMaxLook[1]);
            Camera.transform.localRotation = Quaternion.Euler(camRot);
        }

        private void Jump(ref float yVel)
        {
            yVel = Mathf.Sqrt(m_JumpForce * -2f * Constants.GRAVITY);
        }

        private void ProcessGravity()
        {
            if (Grounded && m_YVel < 0)
            {
                m_YVel = -2f;
            }
            if (m_Player.Input.Jump && Grounded)
            {
                Jump(ref m_YVel);
            }
            m_YVel += Constants.GRAVITY * Time.deltaTime;
            Vector3 gravity = Vector3.zero;
            gravity.y = m_YVel;
            m_Controller.Move(gravity * Time.deltaTime);
        }

        public void EnableCharacterController(bool enable)
        {
            m_Controller.enabled = enable;
        }

        public void ToggleController(bool enable)
        {
            m_Controller.enabled = enable;
            Camera.enabled = enable;
        }
    }
}


