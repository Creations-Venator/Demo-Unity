using UnityEngine;

// n'est pas MonoBehaviour
[System.Serializable]
public class Dialogues
{
    public string name;
	
	[TextArea(3,10)]
	public string[] sentences;
}
