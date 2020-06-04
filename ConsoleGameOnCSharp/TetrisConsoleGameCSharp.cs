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

		private const int matrixWidth = 15;
		private const int matrixHeight = 21;
		private const int whereToSpawn = 6;
		private const int heightOfShapes = 3;//use this if Your blocks don't have same width and height
		private const int widthOfShapes = 3;
		private const int bonusWidthOfTheScreen = 6;
		private const char freeSpace = ' ';
		private const char boundary = '─';
		private const char shapes = '█';
		private const char placedShapes = '#';
		private static char[][] tetrisGrid = new char[matrixWidth][];
		private static System.Timers.Timer aTimer;
		private static Thread thread = new Thread(new ThreadStart(NewThread));

		private const int gameWidth = matrixWidth + bonusWidthOfTheScreen;
		private static Helper helper = new Helper(matrixWidth, matrixWidth);
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
			Console.WindowWidth = gameWidth;
			Console.WindowHeight = matrixHeight;

			GameTetris tetris = new GameTetris();
			tetris.SetMatrix(ref tetrisGrid, matrixHeight, matrixWidth, freeSpace, boundary);
			SetTimer();
			thread.Start();
		}

		/// <summary>
		/// infinite stream for algorithm (moving blocks)
		/// </summary>
		private static void NewThread()
		{
			while (true)
			{
				ConsoleKeyInfo button = Console.ReadKey();
				MovingShapesAway(button.Key);
			}
		}

		/// <summary>
		/// Sets the timer.
		/// </summary>
		public static void SetTimer()
		{
			aTimer = new System.Timers.Timer(1000);
			aTimer.Elapsed += MovingShapesDown;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		/// <summary>
		/// Move the shapes down and checks for game over
		/// </summary>
		/// <param name="sourse">The sourse.</param>
		/// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
		private static void MovingShapesDown(Object sourse, ElapsedEventArgs e)
		{
			//moving 3 down
			for (int i = matrixWidth - 1; i >= 0; i--)
			{
				List<Point> listOfElements = new List<Point>();
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						switch (tetrisGrid[i + 1][j])
						{
							case freeSpace:
								char tempMatrix = tetrisGrid[i][j];
								tetrisGrid[i][j] = tetrisGrid[i + 1][j];
								tetrisGrid[i + 1][j] = tempMatrix;
								listOfElements.Add(new Point(i, j));
								break;

							case boundary:
							case placedShapes:
								helper.Convert3To4(ref tetrisGrid, shapes, placedShapes);
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
					if (tetrisGrid[i][j] == placedShapes)
					{
						counterForLines++;
					}
				}
				if (counterForLines == matrixHeight)
				{
					helper.DeleteLine(tetrisGrid, i);
					score++;
				}
			}
			bool gameOver = true;
			for (int i = 0; i < matrixHeight; i++)
			{
				if (tetrisGrid[2][i] == placedShapes)
				{
					Console.Clear();
					Console.WriteLine("Game over! you loss!");
					Console.WriteLine(score);
					aTimer.Stop();
					thread.Abort();
					gameOver = false;
				}
			}
			if (gameOver)
			{
				helper.SetShape(ref tetrisGrid, ref currentShape, countOfBlocks, whereToSpawn, widthOfShapes, shapes, freeSpace);
				GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth);
				countOfBlocks++;
			}
		}

		/// <summary>
		/// Movings the shapes away.
		/// </summary>
		/// <param name="button">The button.</param>
		/// <param name="matrixHeight">Height of the matrix.</param>
		private static void MovingShapesAway(ConsoleKey button)
		{
			if (countOfBlocks > heightOfShapes)
			{
				switch (button)
				{
					case ConsoleKey.LeftArrow:
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.left))
						{
							helper.MoveLeft(ref tetrisGrid, shapes, freeSpace);
						}
						break;

					case ConsoleKey.RightArrow:
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.rigth))
						{
							helper.MoveRight(ref tetrisGrid, shapes, freeSpace);
						}
						break;

					case ConsoleKey.DownArrow:
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.down))
						{
							helper.MoveDown(ref tetrisGrid, shapes,freeSpace,boundary, placedShapes, countOfBlocks);
						}
						break;
					case ConsoleKey.UpArrow:
						helper.RotateUp(ref tetrisGrid, shapes, freeSpace, boundary, placedShapes,heightOfShapes, widthOfShapes);

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
		private static Object locker = new object();

		/// <summary>
		///start function for main matrix to change values in it
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="matrixHeight"></param>
		/// <param name="matrixWidth"></param>
		/// <param name="freeSpace"></param>
		/// <param name="boundary"></param>
		public void SetMatrix(ref char[][] tetrisGrid, int matrixHeight, int matrixWidth, char freeSpace, char boundary)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					tetrisGrid[i][j] = freeSpace;
					if (i == matrixWidth - 1)
					{
						tetrisGrid[i][j] = boundary;
					}
				}
			}
		}

		/// <summary>
		/// Printings the matrix.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="score">The score.</param>
		/// <param name="matrixWidth">Width of the matrix.</param>
		public static void PrintingMatrix(char[][] tetrisGrid, int score, int matrixWidth)
		{
			lock (locker)
			{
				Console.Clear();
				for (int i = 0; i < matrixWidth; i++)
				{
					Console.WriteLine(tetrisGrid[i]);
					if (i == 2)
					{
						Console.Write(tetrisGrid[i]);
						Console.WriteLine("<<<TopLimit");
					}
				}
				Console.WriteLine("Score: " + score);
			}
		}

		/// <summary>
		/// Printings the matrix.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		public static void PrintingMatrix(char[,] tetrisGrid)
		{
			Console.WriteLine(tetrisGrid.GetLength(0) + " " + tetrisGrid.GetLength(1));
			lock (locker)
			{
				Console.WriteLine();
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
	///Helps with matrix
	/// </summary>
	internal class Helper
	{
		private int matrixWidth { get; set; }
		private int matrixHeight { get; set; }

		public Helper(int _matrixWidth, int _matrixHeight)
		{
			matrixHeight = _matrixHeight;
			matrixWidth = _matrixWidth;
		}
		public Random random = new Random();

		public void MoveLeft(ref char[][] tetrisGrid, char shapes, char freeSpace)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 1; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes && tetrisGrid[i][j - 1] == freeSpace)
					{
						char tempMatrix = tetrisGrid[i][j];
						tetrisGrid[i][j] = tetrisGrid[i][j - 1];
						tetrisGrid[i][j - 1] = tempMatrix;
					}
				}
			}
		}

		public void MoveRight(ref char[][] tetrisGrid, char shapes, char freeSpace)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = matrixHeight - 1; j >= 0; j--)
				{
					if (tetrisGrid[i][j] == shapes && tetrisGrid[i][j + 1] == freeSpace)
					{
						char tempMatrix = tetrisGrid[i][j];
						tetrisGrid[i][j] = tetrisGrid[i][j + 1];
						tetrisGrid[i][j + 1] = tempMatrix;
					}
				}
			}
		}

		public void MoveDown(ref char[][] tetrisGrid, char shapes,char freeSpace,char boundary,char placedShapes,int countOfBlocks)
		{
			List<Point> listOfElements = new List<Point>();
			for (int i = matrixWidth - 1; i > 0; i--)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						if (tetrisGrid[i + 1][j] == freeSpace)
						{
							char tempMatrix = tetrisGrid[i][j];
							tetrisGrid[i][j] = tetrisGrid[i + 1][j];
							tetrisGrid[i + 1][j] = tempMatrix;
							listOfElements.Add(new Point(i, j));
						}
						else if (tetrisGrid[i + 1][j] == boundary)
						{
							Convert3To4(ref tetrisGrid, shapes, placedShapes);
							countOfBlocks = 0;
							for (int z = 0; z < listOfElements.Count; z++)
							{
								char tempMatrix = tetrisGrid[listOfElements[z].X][listOfElements[z].Y];
								tetrisGrid[listOfElements[z].X][listOfElements[z].Y] =
									tetrisGrid[listOfElements[z].X + 1][listOfElements[z].Y];
								tetrisGrid[listOfElements[z].X + 1][listOfElements[z].Y] = tempMatrix;
							}
						}
					}
				}
			}
		}

		public void RotateUp(ref char[][] tetrisGrid, char shapes, char freeSpace, char boundary, char placedShapes, int heightOfShapes, int widthOfShapes)
		{
			int indj = matrixHeight;
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes && j < indj)
					{
						indj = j;
					}
				}
			}
			char[,] borderOfShape = new char[heightOfShapes, widthOfShapes];
			for (int i = 0; i < matrixWidth - 3; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						j = indj;
						bool p = true;
						for (int w = i; w < i + heightOfShapes; w++)
							for (int q = j; q < j + widthOfShapes; q++)
								if (tetrisGrid[w][q] == placedShapes || tetrisGrid[w][q] == boundary)
									p = false;
						if (p)
						{
							for (int w = i; w < i + heightOfShapes; w++)
							{
								for (int q = j; q < j + widthOfShapes; q++)
								{
									borderOfShape[w - i, q - j] = tetrisGrid[w][q];
								}
							}

							borderOfShape = Rotate(borderOfShape, widthOfShapes);
							for (int w = i; w < i + heightOfShapes; w++)
							{
								for (int q = j; q < j + widthOfShapes; q++)
								{
									tetrisGrid[w][q] = borderOfShape[w - i, q - j];
								}
							}
							i = matrixWidth;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks the border.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="border">The border.</param>
		/// <param name="shapes">The shapes.</param>
		/// <param name="side">The side.</param>
		/// <param name="matrixHeight">Height of the matrix.</param>
		/// <param name="matrixWidth">Width of the matrix.</param>
		/// <returns></returns>
		public bool CheckBorder(char[][] tetrisGrid, char border,
			char shapes, Side side)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						switch (side)
						{
							case Side.left:
								if (j == 0)
								{
									return true;
								}
								if (tetrisGrid[i][j - 1] == border)
								{
									return true;
								}
								break;

							case Side.rigth:
								if (j == matrixHeight - 1)
								{
									return true;
								}
								if (tetrisGrid[i][j + 1] == border)
								{
									return true;
								}
								break;

							case Side.down:
								if (i == matrixWidth)
								{
									return true;
								}
								if (tetrisGrid[i][j] == border)
								{
									return true;
								}
								break;

							default:
								break;
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Rotates the specified matrix.
		/// </summary>
		/// <param name="matrix">The matrix.</param>
		/// <param name="side">The side.</param>
		/// <returns></returns>
		public char[,] Rotate(char[,] matrix, int side)
		{
			char[,] forRotation = new char[side, side];
			for (int i = 0; i < side; i++)
			{
				for (int j = 0; j < side; j++)
				{
					forRotation[j, i] = matrix[side - i - 1, j];
				}
			}
			return forRotation;
		}
		/// <summary>
		/// Deletes the line.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="line">The line.</param>
		/// <param name="matrixHeight">Height of the matrix.</param>
		public void DeleteLine(char[][] tetrisGrid, int line)
		{
			for (int i = line; i > 0; i--)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					tetrisGrid[i][j] = tetrisGrid[i - 1][j];
				}
			}
		}

		/// <summary>
		/// Convert 3 to 4.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="matrixHeight">Height of the matrix.</param>
		/// <param name="matrixWidth">Width of the matrix.</param>
		/// <param name="shapes">The shapes.</param>
		/// <param name="placedShapes">The placed shapes.</param>
		public void Convert3To4(ref char[][] tetrisGrid, char shapes, char placedShapes)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						tetrisGrid[i][j] = placedShapes;
					}
				}
			}
		}

		/// <summary>
		/// Sets the shape.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="currentShape">The current shape.</param>
		/// <param name="countOfBlocks">The count of blocks.</param>
		/// <param name="whereToSpawn">The where to spawn.</param>
		/// <param name="widthOfShapes">The width of shapes.</param>
		/// <param name="shapes">The shapes.</param>
		/// <param name="freeSpace">The free space.</param>
		public void SetShape(ref char[][] tetrisGrid, ref char[,] currentShape,
		int countOfBlocks, int whereToSpawn, int widthOfShapes, char shapes, char freeSpace)
		{
			switch (countOfBlocks)
			{
				case 0:
					currentShape = CreateShape(currentShape, shapes, freeSpace);
					for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
					{
						tetrisGrid[0][i] = currentShape[countOfBlocks, i - whereToSpawn];
					}
					break;

				case 1:
				case 2:
					for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
					{
						tetrisGrid[0][i] = currentShape[countOfBlocks, i - whereToSpawn];
					}
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// Creates the shape.
		/// </summary>
		/// <param name="currentShape">The current shape.</param>
		/// <param name="shapes">The shapes.</param>
		/// <param name="freeSpace">The free space.</param>
		/// <returns></returns>
		private char[,] CreateShape(char[,] currentShape, char shapes, char freeSpace)
		{
			switch (random.Next(7))
			{
				case 0:
					currentShape = new char[,] { { shapes, freeSpace, freeSpace }, { shapes, freeSpace, freeSpace }, { shapes, freeSpace, freeSpace } };
					break;

				case 1:
					currentShape = new char[,] { { shapes, shapes, freeSpace }, { shapes, shapes, freeSpace }, { shapes, shapes, freeSpace } };
					break;

				case 2:
					currentShape = new char[,] { { shapes, shapes, shapes }, { shapes, shapes, shapes }, { shapes, shapes, shapes } };
					break;

				case 3:
					currentShape = new char[,] { { shapes, shapes, shapes }, { freeSpace, freeSpace, shapes }, { freeSpace, freeSpace, shapes } };
					break;

				case 4:
					currentShape = new char[,] { { freeSpace, freeSpace, shapes }, { shapes, shapes, shapes }, { freeSpace, freeSpace, shapes } };
					break;

				case 5:
					currentShape = new char[,] { { shapes, freeSpace, shapes }, { shapes, shapes, shapes }, { shapes, freeSpace, shapes } };
					break;

				case 6:
					currentShape = new char[,] { { shapes, shapes, shapes }, { shapes, freeSpace, shapes }, { shapes, freeSpace, shapes } };
					break;

				default:
					break;
			}
			return currentShape;
		}
	}
}
