using System;
using System.Text;
using System.Threading;
using System.Timers;

/// <summary>
/// My console game on csharp, in which you can play tetris
/// </summary>
namespace Console_Game_CSharp
{
	/// <summary>
	/// main class
	/// </summary>
	internal class Tetris_Console_Game_CSharp
	{
		#region Variables

		private const int matrixWidth = 12;
		private const int matrixHeight = 16;
		private static int[][] tetrisGrid = new int[matrixWidth][];
		private static System.Timers.Timer aTimer;

		private const int gameWidth = matrixWidth + 3;
		private static Helper helper = new Helper();
		private static int Score = 0;
		private static int[,] currentShape;
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
			Console.WindowWidth = gameWidth + 2;
			Console.WindowHeight = matrixHeight;

			GameTetris tetris = new GameTetris();
			tetris.SetMatrix(tetrisGrid);
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
			aTimer.Elapsed += MovingShapesDown
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		/// <summary>
		///function, which is called once per second to find and move blocks
		/// </summary>
		/// <param name="sourse"></param>
		/// <param name="e"></param>
		private static void MovingShapesDown(Object sourse, ElapsedEventArgs e)
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

			Helper helper = new Helper();
			helper.SetShape(tetrisGrid, ref currentShape, nextShape, countOfBlocks);

			GameTetris.PrintingMatrix(tetrisGrid);
			countOfBlocks++;
		}

		/// <summary>
		/// for moving shapes
		/// </summary>
		/// <param name="movingRight"></param>
		private static void MovingShapesAway(int movingRight)
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
					if (!helper.CheckBorder(tetrisGrid, 4, 3, Side.left))
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

				//not working good
				case 2:
					if (!helper.CheckBorder(tetrisGrid, 4, 3, Side.rigth))
					{
						for (int i = 0; i < tetrisGrid.GetLength(0); i++)
						{
							for (int j = tetrisGrid.GetLength(1); j > 0; j--)
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

				//not working
				case 3:
					if (!helper.CheckBorder(tetrisGrid, 4, 3, Side.down))
					{
						for (int i = 0; i < tetrisGrid.GetLength(0); i++)
						{
							for (int j = 1; j < tetrisGrid.GetLength(1); j++)
							{
								if (tetrisGrid[i, j] == 3)
								{
									switch (tetrisGrid[i + 1, j])
									{
										case 1:
											int tl = tetrisGrid[i, j];
											tetrisGrid[i, j] = tetrisGrid[i + 1, j];
											tetrisGrid[i + 1, j] = tl;
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
		public const char Border = (char)178;

		/// <summary>
		/// start function for main matrix to change values in it
		/// </summary>
		/// <param name="matrix"></param>
		public void SetMatrix(int[,] tetrisGrid)
		{
			for (int i = 0; i < tetrisGrid.GetLength(0); i++)
			{
				for (int j = 0; j < tetrisGrid.GetLength(1); j++)
				{
					tetrisGrid[i, j] = 1;
					//1 is for empty space
					//2 is for bottom (if block will hit it-it stops)
					//3 is for blocks
					//4 is for delivered block
					if (i == 11)
					{
						tetrisGrid[i, j] = 2;
					}
				}
			}
		}

		/// <summary>
		/// prints matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void PrintingMatrix(int[,] tetrisGrid)
		{
			for (int i = 0; i < tetrisGrid.GetLength(0); i++)
			{
				for (int j = 0; j < tetrisGrid.GetLength(1); j++)
				{
					Console.Write(tetrisGrid[i, j]);
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

/// <summary>
/// helps with shapes
/// </summary>
internal class Helper
{
	public Random random = new Random();

	/// <summary>
	/// checks external numbers to correctly place shapes
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="border"></param>
	/// <param name="current"></param>
	/// <param name="side"></param>
	/// <returns></returns>
	public bool CheckBorder(int[,] tetrisGrid, int border, int current, Side side)
	{
		for (int i = 0; i < tetrisGrid.GetLength(0); i++)
		{
			for (int j = 0; j < tetrisGrid.GetLength(1); j++)
			{
				if (tetrisGrid[i, j] == current)
				{
					//TODO
					switch (side)
					{
						case Side.left:
							if (j == 0)
								return true;
							if (tetrisGrid[i, j - 1] == border)
								return true;
							break;

						case Side.rigth:
							if (j == 0)
								return true;
							if (tetrisGrid[i, j + 1] == border)
								return true;
							break;

						case Side.down:
							if (j == 0)
								return true;
							if (tetrisGrid[i + 1, j] == border)
								return true;
							break;

						case Side.up:
							if (j == 0)
								return true;
							if (tetrisGrid[i - 1, j] == border)
								return true;
							break;

						default:
							break;
					}
				}
			}
		}
		return false;
	}

	public void FindFullLines(int[,] tetrisGrid)
	{
		for (int i = 0; i < tetrisGrid.GetLength(0); i++)
		{
			for (int j = 0; j < tetrisGrid.GetLength(1); j++)
			{
			}
		}
	}

	/// <summary>
	///spawns shape
	/// </summary>
	/// <param name="tetrisGrid"></param>
	/// <param name="currentShape"></param>
	/// <param name="nextShape"></param>
	/// <param name="countOfBlocks"></param>
	public void SetShape(int[,] tetrisGrid, ref int[,] currentShape, int[,] nextShape,
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

	/// <summary>
	/// selects a random shape to place
	/// </summary>
	/// <param name="currentShape"></param>
	/// <returns></returns>
	private int[,] CreateShape(int[,] currentShape)
	{
		switch (random.Next(7))
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
			case 3:
				currentShape = new int[,] { { 3, 3, 3 }, { 1, 1, 3 } };
				break;
			case 4:
				currentShape = new int[,] { { 1, 1, 3 }, { 3, 3, 3 } };
				break;
			case 5:
				currentShape = new int[,] { { 3, 1, 3 }, { 3, 3, 3 } };
				break;
			case 6:
				currentShape = new int[,] { { 3, 3, 3 }, { 3, 1, 3 } };
				break;
			default:
				break;
		}
		return currentShape;
	}
}