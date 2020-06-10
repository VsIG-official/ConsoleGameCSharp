using ConsoleGameCSharp.Enums;
using ConsoleGameOnCSharp;
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
	static class TetrisConsoleGameCSharp
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
		private static Mover mover = new Mover(matrixWidth, matrixHeight,
			shapes, whereToSpawn, widthOfShapes, freeSpace);
		private static int score;
		private static char[,] currentShape;
		private static int countOfBlocks;

		private static char[][,] shapesArray = new char[][,]
		{
			new char[,] { { shapes, freeSpace, freeSpace },
				{ shapes, freeSpace, freeSpace }, { shapes, freeSpace, freeSpace } },
			new char[,] { { shapes, shapes, freeSpace },
				{ shapes, shapes, freeSpace }, { shapes, shapes, freeSpace } },
			new char[,] { { shapes, shapes, shapes },
				{ freeSpace, freeSpace, shapes }, { freeSpace, freeSpace, shapes } },
			new char[,] { { shapes, shapes, shapes },
				{ freeSpace, freeSpace, shapes }, { freeSpace, freeSpace, shapes } },
			new char[,] { { freeSpace, freeSpace, shapes },
				{ shapes, shapes, shapes }, { freeSpace, freeSpace, shapes } },
			new char[,] { { shapes, freeSpace, shapes },
				{ shapes, shapes, shapes }, { shapes, freeSpace, shapes } },
			new char[,] { { shapes, shapes, shapes },
				{ shapes, freeSpace, shapes }, { shapes, freeSpace, shapes } },
		};

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
			List<Point> listOfElements = new List<Point>();
			for (int i = matrixWidth - 1; i >= 0; i--)
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
						else if (tetrisGrid[i + 1][j] == boundary || tetrisGrid[i + 1][j] == placedShapes)
						{
							mover.Convert3To4(ref tetrisGrid, placedShapes);
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
			Endgame();
		}

		private static void Endgame()
		{
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
					shapesArray);
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
			//TODO на іфи
			if (countOfBlocks > heightOfShapes)
			{
				switch (button)
				{
					case ConsoleKey.LeftArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, Side.left))
							mover.MoveLeft(ref tetrisGrid);
						break;

					case ConsoleKey.RightArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, Side.rigth))
							mover.MoveRight(ref tetrisGrid);
						break;

					case ConsoleKey.DownArrow:
						if (!mover.CheckBorder(tetrisGrid, placedShapes, Side.down))
							mover.MoveDown(ref tetrisGrid,
								boundary, placedShapes, ref countOfBlocks);
						break;
					case ConsoleKey.UpArrow:
						mover.UpButton(ref tetrisGrid, boundary,
							placedShapes, heightOfShapes);
						break;

					default: break;
				}
			}
			GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth, topLimit);
		}
	}
}
