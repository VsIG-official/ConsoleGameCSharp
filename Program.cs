using System;
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
		int score;
		private static int[,] currentShape;
		private static int[,] nextShape;
		static int countOfBlocks;
		static int movingRight;

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
				//var key = Console.ReadKey();
				//if (key.Key == ConsoleKey.LeftArrow)
				//{
				//	Console.WriteLine("Left");
				//}
				//else if (key.Key == ConsoleKey.RightArrow)
				//{
				//	Console.WriteLine("Right");
				//}

				ConsoleKeyInfo key = Console.ReadKey();
				if (key.Key == ConsoleKey.LeftArrow)
				{
					movingRight = 1;
				}
				else if (key.Key == ConsoleKey.RightArrow)
				{
					movingRight = 2;
				}

				//if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
				//{
				//	Console.WriteLine("Left");
				//}
				//else if (Console.ReadKey().Key == ConsoleKey.RightArrow)
				//{
				//	Console.WriteLine("Right");
				//}
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
			//Console.WriteLine(tetrisGrid.GetLength(0)/*hight*/ + "+" + tetrisGrid.GetLength(1)/*width*/);
			//to make entire figure stop and transform to 4,you can copy entire array to tempArray and if
			//case 2 or 4 is true than revert to tempArray, make it current and transform all 3 to 4

			switch (movingRight)
			{
				case 1://from right to left
					for (int i = tetrisGrid.GetLength(0) - 1; i >= 0; i--)
					{
						for (int j = 1; j < tetrisGrid.GetLength(1); j++)
						{
							if (tetrisGrid[i, j] == 3)
							{
								switch (tetrisGrid[i, j - 1])
								{
									case 1:
										int tl = tetrisGrid[i, j];
										tetrisGrid[i, j] = tetrisGrid[i, j - 1];
										tetrisGrid[i, j - 1] = tl;
										Console.WriteLine("Nothing 1 Left");
										break;
									case 2:
										Console.WriteLine("Nothing 2");
										break;
									case 4:
										Console.WriteLine("Nothing 4");
										break;
									default:
										Console.WriteLine("Or nothing there or just default");
										break;
								}
								Console.WriteLine("Left");
								break;
							}
						}
					}
					break;
				case 2:
					Console.WriteLine("RightCase");
					break;
				default:
					break;
			}

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
								countOfBlocks = 0;
								break;
							case 4:
								tetrisGrid[i, j] = 4;
								countOfBlocks = 0;
								break;
							default:
								break;
						}

						/*
						switch (movingRight)
						{
							case 1://move left
								switch (tetrisGrid[i, j-1])
								{
									case 1:
										int tl = tetrisGrid[i, j];
										tetrisGrid[i, j] = tetrisGrid[i, j - 1];
										tetrisGrid[i, j - 1] = tl;
										Console.WriteLine("Nothing 1 Left");
										break;
									case 2:
										Console.WriteLine("Nothing 2");
										break;
									case 4:
										Console.WriteLine("Nothing 4");
										break;
									default:
										Console.WriteLine("Or nothing there or just default");
										break;
								}
								Console.WriteLine("Left");
								break;

							case 2://move right
								switch (tetrisGrid[i, j + 1])
								{
									case 1:
										int tr = tetrisGrid[i, j];
										tetrisGrid[i, j] = tetrisGrid[i, j + 1];
										tetrisGrid[i, j + 1] = tr;
										Console.WriteLine("Nothing 1 right");
										break;
									case 2:
										Console.WriteLine("Nothing 2");
										break;
									case 4:
										Console.WriteLine("Nothing 4");
										break;
									default:
										break;
								}
								Console.WriteLine("Right");
								break;
							default:
								Console.WriteLine("Zero");
								break;
						}
						*/
					}
				}
			}

			Console.Clear();
			Tetris.GenerateShape(tetrisGrid, currentShape, nextShape, countOfBlocks);

			Tetris.PrintingMatrix(tetrisGrid);
			countOfBlocks++;
			movingRight = 0;
			Console.WriteLine(countOfBlocks);
		}
	}

	/// <summary>
	///for tetris logic
	/// </summary>
	class Tetris
	{
		//public const char Border = (char)178;
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
					//4 is for delivered block
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
			switch (random.Next(5))
			{
				case 0:
					currentShape = new int[,] { { 3, 1, 1 }, { 3, 1, 1 } };
					break;
				case 1:
					currentShape = new int[,] { { 3, 3, 1 }, { 3, 3, 1 } };
					break;
				case 2:
					currentShape = new int[,] { { 3, 3, 3 }, { 3, 3, 3 } };
					break;
				case 3:
					currentShape = new int[,] { { 3, 3, 3 }, { 1, 1, 3 } };
					break;
				case 4:
					currentShape = new int[,] { { 1, 1, 3 }, { 3, 3, 3 } };
					break;
				default:
					break;
			}

			switch (countOfBlocks)
			{
				case 0:
					Array.Copy(currentShape, 0, tetrisGrid, 6, 3);
					break;
				case 1:
					Array.Copy(currentShape, 3, tetrisGrid, 6, 3);
					break;
				default:
					break;
			}
		}

		public static void MovingBlocks(int movingRight)
		{
			switch (movingRight)
			{
				case 0:
					//do nothing
					break;
				case 1:
					//move left
					break;
				case 2:
					//move right
					break;
				default:
					break;
			}
		}
	}
}