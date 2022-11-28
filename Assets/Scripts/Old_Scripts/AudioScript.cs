using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
	public AudioClip[] playlist;
	public AudioSource audioSource;
	private int musicIndex = 0 ;
	
	public AudioMixerGroup soundEffectMixer;
	
	public static AudioScript instance;
	
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de AudioScript/Managers dans la scène");
			return;
		}
		instance = this;
	}

    
    void Start()
    {
        audioSource.clip = playlist[0];
		audioSource.Play();
    }
    
    void Update()
    {
        if(!audioSource.isPlaying)
		{
			PlayNextSong();
		}
    }
	
	void PlayNextSong()
	{
		musicIndex = (musicIndex + 1 ) % playlist.Length;
		audioSource.clip = playlist[musicIndex];
		audioSource.Play();
	}
	
	// PlayClipAt est ma propre méthode pour générer un sound
	public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
	{
		// creer un GameObject temporaire dont la variable tempraire est "tempGO"
		GameObject tempGO = new GameObject("TempAudio");
		tempGO.transform.position = pos;
		
		// creer un AudioSource dont la variable tempraire est audioSource
		// .AddComponent<AudioSource>();  va générer le component en question
		AudioSource audioSource = tempGO.AddComponent<AudioSource>();
		audioSource.clip = clip;
		// les components type AudioSource ont ce "outputAudioMixerGroup"
		audioSource.outputAudioMixerGroup = soundEffectMixer;
		audioSource.Play();
		// détruit l'objet temporaire après une durée égale à celle du clip
		Destroy(tempGO, clip.length);
		// il faut retourner un AudioSource  :
		return audioSource;
	}
	
}
