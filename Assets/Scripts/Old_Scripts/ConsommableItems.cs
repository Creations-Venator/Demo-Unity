
using UnityEngine;


// ce script gère les datas, ce n'est pas un MonoBehaviour
[CreateAssetMenu(fileName = "ConsommableItems", menuName = "Inventaire/ConsommableItems")]
public class ConsommableItems : ScriptableObject
{
    public int id;
	public string name;
	public string description;
	public Sprite image;
	public int hpGiven;
	public int speedGiven;
	public float speedDuration;
	public int price;
}
