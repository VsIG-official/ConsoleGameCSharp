using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleGameOnCSharp
{

	/// <summary>
	///for tetris logic
	/// </summary>
	class GameTetris
	{
		private static Object locker = new object();

		private int matrixWidth { get; set; }
		private int matrixHeight { get; set; }
		private char shapes { get; set; }
		private char freeSpace { get; set; }

		public GameTetris(int _matrixWidth, int _matrixHeight, char _shapes, char _freeSpace)
		{
			matrixHeight = _matrixHeight;
			matrixWidth = _matrixWidth;
			shapes = _shapes;
			freeSpace = _freeSpace;
		}

		/// <summary>
		///start function for main matrix to change values in it
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="matrixHeight"></param>
		/// <param name="matrixWidth"></param>
		/// <param name="freeSpace"></param>
		/// <param name="boundary"></param>
		public void SetMatrix(ref char[][] tetrisGrid, int matrixHeight,
			int matrixWidth, char freeSpace, char boundary)
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
		public static void PrintingMatrix(char[][] tetrisGrid, int score,
			int matrixWidth, int topLimit)
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
		/// Convert 3 to 4.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="shapes">The shapes.</param>
		/// <param name="placedShapes">The placed shapes.</param>
		public void Convert3To4(ref char[][] tetrisGrid, char placedShapes)
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
		/// Actions on Down button
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="boundary"></param>
		/// <param name="placedShapes"></param>
		/// <param name="countOfBlocks"></param>
		public void OnButtonDown(ref char[][] tetrisGrid,
			char boundary, char placedShapes, ref int countOfBlocks)
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
						else if (tetrisGrid[i + 1][j] == boundary || tetrisGrid[i + 1][j] == placedShapes)
						{
							Convert3To4(ref tetrisGrid, placedShapes);
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
	}
}
