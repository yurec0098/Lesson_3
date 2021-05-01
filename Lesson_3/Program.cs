using System;

namespace Lesson_3
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Вывод пользовательского ввода в инвертированном виде");
			do
			{
				Console.WriteLine("Ожидание пользовательского ввода:");

				string line = Console.ReadLine();
				for (int i = line.Length - 1; i >= 0; i--)
					Console.Write(line[i]);
				Console.WriteLine();

				Console.WriteLine("Попробуем ещё? Тогда введите +");
			} while (Console.ReadLine() == "+");
		}
	}
}
