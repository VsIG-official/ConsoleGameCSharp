﻿using System;
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
		private const int whereToSpawn = 6;
		private const int widthOfShapes = 3;
		private const int bonusWidthOfTheScreen = 6;
		private const char freeSpace = '1';
		private const char boundary = '2';
		private const char shapes = '3';
		private const char placedShapes = '4';
		private static char[][] tetrisGrid = new char[matrixWidth][];
		private static System.Timers.Timer aTimer;

		private const int gameWidth = matrixWidth + bonusWidthOfTheScreen;
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
			Console.WindowWidth = gameWidth;
			Console.WindowHeight = matrixHeight;

			GameTetris tetris = new GameTetris();
			tetris.SetMatrix(ref tetrisGrid, matrixHeight, matrixWidth,freeSpace, boundary);
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
								helper.Convert3To4(ref tetrisGrid, matrixHeight, matrixWidth, shapes, placedShapes);
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
					helper.DeleteLine(tetrisGrid, i, matrixHeight);
					score++;
				}
			}
			helper.SetShape(ref tetrisGrid, ref currentShape, countOfBlocks, whereToSpawn, widthOfShapes, shapes,freeSpace);
			GameTetris.PrintingMatrix(tetrisGrid, score, matrixWidth);
			countOfBlocks++;
		}

		/// <summary>
		/// for moving shapes
		/// </summary>
		/// <param name="movingRight"></param>
		private static void MovingShapesAway(ConsoleKey button, int matrixHeight)
		{
			if (countOfBlocks > 2)
			{
				switch (button)
				{
					case ConsoleKey.LeftArrow:
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.left, matrixHeight, matrixWidth))
						{
							for (int i = 0; i < matrixWidth; i++)
							{
								for (int j = 1; j < matrixHeight; j++)
								{
									if (tetrisGrid[i][j] == shapes)
									{
										switch (tetrisGrid[i][j - 1])
										{
											case freeSpace:
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
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.rigth, matrixHeight, matrixWidth))
						{
							for (int i = 0; i < matrixWidth; i++)
							{
								//int len = matrixHeight;
								for (int j = matrixHeight - 1; j > 0; j--)
								{
									if (tetrisGrid[i][j] == shapes)
									{
										switch (tetrisGrid[i][j + 1])
										{
											case freeSpace:
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
						if (!helper.CheckBorder(tetrisGrid, placedShapes, shapes, Side.down, matrixHeight, matrixWidth))
						{
							for (int i = matrixWidth - 1; i > 0; i--)
							{
								//int len = matrixHeight;
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
		public void SetMatrix(ref char[][] tetrisGrid, int matrixHeight, int matrixWidth,char freeSpace,char boundary)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				//int len = tetrisGrid[i].Length;
				for (int j = 0; j < matrixHeight; j++)
				{
					tetrisGrid[i][j] = freeSpace;
					//1 is for empty space
					//2 is for bottom (if block will hit it-it stops)
					//3 is for blocks
					//4 is for delivered block
					if (i == 11)
					{
						tetrisGrid[i][j] = boundary;
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
		char shapes, Side side, int matrixHeight, int matrixWidth)
	{
		for (int i = 0; i < matrixWidth; i++)
		{
			//int len = tetrisGrid[i].Length;
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
							if (tetrisGrid[i][j - 1] == '2')
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

	public void DeleteLine(char[][] tetrisGrid, int line, int matrixHeight)
	{
		for (int i = line; i > 0; i--)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
			{
				tetrisGrid[i][j] = tetrisGrid[i - 1][j];
			}
		}
	}

	public void Convert3To4(ref char[][] tetrisGrid, int matrixHeight, int matrixWidth,char shapes,char placedShapes)
	{
		for (int i = 0; i < matrixWidth; i++)
		{
			//int len = tetrisGrid[i].Length;
			for (int j = 0; j < matrixHeight; j++)
				if (tetrisGrid[i][j] == '3')
				{
					tetrisGrid[i][j] = '4';
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
	int countOfBlocks,int whereToSpawn,int widthOfShapes,char shapes,char freeSpace)
	{
		switch (countOfBlocks)
		{
			case 0:
				currentShape = CreateShape(currentShape,shapes,freeSpace);
				for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
					tetrisGrid[0][i] = currentShape[0, i - 6];
				break;

			case 1:
				for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
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
	private char[,] CreateShape(char[,] currentShape,char shapes,char freeSpace)
	{
		switch (random.Next(7))
		{
			case 0:
				currentShape = new char[,] { { shapes, freeSpace, freeSpace }, { shapes, freeSpace, freeSpace } };
				break;

			case 1:
				currentShape = new char[,] { { shapes, shapes, freeSpace }, { shapes, shapes, freeSpace } };
				break;

			case 2:
				currentShape = new char[,] { { shapes, shapes, shapes }, { shapes, shapes, shapes } };
				break;

			case 3:
				currentShape = new char[,] { { shapes, shapes, shapes }, { freeSpace, freeSpace, shapes } };
				break;

			case 4:
				currentShape = new char[,] { { freeSpace, freeSpace, shapes }, { shapes, shapes, shapes } };
				break;

			case 5:
				currentShape = new char[,] { { shapes, freeSpace, shapes }, { shapes, shapes, shapes } };
				break;

			case 6:
				currentShape = new char[,] { { shapes, shapes, shapes }, { shapes, freeSpace, shapes } };
				break;

			default:
				break;
		}
		return currentShape;
	}
}