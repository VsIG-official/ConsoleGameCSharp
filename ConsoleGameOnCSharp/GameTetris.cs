using System;

namespace ConsoleGameOnCSharp
{

	/// <summary>
	///for tetris logic
	/// </summary>
	class GameTetris
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
	}

}
