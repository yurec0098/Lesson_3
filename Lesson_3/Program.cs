using System;

namespace Lesson_3
{
	class Program
	{
		/* Написать программу — телефонный справочник — создать двумерный массив 5*2,
		 * хранящий список телефонных контактов: первый элемент хранит имя контакта,
		 * второй — номер телефона/e-mail. */
		static void Main(string[] args)
		{
			Console.WriteLine("Телефонный справочник");
			string[,] contacts = new string[5, 2];

			Console.WriteLine("Давайте заполним телефонный справочник");
			for (int i = 0; i < contacts.GetLength(0); i++)
			{
				Console.WriteLine($"Введите имя контакта №{i + 1}");
				contacts[i, 0] = Console.ReadLine();
				Console.WriteLine($"Введите телефон/e-mail контакта №{i + 1}");
				contacts[i, 1] = Console.ReadLine();
			}

			Console.WriteLine();
			Console.WriteLine($"Телефонный справочник хранит {contacts.GetLength(0)} контактов");
			for (int i = 0; i < contacts.GetLength(0); i++)
				Console.WriteLine("{0,-20} {1,12}", contacts[i,0], contacts[i,1]);
		}
	}
}
