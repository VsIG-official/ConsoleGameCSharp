using System;
using System.Threading;
using System.Timers;

/// <summary>
/// My console game on csharp, in which you can play tetris
/// </summary>
namespace Console_Game_CSharp
{
	internal class Tetris_Console_Game_CSharp
	{
		#region Variables

		private static int[,] tetrisGrid = new int[12, 16];
		private static System.Timers.Timer aTimer;

		private const int InfoPanelWidth = 10;
		private const int TetrisWidth = 12;
		private const int TetrisHeight = 16;

		private const int GameWidth = TetrisWidth +
		InfoPanelWidth + 3;

		//const int GameHeight = TetrisHeight + 2;
		private int score;

		private static int[,] currentShape;
		private static int[,] nextShape;
		private static int countOfBlocks;

		#endregion Variables

		/// <summary>
		/// Main function, where all cool things happen
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Console.Title = "Tetris";
			Console.WindowWidth = GameWidth;
			Console.WindowHeight = TetrisHeight;

			Game_Tetris tetris = new Game_Tetris();
			tetris.MakingMatrix(tetrisGrid);
			SetTimer();
			new Thread(NewThread).Start();
		}

		/// <summary>
		/// infinite stream for working algorithm (moving blocks)
		/// </summary>
		private static void NewThread()
		{
			while (true)
			{
				ConsoleKeyInfo key = Console.ReadKey();
				if (key.Key == ConsoleKey.LeftArrow)
				{
					UpDateBorder(1);
				}
				else if (key.Key == ConsoleKey.RightArrow)
				{
					UpDateBorder(2);
				}
			}
		}

		/// <summary>
		/// setting timer
		/// </summary>
		public static void SetTimer()
		{
			aTimer = new System.Timers.Timer(1000);
			aTimer.Elapsed += UpDateDown;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		/// <summary>
		///function, which is called once per second to find and move blocks
		/// </summary>
		/// <param name="sourse"></param>
		/// <param name="e"></param>
		private static void UpDateDown(Object sourse, ElapsedEventArgs e)
		{
			//Console.WriteLine(tetrisGrid.GetLength(0)/*height*/ + "+" + tetrisGrid.GetLength(1)/*width*/);
			//to make entire figure stop and transform to 4,you can copy entire array to tempArray and if
			//case 2 or 4 is true than revert to tempArray, make it current and transform all 3 to 4

			//moving 3 down
			for (int i = tetrisGrid.GetLength(0) - 2; i >= 0; i--)
			{
				for (int j = 0; j < tetrisGrid.GetLength(1); j++)
				{
					if (tetrisGrid[i, j] == 3)
					{
						switch (tetrisGrid[i + 1, j])
						{
							case 1:
								int t = tetrisGrid[i, j];
								tetrisGrid[i, j] = tetrisGrid[i + 1, j];
								tetrisGrid[i + 1, j] = t;
								break;

							case 2:
								tetrisGrid[i, j] = 4;
								countOfBlocks = 0;
								break;

							case 4:
								tetrisGrid[i, j] = 4;
								countOfBlocks = 0;
								break;

							default:
								break;
						}
					}
				}
			}

			Console.Clear();
			Helper helper = new Helper();
			helper.GenerateShape(tetrisGrid, ref currentShape, nextShape, countOfBlocks);

			Game_Tetris.PrintingMatrix(tetrisGrid);
			countOfBlocks++;
		}

		private static void UpDateBorder(int movingRight)
		{
			//Console.WriteLine(tetrisGrid.GetLength(0)/*height*/ + "+" + tetrisGrid.GetLength(1)/*width*/);
			//to make entire figure stop and transform to 4,you can copy entire array to tempArray and if
			//case 2 or 4 is true than revert to tempArray, make it current and transform all 3 to 4

			//moving 3 down

			Helper helper = new Helper();
			//moving 3 left and right
			switch (movingRight)
			{
				case 0:
					break;

				case 1:
					if (!helper.ChekBorder(tetrisGrid, 4, 3, Side.left))
					{
						for (int i = 0; i < tetrisGrid.GetLength(0); i++)
						{
							for (int j = 1; j < tetrisGrid.GetLength(1); j++)
							{
								if (tetrisGrid[i, j] == 3)
								{
									switch (tetrisGrid[i, j - 1])
									{
										case 1:
											int tl = tetrisGrid[i, j];
											tetrisGrid[i, j] = tetrisGrid[i, j - 1];
											tetrisGrid[i, j - 1] = tl;
											break;

										case 2:
											break;

										case 4:
											break;

										default:
											break;
									}
								}
							}
						}
					}
					break;

				case 2:
					if (!helper.ChekBorder(tetrisGrid, 4, 3, Side.rigth))
					{
						for (int i = 0; i < tetrisGrid.GetLength(0); i++)
						{
							for (int j = 1; j < tetrisGrid.GetLength(1); j++)
							{
								if (tetrisGrid[i, j] == 3)
								{
									switch (tetrisGrid[i, j + 1])
									{
										case 1:
											int tl = tetrisGrid[i, j];
											tetrisGrid[i, j] = tetrisGrid[i, j + 1];
											tetrisGrid[i, j + 1] = tl;
											break;

										case 2:
											break;

										case 4:
											break;

										default:
											break;
									}
								}
							}
						}
					}
					break;

				default:
					break;
			}

			Console.Clear();
			countOfBlocks++;
			movingRight = 0;
			Game_Tetris.PrintingMatrix(tetrisGrid);
		}
	}

