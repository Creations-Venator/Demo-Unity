using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    public static DialogueScript instance;
	
	public Text nameText;
	public Text dialogueText;
	
	// ici file  != pile
	private Queue<string> sentences;
	
	public Animator animator;
	
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de DialogueScript dans la scène");
			return;
		}
		instance = this;
		
		sentences = new Queue<string>();
	}
	
	public void StartDialogue(Dialogues dialogue)
	{
		nameText.text = dialogue.name;
		
		sentences.Clear();
		
		animator.SetBool("IsOpen",true);
		
		foreach(string sentence in dialogue.sentences )
		{
			// Enqueue : enfile : envoie dans la file
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence();
		
	}
	
	public void DisplayNextSentence()
	{
		if(sentences.Count == 0)
		{
			EndDialogue();
			return;
		}
		// Dequeue c'est l'inverse de Enqueue : récupère prochain élément de la file
		string sentence = sentences.Dequeue();
		//dialogueText.text = sentence;
		
		// afficher les lettres 1 par 1 :   StopAllCoroutines(); permet au joueur de skip
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}
	
	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		//ToCharArray() découpe la sentence en un tableau
		foreach(char letter in sentence.ToCharArray() )
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(0.03f);
		}
	}
	
	public void EndDialogue()
	{
		animator.SetBool("IsOpen",false);
		//Debug.Log("fin de blabla");
	}
	
}
