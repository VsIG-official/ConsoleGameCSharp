using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// My console game, where You can play tetris
/// </summary>
namespace ConsoleGameOnCSharp
{
	/// <summary>
	///for tetris logic
	/// </summary>
	class GameTetris
	{
		private static Object locker = new object();

		private int matrixWidth { get; }
		private int matrixHeight { get; }
		private char shapes { get; }
		private char freeSpace { get; }
		private char boundary { get; }
		private char placedShapes { get; }

		public GameTetris(int _matrixWidth, int _matrixHeight, char _shapes,
			char _freeSpace, char _boundary, char _placedShapes)
		{
			matrixHeight = _matrixHeight;
			matrixWidth = _matrixWidth;
			shapes = _shapes;
			freeSpace = _freeSpace;
			boundary = _boundary;
			placedShapes = _placedShapes;
		}

		/// <summary>
		/// Start function for main matrix to change values in it
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="matrixHeight"></param>
		/// <param name="matrixWidth"></param>
		public void SetMatrix(ref char[][] tetrisGrid, int matrixHeight,
			int matrixWidth)
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
		/// Prints the matrix.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="score">The score.</param>
		/// <param name="matrixWidth">Width of the matrix.</param>
		/// <param name="topLimit">top limit of the matrix.</param>
		public static void PrintMatrix(char[][] tetrisGrid, int score,
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
		/// Converts 3 to 4.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		public void Convert3To4(ref char[][] tetrisGrid)
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
		/// <param name="countOfBlocks"></param>
		public void MoveDown(ref char[][] tetrisGrid, ref int countOfBlocks)
		{
			for (int i = matrixWidth - 1; i >= 0; i--)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						CheckGround(ref tetrisGrid, ref countOfBlocks, i, j);
					}
				}
			}
		}

		/// <summary>
		/// Checks the ground.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="countOfBlocks">The count of blocks.</param>
		/// <param name="i">The i.</param>
		/// <param name="j">The j.</param>
		public void CheckGround(ref char[][] tetrisGrid,
			 ref int countOfBlocks, int i, int j)
		{
			List<Point> listOfElements = new List<Point>();
			if (tetrisGrid[i + 1][j] == freeSpace)
			{
				char tempMatrix = tetrisGrid[i][j];
				tetrisGrid[i][j] = tetrisGrid[i + 1][j];
				tetrisGrid[i + 1][j] = tempMatrix;
				listOfElements.Add(new Point(i, j));
			}
			else if (tetrisGrid[i + 1][j] == boundary || tetrisGrid[i + 1][j] == placedShapes)
			{
				Convert3To4(ref tetrisGrid);
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
