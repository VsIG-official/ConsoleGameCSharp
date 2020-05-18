using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// My console game on csharp, in which you can play tetris
/// </summary>
namespace Console_Game_CSharp
{
	class Program
	{
		//int gameInfo = 10;
		const char Border = (char)178;
        static int[,] currentShape;
        static int[,] nextShape;
        static Random random = new Random();

        /// <summary>
        /// Main function, where all cool things happen
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
		{
			int[,] tetrisGrid = new int[12, 16];//height and width
            //[10,14] is for grid and [2,2] is for border

            MakingMatrix(tetrisGrid);
            printBoundary(tetrisGrid, 12, 16);

            SetShape();
            currentShape = nextShape;
                Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 7, 2);
            System.Threading.Thread.Sleep(1000);
            MoveBlockDown();
            // var result = Filter(tetrisGrid, u => u[0] == 3);
            //Console.WriteLine(result);

            Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 7, 2);
            //var result = Filter(tetrisGrid, u => u[0] == 3);
            //Console.WriteLine(result);
            tetrisGrid.GetValue()
            //Console.WriteLine(tetrisGrid[11, 7]);
            MakingMatrix(tetrisGrid);

            for (int i = 0; i < currentShape.GetLength(0); i++)
            {
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    Console.Write(currentShape[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void MoveBlockDown()
        {

        }
        /// <summary>
        /// start function for main matrix to change values in it
        /// </summary>
        /// <param name="matrix"></param>
        public static void MakingMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i >= 1 && i <= matrix.GetLength(0) - 2)
                    {
                        if (j >= 1 && j <= matrix.GetLength(1) - 2)
                        {
                            matrix[i, j] = 1;
                            //0 is for walls (if block will hit it-nothing will happen)
                            //1 is for empty space
                            //2 is for bottom (if block will hit it-it stops)
                            //3 is for blocks
                        }
                    }
                    else if (i == 11)
                    {
                        matrix[i, j] = 2;
                    }
                    Console.Write(matrix[i, j]);// + " ");
                }
                Console.WriteLine();
            }
            //return matrix;
        }

        public static void printBoundary(int[,] a,int m,int n)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == 0)
                        Console.Write(a[i, j] + " ");
                    else if (i == m - 1)
                        Console.Write(a[i, j] + " ");
                    else if (j == 0)
                        Console.Write(a[i, j] + " ");
                    else if (j == n - 1)
                        Console.Write(a[i, j] + " ");
                    else
                        Console.Write("  ");
                }
                Console.WriteLine(" ");
            }
        }

        public static void SetShape()
        {
            switch (random.Next(2))
            {
                case 0: nextShape = new int[,] { { 3,3 }, { 3,3 } }; break;
                case 1: nextShape = new int[,] { { 3,3,3 }, { 3, 3, 3 } }; break;
                //case 2: nextShape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 9 } }; break;
                //case 3: nextShape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
                //case 4: nextShape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
                //case 5: nextShape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
                //case 6: nextShape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
            }
            //currentShape =
        }
    }
}
