using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{	
	// cette variable fait référence à la class Dialogues ( du script Dialogues )
    public Dialogues dialogue;
	private bool isInRange;
	
	private Text interactUI;
	
	void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUITag").GetComponent<Text>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.I))
		{
			DialogueTri();
		}
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			interactUI.enabled = true;
			isInRange = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{	
		if(collision.CompareTag("Player"))
			{
			interactUI.enabled = false;
			isInRange = false;
			DialogueScript.instance.EndDialogue();
			}
	}
	
	void DialogueTri()
	{
		DialogueScript.instance.StartDialogue(dialogue);
	}
}
