using ConsoleGameCSharp.Enums;
using System;

namespace ConsoleGameCSharp
{
	/// <summary>
	/// Helps with movement in matrix
	/// </summary>
	class Mover
	{
		public Random random = new Random();

		private static int matrixWidth { get; set; }
		private static int matrixHeight { get; set; }
		private static char shapes { get; set; }
		private static char freeSpace { get; set; }
		private int whereToSpawn { get; set; }
		private int widthOfShapes { get; set; }

		public Mover(int _matrixWidth, int _matrixHeight, char _shapes, int _whereToSpawn,
			int _widthOfShapes, char _freeSpace)
		{
			matrixHeight = _matrixHeight;
			matrixWidth = _matrixWidth;
			shapes = _shapes;
			whereToSpawn = _whereToSpawn;
			widthOfShapes = _widthOfShapes;
			freeSpace = _freeSpace;
		}

		/// <summary>
		/// Moving left
		/// </summary>
		/// <param name="tetrisGrid"></param>
		public void MoveLeft(ref char[][] tetrisGrid)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 1; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes && tetrisGrid[i][j - 1] == freeSpace)
					{
						char tempMatrix = tetrisGrid[i][j];
						tetrisGrid[i][j] = tetrisGrid[i][j - 1];
						tetrisGrid[i][j - 1] = tempMatrix;
					}
				}
			}
		}

		/// <summary>
		/// Moving right
		/// </summary>
		/// <param name="tetrisGrid"></param>
		public void MoveRight(ref char[][] tetrisGrid)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = matrixHeight - 1; j >= 0; j--)
				{
					if (tetrisGrid[i][j] == shapes && tetrisGrid[i][j + 1] == freeSpace)
					{
						char tempMatrix = tetrisGrid[i][j];
						tetrisGrid[i][j] = tetrisGrid[i][j + 1];
						tetrisGrid[i][j + 1] = tempMatrix;
					}
				}
			}
		}

		/// <summary>
		/// Actions on Up button
		/// </summary>
		/// <param name="tetrisGrid"></param>
		/// <param name="boundary"></param>
		/// <param name="placedShapes"></param>
		/// <param name="heightOfShapes"></param>
		public void UpButton(ref char[][] tetrisGrid, char boundary,
			char placedShapes, int heightOfShapes)
		{
			int indexJ = matrixHeight;
			int indexI = matrixWidth;

			ChangingIndexes(ref indexJ, ref indexI, ref tetrisGrid);

			char[,] borderOfShape = new char[heightOfShapes, widthOfShapes];

			bool canRotate = true;
			for (int i = indexI; i < indexI + heightOfShapes; i++)
			{
				for (int j = indexJ; j < indexJ + widthOfShapes; j++)
				{
					if (tetrisGrid[i][j] == placedShapes || tetrisGrid[i][j] == boundary)
					{
						canRotate = false;
					}
				}
			}

			if (canRotate)
			{
				for (int i = indexI; i < indexI + heightOfShapes; i++)
				{
					for (int j = indexJ; j < indexJ + widthOfShapes; j++)
					{
						borderOfShape[i - indexI, j - indexJ] = tetrisGrid[i][j];
					}
				}

				borderOfShape = Rotation(borderOfShape, widthOfShapes);
				for (int i = indexI; i < indexI + heightOfShapes; i++)
				{
					for (int j = indexJ; j < indexJ + widthOfShapes; j++)
					{
						tetrisGrid[i][j] = borderOfShape[i - indexI, j - indexJ];
					}
				}
			}
		}

		/// <summary>
		/// Helps "UpButton" with changing indexes
		/// </summary>
		/// <param name="indexJ"></param>
		/// <param name="indexI"></param>
		/// <param name="tetrisGrid"></param>
		public void ChangingIndexes(ref int indexJ, ref int indexI, ref char[][] tetrisGrid)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes && j < indexJ)
					{
						indexJ = j;
					}
				}
			}

			for (int i = 0; i < matrixWidth - 3; i++)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					if (tetrisGrid[i][j] == shapes)
					{
						indexI = i;
						i = matrixWidth;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Checks the border.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="border">The border.</param>
		/// <param name="side">The side.</param>
		public bool CheckBorder(char[][] tetrisGrid, char border, Side side)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
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

								if (tetrisGrid[i][j - 1] == border)
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

							default: break;
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Rotates the specified matrix.
		/// </summary>
		/// <param name="matrix">The matrix.</param>
		/// <param name="side">The side.</param>
		public char[,] Rotation(char[,] matrix, int side)
		{
			char[,] forRotation = new char[side, side];
			for (int i = 0; i < side; i++)
			{
				for (int j = 0; j < side; j++)
				{
					forRotation[j, i] = matrix[side - i - 1, j];
				}
			}
			return forRotation;
		}

		/// <summary>
		/// Deletes the line.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="line">The line.</param>
		public void DeleteLine(char[][] tetrisGrid, int line)
		{
			for (int i = line; i > 0; i--)
			{
				for (int j = 0; j < matrixHeight; j++)
				{
					tetrisGrid[i][j] = tetrisGrid[i - 1][j];
				}
			}
		}




		/// <summary>
		/// Sets the shape.
		/// </summary>
		/// <param name="tetrisGrid">The tetris grid.</param>
		/// <param name="currentShape">The current shape.</param>
		/// <param name="countOfBlocks">The count of blocks.</param>
		public void SetShape(ref char[][] tetrisGrid, ref char[,] currentShape,
		int countOfBlocks, char[][,] shapesArray)
		{
			if (countOfBlocks == 0)
			{
				currentShape = shapesArray[random.Next(7)];
				for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
				{
					tetrisGrid[0][i] = currentShape[countOfBlocks, i - whereToSpawn];
				}
			}
			else if (countOfBlocks == 1 || countOfBlocks == 2)
			{
				for (int i = whereToSpawn; i < whereToSpawn + widthOfShapes; i++)
				{
					tetrisGrid[0][i] = currentShape[countOfBlocks, i - whereToSpawn];
				}
			}
		}
	}
}
