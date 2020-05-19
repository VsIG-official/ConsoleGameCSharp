using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;

/// <summary>
/// My console game on csharp, in which you can play tetris
/// </summary>
namespace Console_Game_CSharp
{
	class Program
	{
        const char Border = (char)178;
        static int[,] currentShape;
        static int[,] nextShape;
        static Random random = new Random();
        static List<int> tempIndexes = new List<int>();

        private static Timer aTimer;

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
            Array.Copy(currentShape, currentShape.GetLowerBound(0), tetrisGrid, 6, 3);

            /*
            // Create a timer with a 1.5 second interval.
            double interval = 1000.0;
            aTimer = new System.Timers.Timer(interval);

            // Hook up the event handler for the Elapsed event.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Only raise the event the first time Interval elapses.
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            */

            // Ensure the event fires before the exit message appears.
            //System.Threading.Thread.Sleep((int)interval * 4);
            //Console.WriteLine("Press the Enter key to exit the program.");
            //Console.ReadLine();

            SearchAndMoveBlocks(tetrisGrid);
            PrintingMatrix(tetrisGrid);
        }

        // Handle the Elapsed event.
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// will find and move blocks down(and,maybe,left and right)
        /// </summary>
        /// <param name="tetrisGrid"></param>
        public static void SearchAndMoveBlocks(int[,] tetrisGrid)
        {
            tempIndexes.Clear();
            //searching
            foreach (int i in tetrisGrid)
            {
                if (i == 3)//you can make some stopper(count elements in block and,if this
                //function has found all of 'em-stop)
                {
                    tempIndexes.Add(tetrisGrid[i,i]);
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
        }
    }
}