using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;


public class SettingsMenu : MonoBehaviour
{
    
	public AudioMixer audioMixer;
	
	Resolution[] resolutions;
	
	public Dropdown resolutionDropdown;
	
	// stock les valeurs actuelles des volumes
	public Slider musicSlider;
	public Slider soundSlider;
	
	public void Start()
	{	// 'out' pour stcoker la valeur  
		audioMixer.GetFloat("Music", out float musicValueForSlider);
		musicSlider.value = musicValueForSlider;
		audioMixer.GetFloat("Sound", out float soundValueForSlider);
		soundSlider.value = soundValueForSlider;
		
		//au lancement du menu, on récupère toutes les résolutions possibles selon la machine
		resolutions = Screen.resolutions;
		//utiliser 'using System.Linq; ' puis Distinct() pour éviter les doubleons:
		// resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
		// remplace  resolutions = Screen.resolutions; en évitant de possibles doubleons
		
		// retire les options par default :
		resolutionDropdown.ClearOptions();
		
		// options au pluriel est une liste de string
		List<string> options = new List<string>();
		
		int currentResolutionIndex = 0;
		for (int i = 0; i< resolutions.Length; i++)
		{
			// option au singulier est une variable temporaire string
			string option = resolutions[i].width + "x" + resolutions[i].height ;
			options.Add(option);
			
			if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
			{
				currentResolutionIndex = i;
			}
			
		}
		
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
		
		// on peut mieux faire que ça :
		Screen.fullScreen = true;
		
	}
	
    public void SetVolume(float volume)
    {
        //Debug.Log(volume);
		audioMixer.SetFloat("Music",volume);
    }
	
	public void SetSoundVolume(float volume)
    {
        //Debug.Log(volume);
		audioMixer.SetFloat("Sound",volume);
    }
	
	public void SetFullScreen(bool isFullScreen)
	{
		 
		Screen.fullScreen = isFullScreen;
	}
	
	public void SetResolution(int resolutionIndex)
	{
		// on récupère la résolutions correspondante à l'index envoyé en parametre
		// de la liste des résolutions possibles 
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen );
		
	}
	
	public void ClearSavedData()
	{
		PlayerPrefs.DeleteAll();
	}
}
