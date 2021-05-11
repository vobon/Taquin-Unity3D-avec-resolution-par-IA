using UnityEngine;
using System.Collections;

public class CasePuzzle3 : MonoBehaviour 
{
	//La position (x,y,z)  de la case que l'on souhaite accédé
	public Vector3 PositionCible;

	// on regarde si la case actuelle est active
	public bool Active = true;

	//verifie si la case est a la bonne position
	public bool bonneposition = false;

	// on sauvegarde la  vrai position de la case dans la grille
	public Vector2 positionreelle = new Vector2();

	// on sauvegarde la position actuelle de la case dans la grille
	public Vector2 positiongrille = new Vector2();

	public int nombre;

	void Awake()
	{
		// on initialise target position au debut du jeu
		PositionCible = this.transform.localPosition;

		// au debut du jeu permet de toujours mettre a jour  la position de la case 
		StartCoroutine(UpdatePosition());
	}
	void Update()
	{
		if (Active == true)
		{
			this.GetComponent<Renderer>().enabled = true;
			this.GetComponent<Collider>().enabled = true;
		}
		else
		{
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}
	}
	public  void DeplacementCase(Vector3 nouvPos)
	{
		// on recupere la nouvelle position de la case
		PositionCible = nouvPos;

		// on modifie la position de la case
		StartCoroutine(UpdatePosition());
	}

	public IEnumerator UpdatePosition()
	{
		// on compare la position actuelle est la position a atteindre tant qu'elle n'est pas atteinte la boucle tourne
		while(PositionCible != this.transform.localPosition)
		{
			// on se deplace vers notre position cible
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, PositionCible, 10.0f * Time.deltaTime);
			yield return null;
		}

		// apres chaque deplacement on verifie si nous avons atteint notre position initiale
		if(positionreelle == positiongrille){bonneposition = true;}else{bonneposition = false;}

		// si la case est la case inactive alors on desactive son rendu et sa boite de collision
		if(Active == false)
		{
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}

		yield return null;
	}

	public void DeplacementCaseAleatoire()
	{
		// permet de modifier la position de la case , la fonction prend en parametre une nouvelle position obtenue grace au parent qui est la grille
		DeplacementCase(this.transform.parent.GetComponent<Puzzle3>().PosCasevide(this.GetComponent<CasePuzzle3>()));
	}

	void OnMouseDown()
	{
		// permet de modifier la position de la case quand un clic droit est effectue 
		DeplacementCase(this.transform.parent.GetComponent<Puzzle3>().PosCasevide(this.GetComponent<CasePuzzle3>()));
	}
}
