using UnityEngine;
// pour que le Slider soit reconnu
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
	
	public Gradient gradient;
	public Image fill;
	
	// initialiser la healthbar : 
	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;
		
		// 1f : couleur à droite, 'location' 100%
		fill.color = gradient.Evaluate(1f);
	}
	
	// indiquer les changements de la healthbar
	public void SetHealth(int health)
	{
		slider.value = health;
		
		// normalisé pour l'avoir en %age, entre 0 et 1
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}
	
}
