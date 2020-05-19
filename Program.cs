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
        static List<int> tempIndexes = new List<int>();

        /// <summary>
        /// Main function, where all cool things happen
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
		{
			int[,] tetrisGrid = new int[12, 16];//height and width
            //[10,14] is for grid and [2,2] is for border

            MakingMatrix(tetrisGrid);

            SetShape();
            currentShape = nextShape;
            Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 6, 3);//instead of 3 create variable,
            //that will hold length of block
            //System.Threading.Thread.Sleep(1000);
            //Console.WriteLine(tetrisGrid.Length);
            SearchingForBlocks(tetrisGrid);
            MoveBlockDown();
            PrintingMatrix(tetrisGrid);

            // Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 7, currentShape.Length/2);

            /*
            MakingMatrix(tetrisGrid);

            for (int i = 0; i < currentShape.GetLength(0); i++)
            {
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    Console.Write(currentShape[i, j]);
                }
                Console.WriteLine();
            }
            //printBoundary(tetrisGrid, 12, 16);
            */
        }


        public static void SearchingForBlocks(int[,] tetrisGrid)
        {
            tempIndexes.Clear();
            /*
            for (int i = 0; i < tetrisGrid.Length-1; i++)
            {
                for (int j = 0; j < tetrisGrid.Length-1; j++)
                {
                    while (tetrisGrid[i, j] == 3)
                    {
                        Console.WriteLine(tetrisGrid[i, j]);
                        break;
                    }
                }
            }
            */

            foreach (int i in tetrisGrid)
            {
                if (i == 3)//you can make some stopper(count elements in block and,if this
                    //function has found all of 'em-stop)
                {
                    tempIndexes.Add(tetrisGrid[i,i]);
                    Console.WriteLine("YOU ARE AMAZING");
                    Console.WriteLine(tetrisGrid[i, i]);
                    //int a = tempIndexes.IndexOf(3);//List.IndexOf(tempIndexes, 3);
                    //Console.WriteLine(a);
                }
            }
        }

        public static void MoveBlockDown()
        {
            foreach (int i in tempIndexes)
            {

            }
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

        public static void PrintingMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j]);// + " ");
                }
                Console.WriteLine();
            }
        }

        /*
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
        */

        public static void SetShape()
        {
            switch (random.Next(3))
            {
                case 0: nextShape = new int[,] { { 3,3,1 }, { 3,3,1 } }; break;
                case 1: nextShape = new int[,] { { 3,3,3 }, { 3, 3, 3 } }; break;
                case 2: nextShape = new int[,] { { 1, 3, 1 }, { 3, 3, 3 } }; break;
                    //case 3: nextShape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
                    //case 4: nextShape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
                    //case 5: nextShape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
                    //case 6: nextShape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
            }
            //currentShape =
        }
    }
}
