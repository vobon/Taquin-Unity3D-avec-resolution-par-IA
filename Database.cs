
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace database
{
	public class Program
	{
		public Dictionary<string, int> Nodeexplorer = new Dictionary<string, int>(6000000);
		public Dictionary<string, int> final = new Dictionary<string, int>(600000);
		public Queue<Node> Nodenonexplorer = new Queue<Node>(6000000);
		public Node racine;
		public Node PremBloc = new Node(4);
		public Node deuxBloc = new Node(4);
		public Node troisBloc = new Node(4);
		public int Height = 4;
		public int Width = 4;
	
			
		


		public List<Node> succeseur(Node node)
		{
			List<Node> aret = new List<Node>(4);
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
			if (save.X == 0 && save.Y == 0)
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



				tampon = node2.etat[1, 0];
				node2.etat[1, 0] = 0;
				node2.etat[0, 0] = tampon;
				node2.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
			}
			else if (save.X == 0 && save.Y == 1)
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


				tampon = node2.etat[0, 2];
				node2.etat[0, 2] = 0;
				node2.etat[0, 1] = tampon;
				node2.ancetre = node;



				tampon = node3.etat[1, 1];
				node3.etat[1, 1] = 0;
				node3.etat[0, 1] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
			}
			else if (save.X == 0 && save.Y == 2)
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


				tampon = node2.etat[0, 1];
				node2.etat[0, 1] = 0;
				node2.etat[0, 2] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[0, 3];
				node3.etat[0, 3] = 0;
				node3.etat[0, 2] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
			}

			else if (save.X == 0 && save.Y == 3)
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


				tampon = node2.etat[0, 2];
				node2.etat[0, 2] = 0;
				node2.etat[0, 3] = tampon;
				node2.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
			}
			else if (save.X == 1 && save.Y == 0)
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


				tampon = node2.etat[1, 1];
				node2.etat[1, 1] = 0;
				node2.etat[1, 0] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[2, 0];
				node3.etat[2, 0] = 0;
				node3.etat[1, 0] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
			}
			else if (save.X == 1 && save.Y == 1)
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


				tampon = node2.etat[1, 0];
				node2.etat[1, 0] = 0;
				node2.etat[1, 1] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[2, 1];
				node3.etat[2, 1] = 0;
				node3.etat[1, 1] = tampon;
				node3.ancetre = node;


				tampon = node4.etat[1, 2];
				node4.etat[1, 2] = 0;
				node4.etat[1, 1] = tampon;
				node4.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
				aret.Add(node4);
			}
			else if (save.X == 1 && save.Y == 2)
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


				tampon = node2.etat[1, 1];
				node2.etat[1, 1] = 0;
				node2.etat[1, 2] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[2, 2];
				node3.etat[2, 2] = 0;
				node3.etat[1, 2] = tampon;
				node3.ancetre = node;


				tampon = node4.etat[1, 3];
				node4.etat[1, 3] = 0;
				node4.etat[1, 2] = tampon;
				node4.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
				aret.Add(node4);
			}

			else if (save.X == 1 && save.Y == 3)
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



				tampon = node2.etat[1, 2];
				node2.etat[1, 2] = 0;
				node2.etat[1, 3] = tampon;
				node2.ancetre = node;



				tampon = node3.etat[2, 3];
				node3.etat[2, 3] = 0;
				node3.etat[1, 3] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);

			}
			else if (save.X == 2 && save.Y == 0)
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


				tampon = node2.etat[1, 0];
				node2.etat[1, 0] = 0;
				node2.etat[2, 0] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[3, 0];
				node3.etat[3, 0] = 0;
				node3.etat[2, 0] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
			}
			else if (save.X == 2 && save.Y == 1)
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


				tampon = node2.etat[2, 0];
				node2.etat[2, 0] = 0;
				node2.etat[2, 1] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[2, 2];
				node3.etat[2, 2] = 0;
				node3.etat[2, 1] = tampon;
				node3.ancetre = node;


				tampon = node4.etat[3, 1];
				node4.etat[3, 1] = 0;
				node4.etat[2, 1] = tampon;
				node4.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
				aret.Add(node4);
			}
			else if (save.X == 2 && save.Y == 2)
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



				tampon = node2.etat[2, 1];
				node2.etat[2, 1] = 0;
				node2.etat[2, 2] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[3, 2];
				node3.etat[3, 2] = 0;
				node3.etat[2, 2] = tampon;
				node3.ancetre = node;



				tampon = node4.etat[2, 3];
				node4.etat[2, 3] = 0;
				node4.etat[2, 2] = tampon;
				node4.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
				aret.Add(node4);
			}
			else if (save.X == 2 && save.Y == 3)
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


				tampon = node2.etat[2, 2];
				node2.etat[2, 2] = 0;
				node2.etat[2, 3] = tampon;
				node2.ancetre = node;


				tampon = node3.etat[3, 3];
				node3.etat[3, 3] = 0;
				node3.etat[2, 3] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);


			}
			else if (save.X == 3 && save.Y == 0)
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


				tampon = node2.etat[2, 0];
				node2.etat[2, 0] = 0;
				node2.etat[3, 0] = tampon;
				node2.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
			}
			else if (save.X == 3 && save.Y == 1)
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


				tampon = node2.etat[3, 2];
				node2.etat[3, 2] = 0;
				node2.etat[3, 1] = tampon;
				node2.ancetre = node;



				tampon = node3.etat[2, 1];
				node3.etat[2, 1] = 0;
				node3.etat[3, 1] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);
			}
			else if (save.X == 3 && save.Y == 2)
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



				tampon = node2.etat[3, 1];
				node2.etat[3, 1] = 0;
				node2.etat[3, 2] = tampon;
				node2.ancetre = node;



				tampon = node3.etat[3, 3];
				node3.etat[3, 3] = 0;
				node3.etat[3, 2] = tampon;
				node3.ancetre = node;


				aret.Add(node1);
				aret.Add(node2);
				aret.Add(node3);

			}

			else if (save.X == 3 && save.Y == 3)
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



				tampon = node2.etat[3, 2];
				node2.etat[3, 2] = 0;
				node2.etat[3, 3] = tampon;
				node2.ancetre = node;

				aret.Add(node1);
				aret.Add(node2);
			}
			return aret;

		}
		public string PBpatterntostringPB(Node A)
		{
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
			int i0 = 0;
			int j0 = 0;
			string aret;
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

					if (A.etat[i, j] == 0)
					{
						i0 = i;
						j0 = j;
					}
				}
			}

			aret = i1.ToString();
			aret += j1.ToString();
			aret += i2.ToString();
			aret += j2.ToString();
			aret += i3.ToString();
			aret += j3.ToString();
			aret += i4.ToString();
			aret += j4.ToString();
			aret += i5.ToString();
			aret += j5.ToString();
			aret += i0.ToString();
			aret += j0.ToString();

			return aret;
		}
		public string PBpatterntostringDB(Node A)
		{
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
			int i0 = 0;
			int j0 = 0;
			string aret;
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{

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


					if (A.etat[i, j] == 0)
					{
						i0 = i;
						j0 = j;
					}
				}
			}

			aret = i6.ToString();
			aret += j6.ToString();
			aret += i7.ToString();
			aret += j7.ToString();
			aret += i8.ToString();
			aret += j8.ToString();
			aret += i11.ToString();
			aret += j11.ToString();
			aret += i12.ToString();
			aret += j12.ToString();
			aret += i0.ToString();
			aret += j0.ToString();

			return aret;
		}
		public string PBpatterntostringTB(Node A)
		{
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
			string aret;
			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{

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

			aret = i9.ToString();
			aret += j9.ToString();
			aret += i10.ToString();
			aret += j10.ToString();
			aret += i13.ToString();
			aret += j13.ToString();
			aret += i14.ToString();
			aret += j14.ToString();
			aret += i15.ToString();
			aret += j15.ToString();
			aret += i0.ToString();
			aret += j0.ToString();

			return aret;
		}
		public int interversionPB(Node A)
		{
			int aret = 0;
			int savei = 0;
			int savej = 0;

			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					if (A.ancetre.etat[i, j] == 0)
					{
						savei = i;
						savej = j;
					}
				}
			}
			if (A.etat[savei, savej] == 1 || A.etat[savei, savej] == 5 || A.etat[savei, savej] == 2 || A.etat[savei, savej] == 3 || A.etat[savei, savej] == 4 )
			{
				aret = 1;
			}
			return aret;
		}
		public int interversionDB(Node A)
		{
			int aret = 0;
			int savei = 0;
			int savej = 0;

			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					if (A.ancetre.etat[i, j] == 0)
					{
						savei = i;
						savej = j;
					}
				}
			}
			if (A.etat[savei, savej] == 7 || A.etat[savei, savej] == 8 || A.etat[savei, savej] == 6 || A.etat[savei, savej] == 12 || A.etat[savei, savej] == 11 )
			{
				aret = 1;
			}
			return aret;
		}
		public int interversionTB(Node A)
		{
			int aret = 0;
			int savei = 0;
			int savej = 0;

			for (int j = Height - 1; j >= 0; j--)
			{
				for (int i = 0; i < Width; i++)
				{
					if (A.ancetre.etat[i, j] == 0)
					{
						savei = i;
						savej = j;
					}
				}
			}
			if (A.etat[savei, savej] == 9 || A.etat[savei, savej] == 10 || A.etat[savei, savej] == 13 || A.etat[savei, savej] == 14 || A.etat[savei, savej] == 15)
			{
				aret = 1;
			}
			return aret;
		}
		public void RechercheProfond()
		{
			Node node = new Node(4);
			string save;
			List<Node> succ = new List<Node>(4);
			racine = new Node(4);
			PremBloc.etat[0, 0] = -1;
			PremBloc.etat[0, 1] = -1;
			PremBloc.etat[0, 2] = 5;
			PremBloc.etat[0, 3] = 1;
			PremBloc.etat[1, 0] = -1;
			PremBloc.etat[1, 1] = -1;
			PremBloc.etat[1, 2] = -1;
			PremBloc.etat[1, 3] = 2;
			PremBloc.etat[2, 0] = -1;
			PremBloc.etat[2, 1] = -1;
			PremBloc.etat[2, 2] = -1;
			PremBloc.etat[2, 3] = 3;
			PremBloc.etat[3, 0] = 0;
			PremBloc.etat[3, 1] = -1;
			PremBloc.etat[3, 2] = -1;
			PremBloc.etat[3, 3] = 4;

			deuxBloc.etat[0, 0] = -1;
			deuxBloc.etat[0, 1] = -1;
			deuxBloc.etat[0, 2] = -1;
			deuxBloc.etat[0, 3] = -1;
			deuxBloc.etat[1, 0] = -1;
			deuxBloc.etat[1, 1] = -1;
			deuxBloc.etat[1, 2] = 6;
			deuxBloc.etat[1, 3] = -1;
			deuxBloc.etat[2, 0] = -1;
			deuxBloc.etat[2, 1] = 11;
			deuxBloc.etat[2, 2] = 7;
			deuxBloc.etat[2, 3] = -1;
			deuxBloc.etat[3, 0] = 0;
			deuxBloc.etat[3, 1] = 12;
			deuxBloc.etat[3, 2] = 8;
			deuxBloc.etat[3, 3] = -1;


			troisBloc.etat[0, 0] = 13;
			troisBloc.etat[0, 1] = 9;
			troisBloc.etat[0, 2] = -1;
			troisBloc.etat[0, 3] = -1;
			troisBloc.etat[1, 0] = 14;
			troisBloc.etat[1, 1] = 10;
			troisBloc.etat[1, 2] = -1;
			troisBloc.etat[1, 3] = -1;
			troisBloc.etat[2, 0] = 15;
			troisBloc.etat[2, 1] = -1;
			troisBloc.etat[2, 2] = -1;
			troisBloc.etat[2, 3] = -1;
			troisBloc.etat[3, 0] = 0;
			troisBloc.etat[3, 1] = -1;
			troisBloc.etat[3, 2] = -1;
			troisBloc.etat[3, 3] = -1;

			racine = troisBloc;

			racine.h = 0;

			Nodenonexplorer.Enqueue(racine);
			Nodeexplorer.Add(PBpatterntostringTB(racine), racine.h);

			while (Nodeexplorer.Count < 5765760)
			{
				node = Nodenonexplorer.Dequeue();
				

				succ = succeseur(node);


				for (int i = 0; i < succ.Count; i++)
				{
					save = PBpatterntostringTB(succ[i]);
					if (Nodeexplorer.ContainsKey(save) == false)
					{
						succ[i].h = succ[i].ancetre.h + interversionTB(succ[i]);
						Nodeexplorer.Add(save, succ[i].h);
						Nodenonexplorer.Enqueue(succ[i]);
					}
				}
				succ.Clear();
				Console.WriteLine("Count TB: " + Nodeexplorer.Count);
			}
			Nodenonexplorer.Clear();

		}
		public void selection()
		{
			List<string> stKey = new List<string>(6000000);
			stKey = Nodeexplorer.Keys.ToList();
			stKey.Sort((x, y) => x.CompareTo(y));
			int n = 0;
			int valeur;
			string cle;
			string result;
			while (n < stKey.Count)
			{
				cle = stKey[n];
				valeur = Nodeexplorer[cle];
				for (int i = 0; i < 11; i++)
				{
					if (Nodeexplorer[cle] > Nodeexplorer[stKey[n + i]])
					{
						cle = stKey[n + i];
						valeur = Nodeexplorer[stKey[n + i]];
					}
				}
				result = cle.Substring(0, 10);
				if (!final.ContainsKey(result))
				{
					final.Add(result, valeur);
				}
				n += 11;
			}
			stKey.Clear();
			Nodeexplorer.Clear();
			Console.WriteLine(final.Count);
		}
		
		static void Write(Dictionary<string, int> dictionary, string file)
		{
			using (FileStream fs = File.OpenWrite(file))
			using (BinaryWriter writer = new BinaryWriter(fs))
			{
				// Put count.
				writer.Write(dictionary.Count);
				// Write pairs.
				foreach (var pair in dictionary)
				{
					writer.Write(pair.Key);
					writer.Write(pair.Value);
				}
			}
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
		public void creationfichier()
		{
			RechercheProfond();
			selection();
			Write(final, "C:\\Users\\rughs\\OneDrive\\Bureau\\TBDB.bin");
			//selectionDB();
			//Write(DBDB, "C:\\Users\\rughs\\OneDrive\\Bureau\\DBDB.bin");
			//selectionTB();
			//Write(TBDB, "C:\\Users\\rughs\\OneDrive\\Bureau\\TBDB.bin");
		}
		static void Main()
		{
			Program c1 = new Program();
			c1.creationfichier();
		}
	}

}