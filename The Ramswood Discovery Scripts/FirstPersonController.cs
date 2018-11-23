using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {

        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;

        [SerializeField] private float m_Stamina;
        public bool isWalking;
        Rect staminaBar;
        public Texture2D staminaTexture;

        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        int count = 0;
        public int inventory = 0;

        [SerializeField] private float originalSpeed;
        private float originalWalkSpeed;
        [SerializeField] private float MAXstamina = 500;
        [SerializeField] private float staminaRecovery = .5f;
        [HideInInspector] public float speed;
        [HideInInspector] public bool timer = false;
        [HideInInspector] public bool check = false;
        [HideInInspector] public float stamina;

        [HideInInspector] public bool trap = false;
        private bool extraCheck = false;
        public float speedPowerUp = 3;
        public float powerUpTime = 4;

        public AudioSource canOpening;

        private bool isGrounded; // is on a slope or not
        public float slideFriction = 0.3f; // ajusting the friction of the slope
        private Vector3 hitNormal; //orientation of the slope.


        // Use this for initialization
        private void Start()
        {
            originalSpeed = m_RunSpeed;
            originalWalkSpeed = m_WalkSpeed;
            stamina = MAXstamina;
            MAXstamina = m_Stamina;
            isWalking = m_IsWalking;
            //speed = m_RunSpeed;
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);

            staminaBar = new Rect(Screen.width / 10, Screen.height * 9 / 10, Screen.width / 3, Screen.height / 50);
            //staminaTexture = new Texture2D(1, 1);
            //staminaTexture.SetPixel(0, 0, Color.red);
            if(staminaTexture!=null)
            staminaTexture.Apply();
            
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("Started Running");
                m_IsWalking = false;
                m_RunSpeed = originalSpeed;
                count += 1;

            }

            if (count >= 2)
            {
                m_IsWalking = true; //Allows the player to press shift instead of hold
                count = 0;
            }

            if (timer == true)
            {
                m_RunSpeed = speed;

                if (isWalking)
                {
                    m_RunSpeed += (speedPowerUp*2);

                }

                if (trap == true)
                {
                    m_WalkSpeed = speed;
                    Debug.Log("Tm_WalkSpeed = speed;");
                    trap = false;
                }
                //Debug.Log("TIMER=TRUE");
                if (check == true)
                {
                    //Debug.Log("CHECK=TRUE");
                    m_RunSpeed = originalSpeed;
                    m_WalkSpeed = originalWalkSpeed;

                    timer = false;
                    check = false;
                }

            }
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            if(inventory <= 0)
            {
                inventory = 0;
            }
            if (Input.GetKeyDown(KeyCode.T) && inventory > 0)
            {
                StartCoroutine(WaitAgain());
            }


            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            m_Stamina = stamina;
            isWalking = m_IsWalking;

        }

        IEnumerator WaitAgain()//////////////////////////////////////////////////////////////////////////////////////////
        {
            inventory -= 1;
            timer = true;
            canOpening.Play();
            m_RunSpeed += speedPowerUp;
            m_WalkSpeed += speedPowerUp;
            yield return new WaitForSeconds(powerUpTime);
            check = true;
            Debug.Log("hmmmmmm");
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            //float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3) 
                || Physics.Raycast(transform.position, -transform.forward, out hit, 3) 
                || Physics.Raycast(transform.position, transform.right, out hit, 3) 
                || Physics.Raycast(transform.position, -transform.right, out hit, 3))
            {
                if (!isGrounded && hit.transform.tag == "terrain")
                {
                    m_MoveDir.x += (1.5f - hitNormal.y) * hitNormal.x * (speed - slideFriction);
                    m_MoveDir.z += (1.5f - hitNormal.y) * hitNormal.z * (speed - slideFriction);
                }
            }

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            isGrounded = Vector3.Angle(Vector3.up, hitNormal) <= m_CharacterController.slopeLimit;/////////

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {

            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;


            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            // m_IsWalking = !Input.GetKey(KeyCode.LeftShift);//////////////////////////////////////////
            /*
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("Started Running");
                m_IsWalking = false;
                m_RunSpeed = originalSpeed;
                count += 1;

            }

            if (count >= 2)
            {
                m_IsWalking = true; //Allows the player to press shift instead of hold
                count = 0;
            }
            */
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);
            //////////////////////////////////////////////////////////
            if (m_IsWalking != true)
            {
                stamina -= 1;
            }
            else
            {
                stamina += staminaRecovery;
            }
            if (stamina >= MAXstamina)
            {
                stamina = MAXstamina;
            }
            if (stamina <= 0)
            {
                stamina = 0;
                m_IsWalking = true;
                speed = m_WalkSpeed;  //WORKS
                m_WalkSpeed = originalWalkSpeed;
                m_RunSpeed = originalSpeed;
                count = 0;//sets count to 0 once the player runs out of stamina
            }
            ///////////////////////////////////////////////////////////
            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            hitNormal = hit.normal;

            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        void OnGUI()
        {
            float ratio = stamina / 500;
            float rectWidth = ratio * Screen.width / 3;
            float rectHeight = Screen.height / 50;
            staminaBar.width = rectWidth;
            staminaBar.height = rectHeight;
            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("box"));
            string preface = "Energy Drinks: ";
            myStyle.fontSize = Screen.height/35;
            myStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(Screen.width / 10.0f, Screen.height / 1.11f, Screen.width / 3, rectHeight), " ", myStyle); // Stamina overlay
            GUI.DrawTexture(staminaBar, staminaTexture); // Stamina Bar
            GUI.Box(new Rect(Screen.width / 10.0f, Screen.height / 1.2f, Screen.width / 3.2f, Screen.height/20), preface + " " + inventory, myStyle); // Inventory
        }
    }
 }



