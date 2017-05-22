using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
	
    public class TankHealth : MonoBehaviour
    {
		
        public float m_StartingHealth = 100f;               // Starting health of tank
        public Slider m_Slider;                             // The slider to represent tank's current health
        public Image m_FillImage;                           // Slider's image
        public Color m_FullHealthColor = Color.green;       // Color of full health slider
        public Color m_ZeroHealthColor = Color.red;         // Color of 0 health slider
        public GameObject m_ExplosionPrefab;                // A prefab of explosion
		public AudioClip HealClip;
		public AudioSource HealAudioSource;
        
        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroye
        private float m_CurrentHealth;                      // Tank's current health
        private bool m_Dead;                                // If tank has less then 0 health, then it's dead

        /// <summary>
        /// Ustawia na mampe deaktywowaną animacje wybuchu, żeby poten z niej skorzystać
        /// </summary>
        private void Awake ()
        {
			HealAudioSource.enabled = false;
            // Instantiate the explosion prefab and get a reference to the particle system on it
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Will be enabled only when needed
            m_ExplosionParticles.gameObject.SetActive (false);
        }

        /// <summary>
        /// Wlącza GUI i odświeża go.
        /// </summary>
        private void OnEnable()
        {
            // When the tank is enabled, reset the tank's health, also he cant be dead
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;

            // Refresh the UI 
            SetHealthUI();
        }

        /// <summary>
        /// Przyjmuje punkty uszkodzenia i odejmuje od punktów życia na punkty uszkodzenia
        /// Odświeża GUI
        /// </summary>
        /// <param name="amount">Ilość punktów uszkodzenia</param>
        public void TakeDamage (float amount)
        {
            // Reduce current health by the amount of damage done
            m_CurrentHealth -= amount;

            // Refresh UI
            SetHealthUI ();

			// If the current health is 0 or below and it has not yet dead, use OnDeath()
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath ();
            }
        }

        /// <summary>
        /// Odświeża GUI
        /// </summary>
        private void SetHealthUI ()
        {
            // Set the slider's the same as health
            m_Slider.value = m_CurrentHealth;

            // Change the color of the bar between the choosen colours based on the current percentage of the starting health
            m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }

        /// <summary>
        /// Przy śmierci ustawia obiekt nieaktywnym
        /// Wlącza animacje wybuchu
        /// </summary>
        private void OnDeath ()
        {
            //Make tank dead, so function could be called only once
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and play it
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive (true);

            // Play the particle system of the tank exploding
            m_ExplosionParticles.Play ();

            // Play the tank explosion sound
            m_ExplosionAudio.Play();

            // Make tank disabled
            gameObject.SetActive (false);
        }

		//On trigger enter with an object with tag "Kit" heal the tank
        /// <summary>
        /// Przy dotykaniu do kitu zwiększa punkty życia na 25, lub do maksymalnego stanu
        /// </summary>
        /// <param name="kit">Przyjmuje obiekt z treigerem z tagiem 'Kit'</param>
		void OnTriggerEnter(Collider kit)
		{
			//Bool to chek if object's tag is "Kit"
			bool a = kit.CompareTag("Kit");
			if (a == true) 
			{
				//Heal
				Heal ();
				//Destroy the kit object
				Destroy (kit.gameObject);
			}
		}

		//Heal the tank
		void Heal()
		{
			HealAudioSource.enabled = true;
			//If tank is below 75 health, add to current health 25
			if (m_CurrentHealth < 75) 
			{
				m_CurrentHealth += 25;
				//Refresh UI
				SetHealthUI ();
			} 
			else 
			{
				//If tank has more than 75 health, set health to max, so the amount couldn't be bigger then 100
				m_CurrentHealth = m_StartingHealth;
				//Refresh UI
				SetHealthUI ();
			}

			HealAudioSource.clip = HealClip;
			HealAudioSource.Play ();

		}

		//Health regeneration +1 per second
		void HealthRegeneration()
		{
			//Regeneration is active only if tank has less then 100 health
			if (m_CurrentHealth < 100f) 
			{
				//+1 healt per second
				m_CurrentHealth += 1 * Time.deltaTime;
			}
			//refresh UI
			SetHealthUI ();
		}

		void Update()
		{
			//Regenerate health
			HealthRegeneration ();
		}
    }
}