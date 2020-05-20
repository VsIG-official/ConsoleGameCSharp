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
		private static int[,] tetrisGrid = new int[12, 16];//height and width
		private static System.Timers.Timer aTimer;

		/// <summary>
		/// Main function, where all cool things happen
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Tetris tetris = new Tetris();
			//[10,14] is for grid and [2,2] is for border

			tetris.MakingMatrix(tetrisGrid);

			tetris.SetShape();
			tetris.currentShape = tetris.nextShape;
			Array.Copy(tetris.currentShape, tetris.currentShape.GetLowerBound(0), tetrisGrid, 6, 3);

			SetTimer();

			new Thread(NewThread).Start();
			//tetris.SearchAndMoveBlocks(tetrisGrid);

		}

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

		private static void UpDate(Object sourse, ElapsedEventArgs e)
		{
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
			Tetris.PrintingMatrix(tetrisGrid);
		}
	}
	class Tetris
	{
		public const char Border = (char)178;
		public int[,] currentShape;
		public int[,] nextShape;
		public Random random = new Random();
		public List<int> tempIndexes = new List<int>();

		public Tetris()
		{

		}

		/// <summary>
		/// will find and move blocks down(and,maybe,left and right)
		/// </summary>
		/// <param name="tetrisGrid"></param>
		public void SearchAndMoveBlocks(int[,] tetrisGrid)
		{
			tempIndexes.Clear();
			//searching
			foreach (int i in tetrisGrid)
			{
				if (i == 3)//you can make some stopper(count elements in block and,if this
						   //function has found all of 'em-stop)
				{
					tempIndexes.Add(tetrisGrid[i, i]);
					Console.WriteLine("YOU ARE AMAZING");
					Console.WriteLine(tetrisGrid[i, i]);
				}
			}
			//moving
			foreach (int i in tempIndexes)
			{

			}
		}

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
		/// setting one of shapes to generate
		/// </summary>
		public void SetShape()
		{
			switch (random.Next(3))
			{
				case 0: nextShape = new int[,] { { 3, 3, 1 }, { 3, 3, 1 } }; break;
				case 1: nextShape = new int[,] { { 3, 3, 3 }, { 3, 3, 3 } }; break;
				case 2: nextShape = new int[,] { { 1, 3, 1 }, { 3, 3, 3 } }; break;
					//case 3: nextShape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
					//case 4: nextShape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
					//case 5: nextShape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
					//case 6: nextShape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
			}
		}
	}
}