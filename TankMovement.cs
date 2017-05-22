using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {

        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player. Every tank has it's own control button based on player's number
        public float m_Speed = 12f;                 // Speed of tank
        public float m_TurnSpeed = 180f;            // How many degrees tank turns per second
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary
        
        private string m_MovementAxisName;          // The name of the input axis for moving forward and back
        private string m_TurnAxisName;              // The name of the input axis for turning
        private Rigidbody m_Rigidbody;              // Reference used to move the tank
        private float m_MovementInputValue;         // The current value of the movement input
        private float m_TurnInputValue;             // The current value of the turn input
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene
        
        /// <summary>
        /// Pobiera component typu Rigidbody
        /// </summary>
        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
        }

        /// <summary>
        /// Przy aktywowaniu zeruje inputy, żeby uniknąć blędów
        /// </summary>
        private void OnEnable ()
        {
            // When the tank is turned on, make sure it's not kinematic
            m_Rigidbody.isKinematic = false;

            //Also reset the input values
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke

			
        }

        /// <summary>
        /// Przy wylączeniu obiektu, wylącz możlowość kontrolu.
        /// </summary>
        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
           
        }

        /// <summary>
        /// Na pocztku gry wskazuje na inputy;.
        /// </summary>
        private void Start ()
        {
            // The axes names are based on player number
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        /// <summary>
        /// Caly czas odczytuje czy istnieją inputy od użytkownika
        /// </summary>
        private void Update ()
        {
            // Store the value of both input axes
            m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

            EngineAudio ();
        }

        /// <summary>
        /// Znienia dzwięk silnika
        /// </summary>
        private void EngineAudio ()
        {
            // If there is no input 
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // If the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        /// <summary>
        /// Wywoluje funkcje(Zoom i update)
        /// </summary>
        private void FixedUpdate ()
        {
            // Using movement and turning functions in fixed update
            Move ();
            Turn ();
        }

        /// <summary>
        /// Zmienia pozyczę obiektu w zależności od inputów
        /// </summary>
        private void Move ()
        {
            // Create a vector for moving tank using input values, speed, time and face it to right direction
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        /// <summary>
        /// Obraca obiekt dookola siebie
        /// </summary>
        private void Turn ()
        {
            // Creating turn value
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis
			Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }
    }
}