	/// <summary>
	///for tetris logic
	/// </summary>
	internal class Game_Tetris
	{
		//public const char Border = (char)178;

		/// <summary>
		/// start function for main matrix to change values in it
		/// </summary>
		/// <param name="matrix"></param>
		public void MakingMatrix(int[,] matrix)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] = 1;
					//1 is for empty space
					//2 is for bottom (if block will hit it-it stops)
					//3 is for blocks
					//4 is for delivered block
					if (i == 11)
					{
						matrix[i, j] = 2;
					}
				}
			}
		}

		/// <summary>
		/// prints matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void PrintingMatrix(int[,] matrix)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					Console.Write(matrix[i, j]);
				}
				Console.WriteLine();
			}
		}
	}
}

internal enum Side
{
	left,
	rigth,
	down,
	up,
}

internal class Helper
{
	public Random random = new Random();

	public bool ChekBorder(int[,] matrix, int border, int current, Side side)
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				if (matrix[i, j] == current)
				{
					//TODO
					switch (side)
					{
						case Side.left:
							if (j == 0)
								return true;
							if (matrix[i, j - 1] == border)
								return true;
							break;

						case Side.rigth:
							if (j == 0)
								return true;
							if (matrix[i, j + 1] == border)
								return true;
							break;

						case Side.down:
							break;

						case Side.up:
							break;

						default:
							break;
					}
				}
			}
		}
		return false;
	}

	public void GenerateShape(int[,] tetrisGrid, ref int[,] currentShape, int[,] nextShape,
	int countOfBlocks)
	{
		switch (countOfBlocks)
		{
			case 0:
				currentShape = CreateShape(currentShape);
				Array.Copy(currentShape, 0, tetrisGrid, 6, 3);
				break;

			case 1:
				Array.Copy(currentShape, 3, tetrisGrid, 6, 3);
				break;

			default:
				break;
		}
	}

	private int[,] CreateShape(int[,] currentShape)
	{
		switch (random.Next(3))
		{
			case 0:
				currentShape = new int[,] { { 3, 1, 1 }, { 3, 1, 1 } };
				break;

			case 1:
				currentShape = new int[,] { { 3, 3, 1 }, { 3, 3, 1 } };
				break;

			case 2:
				currentShape = new int[,] { { 3, 3, 3 }, { 3, 3, 3 } };
				break;
			//case 3:
			//	currentShape = new int[,] { { 3, 3, 3 }, { 1, 1, 3 } };
			//	break;
			//case 4:
			//	currentShape = new int[,] { { 1, 1, 3 }, { 3, 3, 3 } };
			//	break;
			default:
				break;
		}
		return currentShape;
	}
}