using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Timers;

/// <summary>
/// My console game, where You can play tetris
/// </summary>
namespace ConsoleGameCSharp
{
	/// <summary>
	/// Main class
	/// </summary>
	internal static class TetrisConsoleGameCSharp
	{
		#region Variables

		private const int matrixWidth = 12;
		private const int matrixHeight = 16;
		private const int whereToSpawn = 6;
		private const int heightOfShapes = 3;//use this if Your blocks don't have same width and height
		private const int widthOfShapes = 3;
		private const int bonusWidthOfTheScreen = 17;
		private const int bonusHeightOfTheScreen = -2;
		private const int topLimit = 2;
		private const char freeSpace = ' ';
		private const char boundary = '─';
		private const char shapes = '█';
		private const char placedShapes = '#';
		private static char[][] tetrisGrid = new char[matrixWidth][];
		private static System.Timers.Timer aTimer;
		private static Thread thread = new Thread(new ThreadStart(NewThread));

		private const int gameWidth = matrixWidth + bonusWidthOfTheScreen;
		private const int gameHeight = matrixHeight + bonusHeightOfTheScreen;
		private static Mover mover = new Mover(matrixWidth, matrixHeight);
		private static int score;
		private static char[,] currentShape;
		private static int countOfBlocks;

		#endregion Variables

		/// <summary>
		/// Main function, where all cool things happen
		/// </summary>
		private static void Main()
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				tetrisGrid[i] = new char[matrixHeight];
			}
			Console.CursorVisible = false;
			Console.Title = "Tetris";
			Console.WindowWidth = gameWidth;
			Console.WindowHeight = gameHeight;

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
								mover.Convert3To4(ref tetrisGrid, shapes, placedShapes);
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
					mover.DeleteLine(tetrisGrid, i);
					score++;
				}
			}

			bool gameOver = false;
			for (int i = 0; i < matrixHeight; i++)
			{
				if (tetrisGrid[topLimit][i] == placedShapes)
				{
					Console.Clear();
					Console.WriteLine("Game over! You lost!");
					Console.WriteLine("And Your score is " + score);
					aTimer.Stop();
					thread.Abort();
					gameOver = true;
				}
			}

			if (!gameOver)
			{
				mover.SetShape(ref tetrisGrid, ref currentShape, countOfBlocks,
					whereToSpawn, widthOfShapes, shapes, freeSpace);
				GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth, topLimit);
				countOfBlocks++;
			}
		}

		/// <summary>
		/// Movings the shapes away.
		/// </summary>
		/// <param name="button">The button.</param>
		private static void MovingShapesAway(ConsoleKey button)
		{
			if (countOfBlocks > heightOfShapes)
			{
				switch (button)
				{
					case ConsoleKey.LeftArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, shapes, Side.left))
						{
							mover.MoveLeft(ref tetrisGrid, shapes, freeSpace);
						}
						break;

					case ConsoleKey.RightArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, shapes, Side.rigth))
						{
							mover.MoveRight(ref tetrisGrid, shapes, freeSpace);
						}
						break;

					case ConsoleKey.DownArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, shapes, Side.down))
						{
							mover.MoveDown(ref tetrisGrid, shapes, freeSpace,
								boundary, placedShapes, ref countOfBlocks);
						}
						break;
					case ConsoleKey.UpArrow:
						{
							mover.RotateUp(ref tetrisGrid, shapes, boundary,
								placedShapes, heightOfShapes, widthOfShapes);
						}
						break;
					default:
						break;
				}
			}
			GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth, topLimit);
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
		/// <param name="topLimit">top limit of the matrix.</param>
		public static void PrintingMatrix(char[][] tetrisGrid, int score, int matrixWidth, int topLimit)
		{
			lock (locker)
			{
				Console.Clear();
				for (int i = 0; i < matrixWidth; i++)
				{
					if (i == topLimit)
					{
						Console.Write(tetrisGrid[i]);
						Console.WriteLine("<<<TopLimit");
					}
					else
					{
						Console.WriteLine(tetrisGrid[i]);
					}
				}
				Console.WriteLine("Score: " + score);
			}
		}

		/// <summary>
		/// Printing the matrix.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		public static void PrintingMatrix(char[,] tetrisGrid)
		{
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

	/// <summary>
	///Enum declaration (4 values)
	/// </summary>
	internal enum Side
	{
		left,
		rigth,
		down,
		up,
	}

	/// <summary>
	/// Helps with movement in matrix
	/// </summary>
	internal class Mover
	{
		public Random random = new Random();
		private int matrixWidth { get; set; }
		private int matrixHeight { get; set; }

		public Mover(int _matrixWidth, int _matrixHeight)
		{
			matrixHeight = _matrixHeight;
			matrixWidth = _matrixWidth;
		}

		/// <summary>
		/// Moving left
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="shapes"></param>
		/// <param name="freeSpace"></param>
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

		/// <summary>
		/// Moving right
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="shapes"></param>
		/// <param name="freeSpace"></param>
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

		/// <summary>
		/// Moving down
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="shapes"></param>
		/// <param name="freeSpace"></param>
		/// <param name="boundary"></param>
		/// <param name="placedShapes"></param>
		/// <param name="countOfBlocks"></param>
		public void MoveDown(ref char[][] tetrisGrid, char shapes, char freeSpace, char boundary, char placedShapes, ref int countOfBlocks)
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
							countOfBlocks = 0;
							Convert3To4(ref tetrisGrid, shapes, placedShapes);
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

		/// <summary>
		/// Rotating
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="shapes"></param>
		/// <param name="boundary"></param>
		/// <param name="placedShapes"></param>
		/// <param name="heightOfShapes"></param>
		/// <param name="widthOfShapes"></param>
		public void RotateUp(ref char[][] tetrisGrid, char shapes, char boundary, char placedShapes, int heightOfShapes, int widthOfShapes)
		{
			int indexJ = matrixHeight;
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes && j < indexJ)
					{
						indexJ = j;
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
						j = indexJ;
						bool canRotate = true;
						for (int x = i; x < i + heightOfShapes; x++)
						{
							for (int y = j; y < j + widthOfShapes; y++)
							{
								if (tetrisGrid[x][y] == placedShapes || tetrisGrid[x][y] == boundary)
								{
									canRotate = false;
								}
							}
						}

						if (canRotate)
						{
							for (int x = i; x < i + heightOfShapes; x++)
							{
								for (int y = j; y < j + widthOfShapes; y++)
								{
									borderOfShape[x - i, y - j] = tetrisGrid[x][y];
								}
							}

							borderOfShape = Rotate(borderOfShape, widthOfShapes);
							for (int x = i; x < i + heightOfShapes; x++)
							{
								for (int y = j; y < j + widthOfShapes; y++)
								{
									tetrisGrid[x][y] = borderOfShape[x - i, y - j];
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