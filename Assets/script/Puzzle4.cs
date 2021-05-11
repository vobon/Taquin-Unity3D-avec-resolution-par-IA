﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using System;
using System.IO;
using System.Diagnostics;
using UnityEditor;


public class Puzzle4 : MonoBehaviour
{
	// l'image utilisé pour  texturer du puzzle
	public Texture PuzzleImage;

	// la taille du puzzle(hauteur:Z et largeur:X)
	public int Height = 4;
	public int Width = 4;

	// une valeur de scale pour agrandir les cases du puzzle
	public Vector3 PuzzleScale = new Vector3(1.0f, 1.0f, 1.0f);

	// Permet de placer le puzzle en fonction de la camera
	public Vector3 PuzzlePosition = new Vector3(0.0f, 0.0f, 0.0f);

	// La ligne de separation entre 2 cases
	public float Separationcase = 0.5f;

	// on definit une case de puzzle
	public GameObject Case;

	// permet de recuperer le shader de l'image de puzzle
	public Shader PuzzleShader;

	// tableau contenant les cases du puzzle
	private GameObject[,] TabCasePuzzle;
	// tableau contenant les position des cases du puzzle
	private List<Vector3> TabPosition = new List<Vector3>();

	// Des vecteurs de position et de scale
	private Vector3 Scale;
	private Vector3 Position;

	// booleen pour savoir si le puzzle a ete completé
	public bool Complet = false;

	public int[,] etatinitial;
	public Node racine;
	public Node resultat;
	public List<Node> Nodeexplorer = new List<Node>();
	public List<Node> Nodenonexplorer = new List<Node>();
	public List<Node> Chemin = new List<Node>();
	public int nombrealeatoire =0;
	public int choixheur;
	int nbrboucle = 0;
	public Node Solution;
	public Text niveau;
	public Dictionary<string, int> PBDB ;
	public Dictionary<string, int> DBDB;
	public Dictionary<string, int> TBDB;
	public Dictionary<string, int> PBDBS;
	public Dictionary<string, int> DBDBS;
	public Dictionary<string, int> TBDBS;
	Stopwatch stopWatch = new Stopwatch();

