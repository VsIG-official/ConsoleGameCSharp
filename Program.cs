﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

/// <summary>
/// My console game on csharp, in which you can play tetris
/// </summary>
namespace Console_Game_CSharp
{
	class Program
	{
		private static int[,] tetrisGrid = new int[12, 16];
		private static System.Timers.Timer aTimer;

		const int InfoPanelWidth = 10;
		const int TetrisWidth = 12;
		const int TetrisHeight = 16;
		const int GameWidth = TetrisWidth +
		InfoPanelWidth + 3;
		//const int GameHeight = TetrisHeight + 2;
		int score = 0;
		private static int[,] currentShape;
		private static int[,] nextShape;
		static int countOfBlocks = 0;

		/// <summary>
		/// Main function, where all cool things happen
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Console.Title = "Tetris";
			Console.WindowWidth = GameWidth;

			Tetris tetris = new Tetris();

			tetris.MakingMatrix(tetrisGrid);

			SetTimer();

			new Thread(NewThread).Start();
		}

		/// <summary>
		/// infinite stream for working algorithm (moving blocks)
		/// </summary>
		private static void NewThread()
		{
			while(true)
			{

			}
		}

		/// <summary>
		/// setting timer
		/// </summary>
		public static void SetTimer()
		{
			aTimer = new System.Timers.Timer(1000);
			aTimer.Elapsed += UpDate;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		/// <summary>
		///function, which is called once per second to find and move blocks
		/// </summary>
		/// <param name="sourse"></param>
		/// <param name="e"></param>
		private static void UpDate(Object sourse, ElapsedEventArgs e)
		{
			//to make entire figure stop and transform to 4,you can copy entire array to tempArray and if
			//case 2 or 4 is true than revert to tempArray, make it current and transform all 3 to 4
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
								break;
							case 4:
								tetrisGrid[i, j] = 4;
								break;
							default:
								break;
						}
					}
				}
			}

			Console.Clear();
			Tetris.GenerateShape(tetrisGrid, currentShape, nextShape, countOfBlocks);

			Tetris.PrintingMatrix(tetrisGrid);
			Console.WriteLine("nccn");
			countOfBlocks++;
			Console.WriteLine(countOfBlocks);
		}
	}

	/// <summary>
	///for tetris logic
	/// </summary>
	class Tetris
	{
		public const char Border = (char)178;
		public static Random random = new Random();

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

		/// <summary>
		/// Setting block and generating it in matrix
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="currentShape"></param>
		/// <param name="nextShape"></param>
		public static void GenerateShape(int[,] tetrisGrid, int[,] currentShape, int[,] nextShape,
			int countOfBlocks)
		{
			//if (countOfBlocks==0)
			//{
				switch (random.Next(3))
				{
					case 0: currentShape = new int[,] { { 3, 3, 1 }, { 3, 3, 1 } }; break;
					case 1: currentShape = new int[,] { { 3, 3, 3 }, { 3, 3, 3 } }; break;
					case 2: currentShape = new int[,] { { 1, 3, 1 }, { 3, 3, 3 } }; break;
						//case 3: nextShape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
						//case 4: nextShape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
						//case 5: nextShape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
						//case 6: nextShape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
				}
			//}

			if (countOfBlocks<=1)
			{
				Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 6, 3);
			}
		}
	}
}