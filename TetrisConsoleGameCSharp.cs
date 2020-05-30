using System;
using System.Collections.Generic;
using System.Drawing;
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
	//MAYBE NEED TO CHANGE THIS NAME TOO
	internal static class TetrisConsoleGameCSharp
	{
		#region Variables

		private const int matrixWidth = 12;
		private const int matrixHeight = 16;
		private static char[][] tetrisGrid = new char[matrixWidth][];
		private static System.Timers.Timer aTimer;

		private const int gameWidth = matrixWidth + 3;
		private static Helper helper = new Helper();
		private static int score;
		private static char[,] currentShape;
		private static int countOfBlocks;

		#endregion Variables

		/// <summary>
		/// Main function, where all cool things happen
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				tetrisGrid[i] = new char[matrixHeight];
			}
			Console.CursorVisible = false;
			Console.Title = "Tetris";
			Console.WindowWidth = gameWidth + 2;
			Console.WindowHeight = matrixHeight;

			GameTetris tetris = new GameTetris();
			tetris.SetMatrix(ref tetrisGrid, matrixHeight, matrixWidth);
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
				ConsoleKeyInfo button = Console.ReadKey();
				MovingShapesAway(button.Key, matrixHeight);
			}
		}

		/// <summary>
		/// setting timer
		/// </summary>
		public static void SetTimer()
		{
			aTimer = new System.Timers.Timer(1000);
			aTimer.Elapsed += MovingShapesDown;
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
			//moving 3 down
			for (int i = matrixWidth - 1; i >= 0; i--)
			{
				//Console.WriteLine(tetrisGrid.Length);
				List<Point> listOfElements = new List<Point>();
				//for (int j = 0; j < tetrisGrid.Lenght; j++)
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == '3')
					{
						switch (tetrisGrid[i + 1][j])
						{
							case '1':
								char tempMatrix = tetrisGrid[i][j];
								tetrisGrid[i][j] = tetrisGrid[i + 1][j];
								tetrisGrid[i + 1][j] = tempMatrix;
								listOfElements.Add(new Point(i, j));
								break;

							case '2':
							case '4':
								helper.ThreeTo4(ref tetrisGrid, matrixHeight, matrixWidth);
								countOfBlocks = 0;
								for (int z = 0; z < listOfElements.Count; z++)
								{
									tempMatrix = tetrisGrid[listOfElements[z].X][listOfElements[z].Y];
									tetrisGrid[listOfElements[z].X][listOfElements[z].Y] =
										tetrisGrid[listOfElements[z].X + 1][listOfElements[z].Y];
									tetrisGrid[listOfElements[z].X + 1][listOfElements[z].Y] = tempMatrix;
								}
								break;

							default:
								break;
						}
					}
				}
			}

			for (int i = 0; i < matrixWidth; i++)
			{
				int counterForLines = 0;
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == '4')
					{
						counterForLines++;
					}
				}
				if (counterForLines == matrixHeight)
				{
					helper.DeleteLine(tetrisGrid, i, matrixHeight);
					score++;
				}
			}
			helper.SetShape(ref tetrisGrid, ref currentShape, countOfBlocks);
			GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth);
			countOfBlocks++;
		}

		/// <summary>
		/// for moving shapes
		/// </summary>
		/// <param name="movingRight"></param>
		private static void MovingShapesAway(ConsoleKey button, int matrixHeight)
		{
			//to make entire figure stop and transform to 4,you can copy entire array to tempArray and if
			//case 2 or 4 is true than revert to tempArray, make it current and transform all 3 to 4

			//moving 3 down

			//moving 3 left and right
			if (countOfBlocks > 2)
			{
				switch (button)
				{
					case ConsoleKey.LeftArrow:
						if (!helper.CheckBorder(tetrisGrid, '4', '3', Side.left, matrixHeight, matrixWidth))
						{
							for (int i = 0; i < matrixWidth; i++)
							{
								for (int j = 1; j < matrixHeight; j++)
								{
									Console.WriteLine(tetrisGrid[i].Length);
									if (tetrisGrid[i][j] == '3')
									{
										switch (tetrisGrid[i][j - 1])
										{
											case '1':
												char tempMatrix = tetrisGrid[i][j];
												tetrisGrid[i][j] = tetrisGrid[i][j - 1];
												tetrisGrid[i][j - 1] = tempMatrix;
												break;

											default:
												break;
										}
									}
								}
							}
						}
						break;

					case ConsoleKey.RightArrow:
						if (!helper.CheckBorder(tetrisGrid, '4', '3', Side.rigth, matrixHeight, matrixWidth))
						{
							for (int i = 0; i < matrixWidth; i++)
							{
								int len = matrixHeight;
								for (int j = len - 1; j > 0; j--)
								{
									if (tetrisGrid[i][j] == '3')
									{
										switch (tetrisGrid[i][j + 1])
										{
											case '1':
												char tempMatrix = tetrisGrid[i][j];
												tetrisGrid[i][j] = tetrisGrid[i][j + 1];
												tetrisGrid[i][j + 1] = tempMatrix;
												break;

											default:
												break;
										}
									}
								}
							}
						}
						break;

					case ConsoleKey.DownArrow:
						if (!helper.CheckBorder(tetrisGrid, '4', '3', Side.down, matrixHeight, matrixWidth))
						{
							for (int i = matrixWidth - 1; i > 0; i--)
							{
								int len = matrixHeight;
								for (int j = 0; j < len; j++)
								{
									if (tetrisGrid[i][j] == '3')
									{
										switch (tetrisGrid[i + 1][j])
										{
											case '1':
												char tempMatrix = tetrisGrid[i][j];
												tetrisGrid[i][j] = tetrisGrid[i + 1][j];
												tetrisGrid[i + 1][j] = tempMatrix;
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
			}
			GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth);
		}
	}

	/// <summary>
	///for tetris logic
	/// </summary>
	internal class GameTetris
	{
		public const char Border = (char)178;
		private static Object locker = new object();

		/// <summary>
		/// start function for main matrix to change values in it
		/// </summary>
		/// <param name="matrix"></param>
		public void SetMatrix(ref char[][] tetrisGrid, int matrixHeight, int matrixWidth)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				//int len = tetrisGrid[i].Length;
				for (int j = 0; j < matrixHeight; j++)
				{
					tetrisGrid[i][j] = '1';
					//1 is for empty space
					//2 is for bottom (if block will hit it-it stops)
					//3 is for blocks
					//4 is for delivered block
					if (i == 11)
					{
						tetrisGrid[i][j] = '2';
					}
				}
			}
		}

		/// <summary>
		/// prints matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void PrintingMatrix(char[][] tetrisGrid, int score,int matrixWidth)
		{
			lock (locker)
			{
				Console.Clear();
				for (int i = 0; i < matrixWidth; i++)
				{
					Console.WriteLine(tetrisGrid[i]);
				}
				Console.WriteLine("Score: " + score);
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

//NEED TO CHANGE NAME FOR HELPER
internal class Helper
{
	public Random random = new Random();

	/// <summary>
	/// checks external numbers to correctly place shapes
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="border"></param>
	/// <param name="currentElementOfMatrix"></param>
	/// <param name="side"></param>
	/// <returns></returns>
	public bool CheckBorder(char[][] tetrisGrid, char border,
		char currentElementOfMatrix, Side side, int matrixHeight, int matrixWidth)
	{
		for (int i = 0; i < matrixWidth; i++)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
			{
				if (tetrisGrid[i][j] == currentElementOfMatrix)
				{
					switch (side)
					{
						case Side.left:
							if (j == 0)
								return true;
							if (tetrisGrid[i][j - 1] == border)
								return true;
							break;

						case Side.rigth:
							if (j == matrixHeight - 1)
								return true;
							if (tetrisGrid[i][j + 1] == border)
								return true;
							break;

						case Side.down:
							if (i == matrixWidth)
								return true;
							if (tetrisGrid[i][j] == border)
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

	public void DeleteLine(char[][] tetrisGrid, int line, int matrixHeight)
	{
		for (int i = line; i > 0; i--)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
				tetrisGrid[i][j] = tetrisGrid[i - 1][j];
		}
	}

	public void ThreeTo4(ref char[][] tetrisGrid, int matrixHeight, int matrixWidth)
	{
		for (int i = 0; i < matrixWidth; i++)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
				if (tetrisGrid[i][j] == '3')
					tetrisGrid[i][j] = '4';
		}
	}

	public void FindFullLines(int[][] tetrisGrid, int matrixHeight, int matrixWidth)
	{
		for (int i = 0; i < matrixWidth; i++)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
			{
			}
		}
	}

	/// <summary>
	///spawns shape
	/// </summary>
	/// <param name="tetrisGrid"></param>
	/// <param name="currentShape"></param>
	/// <param name="countOfBlocks"></param>
	public void SetShape(ref char[][] tetrisGrid, ref char[,] currentShape,
	int countOfBlocks)
	{
		switch (countOfBlocks)
		{
			case 0:
				currentShape = CreateShape(currentShape);
				for (int i = 6; i < 9; i++)
					tetrisGrid[0][i] = currentShape[0, i - 6];
				break;

			case 1:
				for (int i = 6; i < 9; i++)
					tetrisGrid[0][i] = currentShape[1, i - 6];
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
	private char[,] CreateShape(char[,] currentShape)
	{
		switch (random.Next(7))
		{
			case 0:
				currentShape = new char[,] { { '3', '1', '1' }, { '3', '1', '1' } };
				break;

			case 1:
				currentShape = new char[,] { { '3', '3', '1' }, { '3', '3', '1' } };
				break;

			case 2:
				currentShape = new char[,] { { '3', '3', '3' }, { '3', '3', '3' } };
				break;

			case 3:
				currentShape = new char[,] { { '3', '3', '3' }, { '1', '1', '3' } };
				break;

			case 4:
				currentShape = new char[,] { { '1', '1', '3' }, { '3', '3', '3' } };
				break;

			case 5:
				currentShape = new char[,] { { '3', '1', '3' }, { '3', '3', '3' } };
				break;

			case 6:
				currentShape = new char[,] { { '3', '3', '3' }, { '3', '1', '3' } };
				break;

			default:
				break;
		}
		return currentShape;
	}
}