	void Start()
	{
		string[] guids1 = AssetDatabase.FindAssets("PBDB", null);
		string[] guids2 = AssetDatabase.FindAssets("DBDB", null);
		string[] guids3 = AssetDatabase.FindAssets("TBDB", null);
		string[] guids4 = AssetDatabase.FindAssets("PB6DB", null);
		string[] guids5 = AssetDatabase.FindAssets("DB6DB", null);
		string[] guids6 = AssetDatabase.FindAssets("TB6DB", null);
		PBDB = Read(AssetDatabase.GUIDToAssetPath(guids1[0])) ;
		DBDB = Read(AssetDatabase.GUIDToAssetPath(guids2[0]));
		TBDB = Read(AssetDatabase.GUIDToAssetPath(guids3[0]));

		PBDBS = Read(AssetDatabase.GUIDToAssetPath(guids4[0]));
		DBDBS = Read(AssetDatabase.GUIDToAssetPath(guids5[0]));
		TBDBS = Read(AssetDatabase.GUIDToAssetPath(guids6[0]));
	}
	public void Debut()
	{
		StopAllCoroutines();
		choixheur = 0;
		Complet = false;
		if (TabCasePuzzle != null)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					Destroy(TabCasePuzzle[i, j]);
				}
			}
		}
		if (nombrealeatoire == 0)
		{
			nombrealeatoire = 7;
		}
		niveau.text = "Niveau "+nombrealeatoire.ToString();
		// fonction cree les cases des puzzles sans les mixer
		CreationCasePuzzle();
		//racine = new Node(4);
		//racine.etat[0, 0] = 4;
		//racine.etat[0, 1] = 7;
		//racine.etat[0, 2] = 15;
		//racine.etat[0, 3] = 0;
		//racine.etat[1, 0] = 3;
		//racine.etat[1, 1] = 8;
		//racine.etat[1, 2] = 11;
		//racine.etat[1, 3] = 12;
		//racine.etat[2, 0] = 2;
		//racine.etat[2, 1] = 5;
		//racine.etat[2, 2] = 10;
		//racine.etat[2, 3] = 9;
		//racine.etat[3, 0] = 1;
		//racine.etat[3, 1] = 6;
		//racine.etat[3, 2] = 14;
		//racine.etat[3, 3] = 13;
		//Debug.Log(PatternDatabasecq(racine));

		// on mixe le puzzle au debut du jeu
		StartCoroutine(MixaleatoirePuzzle());

	}

	// La fonction update tourne tout le temps
	void Update()
	{
		// Permet de modifier la position du puzzle au cours du jeu
		this.transform.localPosition = PuzzlePosition;

		// Permet de modifier le scale du puzzle au cours du jeu
		this.transform.localScale = PuzzleScale;
	}


	public Vector3 PosCasevide(CasePuzzle4 casechoisie)
	{
		// On verifie si un mouvement est possible est possible et si oui vers quelle cellule
		CasePuzzle4 casecible = VerifMouvement((int)casechoisie.positiongrille.x, (int)casechoisie.positiongrille.y, casechoisie);

		// si le mouvement est possible
		if (casecible != casechoisie)
		{
			// On recupere la position de la case cible
			Vector3 ciblePos = casecible.PositionCible;
			Vector2 position = casechoisie.positiongrille;
			casechoisie.positiongrille = casecible.positiongrille;

			// on deplace la case vide vers l'ancienne position de la case
			casecible.DeplacementCase(casechoisie.PositionCible);
			casecible.positiongrille = position;

			// return la position de la nouvelle cible a atteindre
			return ciblePos;
		}

		// sinon si il y'a pas de mouvement on return la position actuelle
		return casechoisie.PositionCible;
	}

	private CasePuzzle4 DeplaGauche(int Xpos, int Ypos, CasePuzzle4 caseactuelle)
	{
		// on verifie que l'on ne sorte pas du puzzle
		if ((Xpos - 1) >= 0)
		{
			//on verifie maintenant si la case a Gauche est vide
			return VerifPosCase(Xpos - 1, Ypos, caseactuelle);
		}

		return caseactuelle;
	}

	private CasePuzzle4 DeplaDroite(int Xpos, int Ypos, CasePuzzle4 caseactuelle)
	{
		// on verifie que l'on ne sorte pas du puzzle
		if ((Xpos + 1) < Width)
		{
			//on verifie maintenant si la case a Droite est vide
			return VerifPosCase(Xpos + 1, Ypos, caseactuelle);
		}

		return caseactuelle;
	}

	private CasePuzzle4 DeplaBas(int Xpos, int Ypos, CasePuzzle4 caseactuelle)
	{
		// on verifie que l'on ne sorte pas du puzzle
		if ((Ypos - 1) >= 0)
		{
			//on verifie maintenant si la case en Bas est vide
			return VerifPosCase(Xpos, Ypos - 1, caseactuelle);
		}

		return caseactuelle;
	}

	private CasePuzzle4 DeplaHaut(int Xpos, int Ypos, CasePuzzle4 caseactuelle)
	{
		// on verifie que l'on ne sorte pas du puzzle
		if ((Ypos + 1) < Height)
		{
			//on verifie maintenant si la case en Haut est vide
			return VerifPosCase(Xpos, Ypos + 1, caseactuelle);
		}

		return caseactuelle;
	}

	private CasePuzzle4 VerifMouvement(int Xpos, int Ypos, CasePuzzle4 caseactuelle)
	{
		// On verif tous les deplacements possibles
		if (DeplaGauche(Xpos, Ypos, caseactuelle) != caseactuelle)
		{
			return DeplaGauche(Xpos, Ypos, caseactuelle);
		}

		if (DeplaDroite(Xpos, Ypos, caseactuelle) != caseactuelle)
		{
			return DeplaDroite(Xpos, Ypos, caseactuelle);
		}

		if (DeplaBas(Xpos, Ypos, caseactuelle) != caseactuelle)
		{
			return DeplaBas(Xpos, Ypos, caseactuelle);
		}

		if (DeplaHaut(Xpos, Ypos, caseactuelle) != caseactuelle)
		{
			return DeplaHaut(Xpos, Ypos, caseactuelle);
		}

		return caseactuelle;
	}

	private CasePuzzle4 VerifPosCase(int x, int y, CasePuzzle4 caseactuelle)
	{
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				// on verifie si une case est placée a la position x et y
				if ((TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().positiongrille.x == x) &&
				   (TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().positiongrille.y == y))
				{
					if (TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().Active == false)
					{
						// si la case a cette position est inactive alors on retourne cette case
						return TabCasePuzzle[i, j].GetComponent<CasePuzzle4>();
					}
				}
			}
		}
		// sinon on retourne la case de depart
		return caseactuelle;
	}

	private IEnumerator MixaleatoirePuzzle()
	{
		yield return new WaitForSeconds(1.0f);

		// on desactive le render de la case en bas a droite(la 9eme case)
		TabCasePuzzle[3, 0].GetComponent<CasePuzzle4>().Active = false;


		yield return new WaitForSeconds(1.0f);
		//9 13
		for (int k = 0; k < nombrealeatoire; k++)
		{
			//on deplace les cases autant de fois que le nombrealeatoire
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{

					TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().DeplacementCaseAleatoire();

					yield return new WaitForSeconds(0.02f);
				}
			}
		}
		Complet = false;
		StartCoroutine(EstComplet());

		yield return null;
	}

	public IEnumerator EstComplet()
	{
		while (Complet == false)
		{
			// on verifie pour chaque case si la position reelle de chaque case est sa position actuelle
			Complet = true;
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					if (TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().bonneposition == false)
					{
						Complet = false;
					}
				}
			}

			yield return null;
		}

		// si toutes les cases sont completes alors on affiche
		if (Complet)
		{
			TabCasePuzzle[3, 0].GetComponent<CasePuzzle4>().Active = true;
			Debug.Log("Puzzle Complet!");
		}

		yield return null;
	}


	private void CreationCasePuzzle()
	{
		// On cree un tableau de GameObjet
		TabCasePuzzle = new GameObject[Width, Height];
		etatinitial = new int[4, 4];

		// on donne a chaque case sa taille par rapport a la taille de la grille
		Scale = new Vector3(1.0f / Width, 1.0f, 1.0f / Height);
		Case.transform.localScale = Scale;

		// permet de compter le nombre de case (varie en fonction de la taille de la grille)
		int Compteur = 0;
		int z = 1;
		// on cree une double boucle pour instancier les cases aux bonnes positions
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				// on calcule la position centrée de la case en prenant en compte la distance entre deux cases
				Position = new Vector3(((Scale.x * (i + 0.5f)) - (Scale.x * (Width / 2.0f))) * (10.0f + Separationcase),
									   0.0f,
									  ((Scale.z * (j + 0.5f)) - (Scale.z * (Height / 2.0f))) * (10.0f + Separationcase));

				// on sauvegarde la position dans un tableau de position
				TabPosition.Add(Position);

				// on instancie la case
				TabCasePuzzle[i, j] = Instantiate(Case, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(90.0f, -180.0f, 0.0f)) as GameObject;
				TabCasePuzzle[i, j].gameObject.transform.parent = this.transform;


				// on accede au script de la nouvelle case crée
				CasePuzzle4 nouvcase = TabCasePuzzle[i, j].GetComponent<CasePuzzle4>();
				//on initialise la position dans la grille et sa position dans originelle
				nouvcase.positionreelle = new Vector2(i, j);
				nouvcase.positiongrille = new Vector2(i, j);
				if (i != 3 || j != 0)
				{

					nouvcase.nombre = z++;
				}
				else
				{
					nouvcase.nombre = 0;
				}
				etatinitial[i, j] = nouvcase.nombre;
				//on place la case au bon emplacement
				nouvcase.DeplacementCase(Position);
				//on incremente le nombre de case
				Compteur++;

				// on cree un nouveau materiel en utilisant le shader de la grille
				Material Materielnouvcase = new Material(PuzzleShader);

				// on applique  la texture de l'image du puzzle
				Materielnouvcase.mainTexture = PuzzleImage;

				//permet de centrer au coin inferieur Gauche ou on recupere la texture sur l'image du puzzle
				Materielnouvcase.mainTextureOffset = new Vector2(1.0f / Width * i, 1.0f / Height * j);
				//permet de de definir la surface de la  texture sur l'image du puzzle
				Materielnouvcase.mainTextureScale = new Vector2(1.0f / Width, 1.0f / Height);

				// on applique ce nouveau materiel a la case
				TabCasePuzzle[i, j].GetComponent<Renderer>().material = Materielnouvcase;
			}
		}

	}
	public CasePuzzle4 returnpositiongrille(int i, int j)
	{

		for (int n = Height - 1; n >= 0; n--)
		{
			for (int m = 0; m < Width; m++)
			{
				if (TabCasePuzzle[m, n].GetComponent<CasePuzzle4>().positiongrille.x == i && TabCasePuzzle[m, n].GetComponent<CasePuzzle4>().positiongrille.y == j)
				{
					return TabCasePuzzle[m, n].GetComponent<CasePuzzle4>();
				}
			}
		}
		return null;
	}
	public int ChoixHeurst(Node A)
	{
		if (choixheur == 1)
		{
			return Hammingdistance(A);
		}
		if (choixheur == 2)
		{
			return Manhattandistance(A);
		}
		if (choixheur == 3)
		{
			return linconflict(A);
		}
		if (choixheur == 4)
		{
			return PatternDatabasecq(A);
		}
		if (choixheur == 5)
		{
			return PatternDatabasesx(A);
		}
		else
		{
			return 0;
		}
	}
	public int Hammingdistance(Node A)
	{
		int nbr = 0;
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (etatinitial[i, j] == A.etat[i, j] && etatinitial[i, j] != 0)
				{
					nbr++;
				}
			}
		}
		return 15 - nbr;
	}
	public Vector2Int posinitstate(int A)
	{
		Vector2Int aret = new Vector2Int();

		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (etatinitial[i, j] == A)
				{
					aret = new Vector2Int(i, j);
				}
			}
		}

		return aret;
	}
	public int Manhattandistance(Node A)
	{
		int nbrmanh = 0;

		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (etatinitial[i, j] != A.etat[i, j] && A.etat[i, j] != 0)
				{
					nbrmanh += Mathf.Abs(i - posinitstate(A.etat[i, j]).x) + Mathf.Abs(j - posinitstate(A.etat[i, j]).y);
				}
			}
		}
		return nbrmanh;
	}
	public int PatternDatabasecq(Node A)
	{
		string pbdb;
		string dbdb;
		string tbdb;
		int aret = 0;
		int i1 = 0;
		int j1 = 0;
		int i5 = 0;
		int j5 = 0;
		int i2 = 0;
		int j2 = 0;
		int i3 = 0;
		int j3 = 0;
		int i4 = 0;
		int j4 = 0;
		int i7 = 0;
		int j7 = 0;
		int i8 = 0;
		int j8 = 0;
		int i6 = 0;
		int j6 = 0;
		int i12 = 0;
		int j12 = 0;
		int i11 = 0;
		int j11 = 0;
		int i9 = 0;
		int j9 = 0;
		int i10 = 0;
		int j10 = 0;
		int i13 = 0;
		int j13 = 0;
		int i14 = 0;
		int j14 = 0;
		int i15 = 0;
		int j15 = 0;
		int i0 = 0;
		int j0 = 0;
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{

				if (A.etat[i, j] == 1)
				{
					i1 = i;
					j1 = j;
				}

				if (A.etat[i, j] == 5)
				{
					i5 = i;
					j5 = j;
				}

				if (A.etat[i, j] == 2)
				{
					i2 = i;
					j2 = j;
				}

				if (A.etat[i, j] == 3)
				{
					i3 = i;
					j3 = j;
				}

				if (A.etat[i, j] == 4)
				{
					i4 = i;
					j4 = j;
				}
				if (A.etat[i, j] == 7)
				{
					i7 = i;
					j7 = j;
				}

				if (A.etat[i, j] == 8)
				{
					i8 = i;
					j8 = j;
				}

				if (A.etat[i, j] == 11)
				{
					i11 = i;
					j11 = j;
				}

				if (A.etat[i, j] == 12)
				{
					i12 = i;
					j12 = j;
				}

				if (A.etat[i, j] == 6)
				{
					i6 = i;
					j6 = j;
				}

				if (A.etat[i, j] == 9)
				{
					i9 = i;
					j9 = j;
				}

				if (A.etat[i, j] == 10)
				{
					i10 = i;
					j10 = j;
				}

				if (A.etat[i, j] == 13)
				{
					i13 = i;
					j13 = j;
				}

				if (A.etat[i, j] == 14)
				{
					i14 = i;
					j14 = j;
				}

				if (A.etat[i, j] == 15)
				{
					i15 = i;
					j15 = j;
				}

				if (A.etat[i, j] == 0)
				{
					i0 = i;
					j0 = j;
				}
			}
		}
		tbdb = i9.ToString();
		tbdb += j9.ToString();
		tbdb += i10.ToString();
		tbdb += j10.ToString();
		tbdb += i13.ToString();
		tbdb += j13.ToString();
		tbdb += i14.ToString();
		tbdb += j14.ToString();
		tbdb += i15.ToString();
		tbdb += j15.ToString();

		dbdb = i6.ToString();
		dbdb += j6.ToString();
		dbdb += i7.ToString();
		dbdb += j7.ToString();
		dbdb += i8.ToString();
		dbdb += j8.ToString();
		dbdb += i11.ToString();
		dbdb += j11.ToString();
		dbdb += i12.ToString();
		dbdb += j12.ToString();

		pbdb = i1.ToString();
		pbdb += j1.ToString();
		pbdb += i2.ToString();
		pbdb += j2.ToString();
		pbdb += i3.ToString();
		pbdb += j3.ToString();
		pbdb += i4.ToString();
		pbdb += j4.ToString();
		pbdb += i5.ToString();
		pbdb += j5.ToString();

		aret = PBDB[pbdb] + DBDB[dbdb] + TBDB[tbdb];
		return aret;

	}
	public int PatternDatabasesx(Node A)
	{
		string pbdb;
		string dbdb;
		string tbdb;
		int aret = 0;
		int i1 = 0;
		int j1 = 0;
		int i5 = 0;
		int j5 = 0;
		int i2 = 0;
		int j2 = 0;
		int i3 = 0;
		int j3 = 0;
		int i4 = 0;
		int j4 = 0;
		int i7 = 0;
		int j7 = 0;
		int i8 = 0;
		int j8 = 0;
		int i6 = 0;
		int j6 = 0;
		int i12 = 0;
		int j12 = 0;
		int i11 = 0;
		int j11 = 0;
		int i9 = 0;
		int j9 = 0;
		int i10 = 0;
		int j10 = 0;
		int i13 = 0;
		int j13 = 0;
		int i14 = 0;
		int j14 = 0;
		int i15 = 0;
		int j15 = 0;
		int i0 = 0;
		int j0 = 0;
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{

				if (A.etat[i, j] == 1)
				{
					i1 = i;
					j1 = j;
				}

				if (A.etat[i, j] == 5)
				{
					i5 = i;
					j5 = j;
				}

				if (A.etat[i, j] == 2)
				{
					i2 = i;
					j2 = j;
				}

				if (A.etat[i, j] == 3)
				{
					i3 = i;
					j3 = j;
				}

				if (A.etat[i, j] == 4)
				{
					i4 = i;
					j4 = j;
				}
				if (A.etat[i, j] == 7)
				{
					i7 = i;
					j7 = j;
				}

				if (A.etat[i, j] == 8)
				{
					i8 = i;
					j8 = j;
				}

				if (A.etat[i, j] == 11)
				{
					i11 = i;
					j11 = j;
				}

				if (A.etat[i, j] == 12)
				{
					i12 = i;
					j12 = j;
				}

				if (A.etat[i, j] == 6)
				{
					i6 = i;
					j6 = j;
				}

				if (A.etat[i, j] == 9)
				{
					i9 = i;
					j9 = j;
				}

				if (A.etat[i, j] == 10)
				{
					i10 = i;
					j10 = j;
				}

				if (A.etat[i, j] == 13)
				{
					i13 = i;
					j13 = j;
				}

				if (A.etat[i, j] == 14)
				{
					i14 = i;
					j14 = j;
				}

				if (A.etat[i, j] == 15)
				{
					i15 = i;
					j15 = j;
				}

				if (A.etat[i, j] == 0)
				{
					i0 = i;
					j0 = j;
				}
			}
		}
		tbdb = i2.ToString();
		tbdb += j2.ToString();
		tbdb += i3.ToString();
		tbdb += j3.ToString();
		tbdb += i4.ToString();
		tbdb += j4.ToString();

		dbdb = i7.ToString();
		dbdb += j7.ToString();
		dbdb += i8.ToString();
		dbdb += j8.ToString();
		dbdb += i11.ToString();
		dbdb += j11.ToString();
		dbdb += i12.ToString();
		dbdb += j12.ToString();
		dbdb += i14.ToString();
		dbdb += j14.ToString();
		dbdb += i15.ToString();
		dbdb += j15.ToString();

		pbdb = i1.ToString();
		pbdb += j1.ToString();
		pbdb += i5.ToString();
		pbdb += j5.ToString();
		pbdb += i6.ToString();
		pbdb += j6.ToString();
		pbdb += i9.ToString();
		pbdb += j9.ToString();
		pbdb += i10.ToString();
		pbdb += j10.ToString();
		pbdb += i13.ToString();
		pbdb += j13.ToString();

		aret = PBDBS[pbdb] + DBDBS[dbdb] + TBDBS[tbdb];
		return aret;

	}
	public bool appartlc(int[] A, int B)
	{
		bool test = false;
		for (int i = 0; i < A.Length; i++)
		{
			if (A[i] == B)
			{
				test = true;
			}
		}
		return test;
	}
	public int postabin(int[] tabin, int B)
	{
		int indice = 0;

		for (int i = 0; i < tabin.Length; i++)
		{
			if (tabin[i] == B)
			{
				return i;
			}
		}
		return indice;
	}
	public List<int> nbrcasconfl(int[] tabin, int[] tabact, int nbr)
	{
		List<int> aret = new List<int>();


		for (int i = 0; i < tabact.Length; i++)
		{
			if (appartlc(tabin, tabact[i]) && appartlc(tabin, tabact[nbr]) && i != nbr && tabact[i]!=0 && tabact[nbr]!=0)
			{
				if (nbr < i && postabin(tabin, tabact[nbr]) > postabin(tabin, tabact[i]) || nbr > i && postabin(tabin, tabact[nbr]) < postabin(tabin, tabact[i]))
				{
					aret.Add(i);
				}
			}
		}
		return aret;
	}
	public int maxlincof(List<int> Tab)
	{
		List<int> Tab1 = new List<int>(Tab);
		Tab1.Sort((x, y) => x.CompareTo(y));
		return Tab1[Tab1.Count - 1];
	}
	public int lclignecol(Node B)
	{
		List<int[]> Tabcin = new List<int[]>(4);
		List<int[]> Tabc = new List<int[]>(4);
		List<int[]> Tablin = new List<int[]>(4);
		List<int[]> Tabl = new List<int[]>(4);
		List<List<int>> Tablinconf = new List<List<int>>();
		List<int> Tabvallinconf = new List<int>();
		int[] tc = new int[4];
		int[] tcn = new int[4];
		int[] tl = new int[4];
		int[] tln = new int[4];
		int nbrlinconf = 0;
		int indice = 0;
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				tl[i] = (B.etat[i, j]);
				tln[i] = (etatinitial[i, j]);
				tc[i] = (B.etat[j, i]);
				tcn[i] = (etatinitial[j, i]);
			}
			Tabc.Add(tc);
			tc = new int[4];
			Tabcin.Add(tcn);
			tcn = new int[4];
			Tabl.Add(tl);
			tl = new int[4];
			Tablin.Add(tln);
			tln = new int[4];

		}

		for (int i = 0; i < Tabl.Capacity; i++)
		{


			for (int j = 0; j < Tabl[i].Length; j++)
			{
				Tablinconf.Add(nbrcasconfl(Tablin[i], Tabl[i], j));
				Tabvallinconf.Add(Tablinconf[j].Count);
			}

			while (maxlincof(Tabvallinconf) != 0)
			{
				indice = Tabvallinconf.IndexOf(maxlincof(Tabvallinconf));
				Tabvallinconf[indice] = 0;
				for (int v = 0; v < Tablinconf[indice].Count; v++)
				{
					Tabvallinconf[Tablinconf[indice][v]] -= 1;
				}
				nbrlinconf += 1;
			}

			Tabvallinconf.Clear();
			Tablinconf.Clear();

		}
		Tabvallinconf.Clear();
		Tablinconf.Clear();
		for (int i = 0; i < Tabc.Capacity; i++)
		{


			for (int j = 0; j < Tabc[i].Length; j++)
			{
				Tablinconf.Add(nbrcasconfl(Tabcin[i], Tabc[i], j));
				Tabvallinconf.Add(Tablinconf[j].Count);
			}

			while (maxlincof(Tabvallinconf) != 0)
			{
				indice = Tabvallinconf.IndexOf(maxlincof(Tabvallinconf));
				Tabvallinconf[indice] = 0;
				for (int v = 0; v < Tablinconf[indice].Count; v++)
				{
					Tabvallinconf[Tablinconf[indice][v]] -= 1;
				}
				nbrlinconf += 1;
			}
			Tabvallinconf.Clear();
			Tablinconf.Clear();
		}
		return nbrlinconf;
	}
	public int linconflict(Node A)
	{
		return Manhattandistance(A) + 2 * lclignecol(A);
	}

	public int compareNode(Node A, Node B)
	{
		if (A.f > B.f)
		{
			return 1;
		}
		if (A.f < B.f)
		{
			return -1;
		}
		return 0;

	}
	public bool EstpresentNodexpl(Node A)
	{
		bool aret = false;
		for (int i = 0; i < Nodeexplorer.Count; i++)
		{
			if (compareEtat(A, Nodeexplorer[i]) == true)
			{
				aret = true;
				break;
			}
		}
		return aret;
	}
	public bool EstpresentNodenonexpl(Node A)
	{
		bool aret = false;
		for (int i = 0; i < Nodenonexplorer.Count; i++)
		{
			if (compareEtat(A, Nodenonexplorer[i]) == true)
			{
				aret = true;
				break;
			}
		}
		return aret;

	}
	public bool compareEtat(Node A, Node B)
	{
		bool aret = true;
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (A.etat[i, j] != B.etat[i, j])
				{
					aret = false;
					break;
				}
			}
		}
		return aret;
	}
	void supprimeNode()
	{
		Nodenonexplorer.RemoveAt(0);
	}
	void ajouteNode(Node A)
	{
		Nodenonexplorer.Add(A);
		Nodenonexplorer.Sort((x, y) => x.h.CompareTo(y.h));
	}
	CasePuzzle4 correspondancenumcase(int n)
	{
		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (TabCasePuzzle[i, j].GetComponent<CasePuzzle4>().nombre == n)
				{
					return TabCasePuzzle[i, j].GetComponent<CasePuzzle4>();
				}
			}
		}
		return null;
	}
	IEnumerator retracagesolution(Node A)
	{

		List<Node> rtrsoluce = new List<Node>();
		Node node = new Node(4);
		rtrsoluce.Add(A);
		node = A.ancetre;
		while (compareEtat(node, racine) == false)
		{
			rtrsoluce.Add(node);
			node = node.ancetre;
		}
		rtrsoluce.Add(node);
		rtrsoluce.Reverse();
		Debug.Log("Le nombre de coups a effectuer est de : " + rtrsoluce[rtrsoluce.Count - 1].g);
		for (int k = 0; k < rtrsoluce.Count - 1; k++)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					if (rtrsoluce[k].etat[i, j] != rtrsoluce[k + 1].etat[i, j] && rtrsoluce[k + 1].etat[i, j] == 0)
					{

						yield return new WaitForSeconds(30.0f * Time.deltaTime);
						correspondancenumcase(rtrsoluce[k].etat[i, j]).DeplacementCase(PosCasevide(correspondancenumcase(rtrsoluce[k].etat[i, j])));
					}

				}
			}
		}
	}
	public List<Node> succeseur(Node node)
	{
		List<Node> aret = new List<Node>();
		Node node1 = new Node(4);
		Node node2 = new Node(4);
		Node node3 = new Node(4);
		Node node4 = new Node(4);
		int tampon;
		Vector2 save = new Vector2();

		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				if (node.etat[i, j] == 0)
				{
					save = new Vector2(i, j);
				}
			}
		}
		if (save.x == 0 && save.y == 0)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 1];
			node1.etat[0, 1] = 0;
			node1.etat[0, 0] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;


			tampon = node2.etat[1, 0];
			node2.etat[1, 0] = 0;
			node2.etat[0, 0] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
		}
		else if (save.x == 0 && save.y == 1)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 0];
			node1.etat[0, 0] = 0;
			node1.etat[0, 1] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[0, 2];
			node2.etat[0, 2] = 0;
			node2.etat[0, 1] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;


			tampon = node3.etat[1, 1];
			node3.etat[1, 1] = 0;
			node3.etat[0, 1] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
		}
		else if (save.x == 0 && save.y == 2)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[1, 2];
			node1.etat[1, 2] = 0;
			node1.etat[0, 2] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[0, 1];
			node2.etat[0, 1] = 0;
			node2.etat[0, 2] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[0, 3];
			node3.etat[0, 3] = 0;
			node3.etat[0, 2] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
		}

		else if (save.x == 0 && save.y == 3)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[1, 3];
			node1.etat[1, 3] = 0;
			node1.etat[0, 3] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[0, 2];
			node2.etat[0, 2] = 0;
			node2.etat[0, 3] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
		}
		else if (save.x == 1 && save.y == 0)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 0];
			node1.etat[0, 0] = 0;
			node1.etat[1, 0] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[1, 1];
			node2.etat[1, 1] = 0;
			node2.etat[1, 0] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[2, 0];
			node3.etat[2, 0] = 0;
			node3.etat[1, 0] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
		}
		else if (save.x == 1 && save.y == 1)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];
					node4.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 1];
			node1.etat[0, 1] = 0;
			node1.etat[1, 1] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[1, 0];
			node2.etat[1, 0] = 0;
			node2.etat[1, 1] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[2, 1];
			node3.etat[2, 1] = 0;
			node3.etat[1, 1] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			tampon = node4.etat[1, 2];
			node4.etat[1, 2] = 0;
			node4.etat[1, 1] = tampon;
			node4.ancetre = node;
			node4.h = ChoixHeurst(node4);
			node4.g = node4.ancetre.g + 1;
			node4.f = node4.h + node4.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
			aret.Add(node4);
		}
		else if (save.x == 1 && save.y == 2)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];
					node4.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 2];
			node1.etat[0, 2] = 0;
			node1.etat[1, 2] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[1, 1];
			node2.etat[1, 1] = 0;
			node2.etat[1, 2] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[2, 2];
			node3.etat[2, 2] = 0;
			node3.etat[1, 2] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			tampon = node4.etat[1, 3];
			node4.etat[1, 3] = 0;
			node4.etat[1, 2] = tampon;
			node4.ancetre = node;
			node4.h = ChoixHeurst(node4);
			node4.g = node4.ancetre.g + 1;
			node4.f = node4.h + node4.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
			aret.Add(node4);
		}

		else if (save.x == 1 && save.y == 3)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[0, 3];
			node1.etat[0, 3] = 0;
			node1.etat[1, 3] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;


			tampon = node2.etat[1, 2];
			node2.etat[1, 2] = 0;
			node2.etat[1, 3] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;


			tampon = node3.etat[2, 3];
			node3.etat[2, 3] = 0;
			node3.etat[1, 3] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);

		}
		else if (save.x == 2 && save.y == 0)
		{

			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[2, 1];
			node1.etat[2, 1] = 0;
			node1.etat[2, 0] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[1, 0];
			node2.etat[1, 0] = 0;
			node2.etat[2, 0] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[3, 0];
			node3.etat[3, 0] = 0;
			node3.etat[2, 0] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node1.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
		}
		else if (save.x == 2 && save.y == 1)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];
					node4.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[1, 1];
			node1.etat[1, 1] = 0;
			node1.etat[2, 1] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[2, 0];
			node2.etat[2, 0] = 0;
			node2.etat[2, 1] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[2, 2];
			node3.etat[2, 2] = 0;
			node3.etat[2, 1] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			tampon = node4.etat[3, 1];
			node4.etat[3, 1] = 0;
			node4.etat[2, 1] = tampon;
			node4.ancetre = node;
			node4.h = ChoixHeurst(node4);
			node4.g = node4.ancetre.g + 1;
			node4.f = node4.h + node4.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
			aret.Add(node4);
		}
		else if (save.x == 2 && save.y == 2)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];
					node4.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[1, 2];
			node1.etat[1, 2] = 0;
			node1.etat[2, 2] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;


			tampon = node2.etat[2, 1];
			node2.etat[2, 1] = 0;
			node2.etat[2, 2] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[3, 2];
			node3.etat[3, 2] = 0;
			node3.etat[2, 2] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;


			tampon = node4.etat[2, 3];
			node4.etat[2, 3] = 0;
			node4.etat[2, 2] = tampon;
			node4.ancetre = node;
			node4.h = ChoixHeurst(node4);
			node4.g = node4.ancetre.g + 1;
			node4.f = node4.h + node4.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
			aret.Add(node4);
		}
		else if (save.x == 2 && save.y == 3)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[1, 3];
			node1.etat[1, 3] = 0;
			node1.etat[2, 3] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[2, 2];
			node2.etat[2, 2] = 0;
			node2.etat[2, 3] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			tampon = node3.etat[3, 3];
			node3.etat[3, 3] = 0;
			node3.etat[2, 3] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);


		}
		else if (save.x == 3 && save.y == 0)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[3, 1];
			node1.etat[3, 1] = 0;
			node1.etat[3, 0] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[2, 0];
			node2.etat[2, 0] = 0;
			node2.etat[3, 0] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
		}
		else if (save.x == 3 && save.y == 1)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[3, 0];
			node1.etat[3, 0] = 0;
			node1.etat[3, 1] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;

			tampon = node2.etat[3, 2];
			node2.etat[3, 2] = 0;
			node2.etat[3, 1] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;


			tampon = node3.etat[2, 1];
			node3.etat[2, 1] = 0;
			node3.etat[3, 1] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);
		}
		else if (save.x == 3 && save.y == 2)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];
					node3.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[2, 2];
			node1.etat[2, 2] = 0;
			node1.etat[3, 2] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;


			tampon = node2.etat[3, 1];
			node2.etat[3, 1] = 0;
			node2.etat[3, 2] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;


			tampon = node3.etat[3, 3];
			node3.etat[3, 3] = 0;
			node3.etat[3, 2] = tampon;
			node3.ancetre = node;
			node3.h = ChoixHeurst(node3);
			node3.g = node3.ancetre.g + 1;
			node3.f = node3.h + node3.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
			aret.Add(node3);

		}

		else if (save.x == 3 && save.y == 3)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					node1.etat[i, j] = node.etat[i, j];
					node2.etat[i, j] = node.etat[i, j];

				}
			}
			tampon = node1.etat[2, 3];
			node1.etat[2, 3] = 0;
			node1.etat[3, 3] = tampon;
			node1.ancetre = node;
			node1.h = ChoixHeurst(node1);
			node1.g = node1.ancetre.g + 1;
			node1.f = node1.h + node1.ancetre.g + 1;


			tampon = node2.etat[3, 2];
			node2.etat[3, 2] = 0;
			node2.etat[3, 3] = tampon;
			node2.ancetre = node;
			node2.h = ChoixHeurst(node2);
			node2.g = node2.ancetre.g + 1;
			node2.f = node2.h + node2.ancetre.g + 1;

			aret.Add(node1);
			aret.Add(node2);
		}
		return aret;
	}
	public void resolutionHamming()
	{

		choixheur = 1;
		resolution();

	}
	public void resolutionManhattan()
	{
		choixheur = 2;

		resolution();

	}
	public void resolutionlinconf()
	{
		choixheur = 3;

		resolution();

	}
	public void resolutionIDAlinconf()
	{
		choixheur = 3;

		ResolutionIDA();

	}
	public void resolutionIDAPB()
	{
		choixheur = 4;

		ResolutionIDA();

	}
	public void resolutionIDAPB6()
	{
		choixheur = 5;

		ResolutionIDA();

	}
	public void resolution()
	{
		stopWatch = new Stopwatch();
		stopWatch.Start();
		List<Node> succ = new List<Node>();
		racine = new Node(4);
		resultat = new Node(4);

		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				racine.etat[i, j] = returnpositiongrille(i, j).nombre;
				resultat.etat[i, j] = etatinitial[i, j];
			}
		}

		racine.h = ChoixHeurst(racine);
		Debug.Log("La distance estimée de la racine est : " + racine.h);
		Node node = new Node(4);

		ajouteNode(racine);

		while (compareEtat(node, resultat) == false)
		{

			succ = new List<Node>();

			node = Nodenonexplorer[0];

			supprimeNode();

			succ = succeseur(node);

			for (int i = 0; i < succ.Count; i++)
			{
				if (!EstpresentNodenonexpl(succ[i]) && !EstpresentNodexpl(succ[i]))
				{
					ajouteNode(succ[i]);
				}
			}

			Nodeexplorer.Add(node);

			if (compareEtat(node, resultat) == true)
			{
				Debug.Log("Solution Trouvée");
			}


		}
		stopWatch.Stop();
		Debug.Log("Le nombre de Noeuds  explorés a la fin : " + Nodeexplorer.Count);
		Debug.Log("Le nombre de Noeuds non explorés  a la fin : " + Nodenonexplorer.Count);
		TimeSpan ts = stopWatch.Elapsed;

		string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
		Debug.Log("temps ecoulé : " + elapsedTime);
		StartCoroutine(retracagesolution(node));
		Nodeexplorer.Clear();
		Nodenonexplorer.Clear();

	}

	public void ResolutionIDA()
	{
		stopWatch = new Stopwatch();
		stopWatch.Start();
		Solution = new Node(4);
		int threshold;
		int temp;
		racine = new Node(4);

		resultat = new Node(4);

		for (int j = Height - 1; j >= 0; j--)
		{
			for (int i = 0; i < Width; i++)
			{
				racine.etat[i, j] = returnpositiongrille(i, j).nombre;
				resultat.etat[i, j] = etatinitial[i, j];
			}
		}
		racine.h = ChoixHeurst(racine);
		Debug.Log("La distance estimée de la racine est : "+racine.h);
		threshold = ChoixHeurst(racine);
		Chemin.Add(racine);

		while (nbrboucle < 1500000000)
		{
			temp = rechercherecc( 0, threshold, resultat);
			if (temp == -10)
			{
				break;
			}

			threshold = temp;
		}
		stopWatch.Stop();
		Chemin.Clear();
		Debug.Log("Le nombre de noeuds rencontrés : "+nbrboucle);
		nbrboucle = 0;
		if (compareEtat(Solution, resultat) == true)
		{
			Debug.Log("Solution Trouvée");
			StartCoroutine(retracagesolution(Solution));
		}
		else
		{
			Debug.Log("Solution Non Trouvée Apres le Temps Imparti");
		}
		TimeSpan ts = stopWatch.Elapsed;

		string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
		Debug.Log("temps ecoulé : "+elapsedTime);
	}
	public int rechercherecc( int g, int treshold, Node resultat)
	{
		Node node = Chemin[Chemin.Count - 1];
		int f;
		List<Node> succ = new List<Node>();
		int temp = 0;
		int mini;
		f = g + ChoixHeurst(node);

		if (f > treshold)
		{
			return f;
		}

		if (compareEtat(node, resultat) == true)
		{
			Solution = node;
			return -10;
		}
		mini = 10000000;
		succ = succeseur(node);
		for (int i = 0; i < succ.Count; i++)
		{
			
			if (verifchemin(succ[i]) == false)
			{
				nbrboucle++;
				Chemin.Add(succ[i]);
				//Debug.Log("G "+ succ[i].g+" H "+ChoixHeurst(succ[i])+ " TRESHOLD "+ treshold);
				temp = rechercherecc(succ[i].g, treshold, resultat);
				if (temp == -10 || nbrboucle > 1500000000)
				{

					return -10;

				}
				if (temp < mini)
				{
					mini = temp;
				}
				Chemin.RemoveAt(Chemin.Count - 1);

			}
		}
		return mini;
	}
	public bool verifchemin(Node A)
	{
		bool aret = false;
		for (int i = 0; i < Chemin.Count; i++)
		{
			if (compareEtat(A, Chemin[i]) == true)
			{
				aret = true;
			}
		}
		return aret;
	}
	public void rightbutton()
	{
		nombrealeatoire += 1;
		niveau.text = "Niveau " + nombrealeatoire.ToString();
		StopAllCoroutines();
		choixheur = 0;
		Complet = false;
		if (TabCasePuzzle != null)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					Destroy(TabCasePuzzle[i, j]);
				}
			}
		}
		//nombrealeatoire = Random.Range(8,8);
		// fonction cree les cases des puzzles sans les mixer
		CreationCasePuzzle();
		// on mixe le puzzle au debut du jeu
		StartCoroutine(MixaleatoirePuzzle());
	}
	public void leftbutton()
	{
		nombrealeatoire -= 1;
		niveau.text = "Niveau " + nombrealeatoire.ToString();
		StopAllCoroutines();
		choixheur = 0;
		Complet = false;
		if (TabCasePuzzle != null)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					Destroy(TabCasePuzzle[i, j]);
				}
			}
		}
		//nombrealeatoire = Random.Range(8,8);
		// fonction cree les cases des puzzles sans les mixer
		CreationCasePuzzle();
		// on mixe le puzzle au debut du jeu
		StartCoroutine(MixaleatoirePuzzle());
	}
	public void restart()
	{
		nombrealeatoire = 7;
		niveau.text = "Niveau " + nombrealeatoire.ToString();
		StopAllCoroutines();
		choixheur = 0;
		Complet = false;
		if (TabCasePuzzle != null)
		{
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					Destroy(TabCasePuzzle[i, j]);
				}
			}
		}
		//nombrealeatoire = Random.Range(8,8);
		// fonction cree les cases des puzzles sans les mixer
		CreationCasePuzzle();
		// on mixe le puzzle au debut du jeu
		StartCoroutine(MixaleatoirePuzzle());
	}

	static Dictionary<string, int> Read(string file)
	{
		var result = new Dictionary<string, int>();
		using (FileStream fs = File.OpenRead(file))
		using (BinaryReader reader = new BinaryReader(fs))
		{
			// Get count.
			int count = reader.ReadInt32();
			Console.WriteLine(count);
			// Read in all pairs.
			for (int i = 0; i < count; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				result[key] = value;
			}
		}
		return result;
	}

}







