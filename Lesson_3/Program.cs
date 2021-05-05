using System;

namespace Lesson_3
{
	class Program
	{
		static void Main(string[] args)
		{
			string[,] array = new string[10, 10];
			for (int i = 0; i < 10; i++)
				for (int j = 0; j < 10; j++)
					array[i, j] = $"{(char)(65 + i)}{j}";

			string[] diagonal = new string[10];
			string[] diagonal2 = new string[10];
			for (int i = 0; i < 10; i++)
				for (int j = 0; j < 10; j++)
				{
					if (i == j)
						diagonal[i] = array[i, j];
					if (9 -i == j)
						diagonal2[i] = array[i, j];
				}

			Console.WriteLine(string.Join(", ", diagonal));
			Console.WriteLine(string.Join(", ", diagonal2));
			Console.WriteLine();

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					if (i == j)
						Console.ForegroundColor = ConsoleColor.Green;
					if (9 - i == j)
						Console.ForegroundColor = ConsoleColor.Blue;
					Console.Write($"{array[i, j]} ");
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.WriteLine();
			}
		}
	}
}
