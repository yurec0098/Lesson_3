using System;
using System.Collections.Generic;
using System.Linq;

namespace Lesson_3
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Console.Clear();
				WriteLineCentr("Добро пожаловать!", 30);
				WriteLineCentr("Выберите чем займёмся:", 30);
				Console.WriteLine();
				Console.WriteLine("  1. Вывод диагоналей");
				Console.WriteLine("  2. Телефонный справочник");
				Console.WriteLine("  3. Вывод текста в обрятном порядке");
				Console.WriteLine("  4. Игра «Морской бой»");
				Console.WriteLine();
				Console.WriteLine("  0. Выход");

				string line = Console.ReadLine();
				Console.Clear();
				switch (line)
				{
					case "1":
						WriteDiagonals(); break;
					case "2":
						Phonebook(); break;
					case "3":
						ReverseText(); break;
					case "4":
						SeaBattle(); break;

					case "0":
						return;

					default:
						break;
				}

				Console.ReadLine();
			}
		}
		static void WriteLineCentr(string text, int max = 40)
		{
			int startPos = (max - text.Length) / 2;
			Console.SetCursorPosition(startPos, Console.CursorTop);
			Console.WriteLine($"{text}");
		}

		static void WriteDiagonals()
		{
			WriteLineCentr("Вывод диагоналей", 30);

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
					if (9 - i == j)
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
		static void Phonebook()
		{
			WriteLineCentr("Телефонный справочник", 30);
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
				Console.WriteLine("{0,-20} {1,12}", contacts[i, 0], contacts[i, 1]);
		}
		static void ReverseText()
		{
			WriteLineCentr("Вывод текста в обрятном порядке", 30);
			Console.WriteLine("Введите любой текст:");

			string line = Console.ReadLine();
			for (int i = line.Length - 1; i >= 0; i--)
				Console.Write(line[i]);
		}
		static void SeaBattle()
		{
			var log = new List<LogItem>();
			var random = new Random((int)DateTime.Now.Ticks);

			var sea = new Sea(random, 2, 2);
			var enemy_sea = new Sea(random, 30, 2);

			while (true)
			{
				Console.Clear();
				Console.WriteLine("{0,-15} {1,15}", "", "Игра «Морской бой»");

				WriteLog(log);

				sea.WriteSea();
				enemy_sea.WriteSeaEnemy();

				Console.WriteLine();
				Console.WriteLine();

				if (sea.IsAllShipsKill() || enemy_sea.IsAllShipsKill())
					break;

				UserShoot(enemy_sea, log);

				if (enemy_sea.IsAllShipsKill())
					continue;

				EnemyShootAI(random, sea, log);
			}

			if (enemy_sea.IsAllShipsKill())
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("{0,-15} {1,15}", "", $"Поздравляю! Вы победили сделав {log.Count / 2 + 1} ходов!");
			}
			if (sea.IsAllShipsKill())
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("{0,-15} {1,15}", "", $"Сожалею! Вас победили сделав {log.Count / 2} ходов!");
			}

			Console.ReadLine();
		}

		private static void UserShoot(Sea sea, List<LogItem> log)
		{
			var pos = Console.GetCursorPosition();
			var shoot_pos = ReadSeaArea("Ваш ход, укажите координаты:");
			var shoot_result = sea.Shoot(shoot_pos);
			while (shoot_result == ShootResult.Repeat)
			{
				Console.SetCursorPosition(pos.Left, pos.Top);

				Console.WriteLine(new string(' ', Console.WindowWidth));
				Console.WriteLine(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(pos.Left, pos.Top);

				shoot_pos = ReadSeaArea("Вы сюда уже стреляли. Укажите другие координаты:");
				shoot_result = sea.Shoot(shoot_pos);
			}
			log.Add(new LogItem(0, shoot_pos, shoot_result));
		}
		private static void EnemyShootAI(Random random, Sea sea, List<LogItem> log)
		{
			var enemy_shoot_pos = (SeaArea)(random.Next() % 100);
			if (log.LastOrDefault(x => x.user == 1 && x.result == ShootResult.Wound) is LogItem last_wond)
			{
				bool need_new_pos = false;
				if (log.LastOrDefault(x => x.user == 1 && x.result == ShootResult.Kill) is LogItem last_kill)
					if (log.IndexOf(last_wond) < log.IndexOf(last_kill))
						need_new_pos = true;

				if (!need_new_pos)
				{
					SeaArea left = last_wond.pos;
					SeaArea bottom = last_wond.pos;
					SeaArea right = last_wond.pos;
					SeaArea top = last_wond.pos;

					int pos_x = (int)last_wond.pos % 10;
					int pos_y = (int)last_wond.pos / 10;

					bool horizon = true;
					bool vertical = true;
					if (pos_x < 9)
					{
						if (sea.enemy_area[pos_x + 1, pos_y] == 'O')
							left = last_wond.pos + 1;
						else if (sea.enemy_area[pos_x + 1, pos_y] != '0')
						{
							if (pos_x < 8 && sea.enemy_area[pos_x + 2, pos_y] == 'O')
								left = last_wond.pos + 2;
							else if (pos_x < 7 && sea.enemy_area[pos_x + 3, pos_y] == 'O' && sea.enemy_area[pos_x + 2, pos_y] == Ship.Simbol)
								left = last_wond.pos + 3;
							if (pos_x > 0 && sea.enemy_area[pos_x - 1, pos_y] == 'O')
								right = last_wond.pos - 1;
							else if (pos_x > 1 && sea.enemy_area[pos_x - 2, pos_y] == 'O')
								right = last_wond.pos - 2;
							else if (pos_x > 2 && sea.enemy_area[pos_x - 3, pos_y] == 'O' && sea.enemy_area[pos_x - 2, pos_y] == Ship.Simbol)
								right = last_wond.pos - 3;
							vertical = false;
						}
					}
					if (pos_y < 9)
					{
						if (sea.enemy_area[pos_x, pos_y + 1] == 'O')
							bottom = last_wond.pos + 10;
						else if (sea.enemy_area[pos_x, pos_y + 1] != '0')
						{
							if (pos_y < 8 && sea.enemy_area[pos_x, pos_y + 2] == 'O')
								bottom = last_wond.pos + 20;
							else if (pos_y < 7 && sea.enemy_area[pos_x, pos_y + 3] == 'O' && sea.enemy_area[pos_x, pos_y + 2] == Ship.Simbol)
								bottom = last_wond.pos + 30;
							if (pos_y > 0 && sea.enemy_area[pos_x, pos_y - 1] == 'O')
								top = last_wond.pos - 10;
							else if (pos_y > 1 && sea.enemy_area[pos_x, pos_y - 2] == 'O')
								top = last_wond.pos - 20;
							else if (pos_y > 2 && sea.enemy_area[pos_x, pos_y - 3] == 'O' && sea.enemy_area[pos_x, pos_y - 2] == Ship.Simbol)
								top = last_wond.pos - 30;
							horizon = false;
						}
					}
					if (pos_x > 0)
					{
						if (sea.enemy_area[pos_x - 1, pos_y] == 'O')
							right = last_wond.pos - 1;
						else if (sea.enemy_area[pos_x - 1, pos_y] != '0')
						{
							if (pos_x < 9 && sea.enemy_area[pos_x + 1, pos_y] == 'O')
								left = last_wond.pos + 1;
							else if (pos_x < 8 && sea.enemy_area[pos_x + 2, pos_y] == 'O')
								left = last_wond.pos + 2;
							else if (pos_x < 7 && sea.enemy_area[pos_x + 3, pos_y] == 'O' && sea.enemy_area[pos_x + 2, pos_y] == Ship.Simbol)
								left = last_wond.pos + 3;
							if (pos_x > 1 && sea.enemy_area[pos_x - 2, pos_y] == 'O')
								right = last_wond.pos - 2;
							else if (pos_x > 2 && sea.enemy_area[pos_x - 3, pos_y] == 'O' && sea.enemy_area[pos_x - 2, pos_y] == Ship.Simbol)
								right = last_wond.pos - 3;
							vertical = false;
						}
					}
					if (pos_y > 0)
					{
						if (sea.enemy_area[pos_x, pos_y - 1] == 'O')
							top = last_wond.pos - 10;
						else if (sea.enemy_area[pos_x, pos_y - 1] != '0')
						{
							if (pos_y < 9 && sea.enemy_area[pos_x, pos_y + 1] == 'O')
								bottom = last_wond.pos + 10;
							else if (pos_y < 8 && sea.enemy_area[pos_x, pos_y + 2] == 'O')
								bottom = last_wond.pos + 20;
							else if (pos_y < 7 && sea.enemy_area[pos_x, pos_y + 3] == 'O' && sea.enemy_area[pos_x, pos_y + 2] == Ship.Simbol)
								bottom = last_wond.pos + 30;
							if (pos_y > 1 && sea.enemy_area[pos_x, pos_y - 2] == 'O')
								top = last_wond.pos - 20;
							else if (pos_y > 2 && sea.enemy_area[pos_x, pos_y - 3] == 'O' && sea.enemy_area[pos_x, pos_y - 2] == Ship.Simbol)
								top = last_wond.pos - 30;
							horizon = false;
						}
					}

					var shoot_list = new List<SeaArea>();
					if (left != last_wond.pos && horizon)
						shoot_list.Add(left);
					if (bottom != last_wond.pos && vertical)
						shoot_list.Add(bottom);
					if (right != last_wond.pos && horizon)
						shoot_list.Add(right);
					if (top != last_wond.pos && vertical)
						shoot_list.Add(top);

					if (shoot_list.Count > 0)
						enemy_shoot_pos = shoot_list[random.Next() % shoot_list.Count];
				}
			}

			var enemy_shoot_result = sea.Shoot(enemy_shoot_pos);
			while (enemy_shoot_result == ShootResult.Repeat)
			{
				enemy_shoot_pos = (SeaArea)(random.Next() % 100);
				enemy_shoot_result = sea.Shoot(enemy_shoot_pos);
			}

			log.Add(new LogItem(1, enemy_shoot_pos, enemy_shoot_result));
		}

		private static void WriteLog(List<LogItem> log)
		{
			int start_index = 0;
			if (log.Count > 12)
				start_index = log.Count - 12;
			int cursor_top = 1;
			for (int i = start_index; i < log.Count; i++)
			{
				var log_item = log[i];
				Console.SetCursorPosition(55, cursor_top);

				if (log_item.user == 0)
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.Write("{0, 12}", $"Вы: ");
				}
				if (log_item.user == 1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("{0, 12}", $"Противник: ");
				}

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("{0, -3}", $"{ log_item.pos.ToString().ToUpper()}");

				switch (log_item.result)
				{
					case ShootResult.Miss:
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write($" промах");
						break;

					case ShootResult.Wound:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($" ранил");
						break;

					case ShootResult.Kill:
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($" убил");
						break;
				}

				Console.ForegroundColor = ConsoleColor.White;
				cursor_top++;
			}
		}

		private static SeaArea ReadSeaArea(string text)
		{
			string[] enumNames = Enum.GetNames(typeof(SeaArea));
			SeaArea value = SeaArea.a1;
			var pos = Console.GetCursorPosition();

			Console.WriteLine(text);
			while (Console.ReadLine() is string line &&
				//	Проверка соответствия по имени, отбрасывая возможность ввода обычным числом
				!(enumNames.Any(x => x.Equals(line, StringComparison.OrdinalIgnoreCase)) && Enum.TryParse(line, true, out value)))
			{
				Console.SetCursorPosition(pos.Left, pos.Top);
				Console.WriteLine($"Повторми... {text}");

				Console.WriteLine(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(pos.Left, pos.Top + 1);
			}

			return value;
		}
	}

	class LogItem
	{
		public int user { get; set; }
		public SeaArea pos { get; set; }
		public ShootResult result { get; set; }

		public LogItem(int _user, SeaArea _pos, ShootResult _result)
		{
			user = _user;
			pos = _pos;
			result = _result;
		}
	}

	public enum SeaArea
	{
		a1,
		a2,
		a3,
		a4,
		a5,
		a6,
		a7,
		a8,
		a9,
		a10,
		b1,
		b2,
		b3,
		b4,
		b5,
		b6,
		b7,
		b8,
		b9,
		b10,
		c1,
		c2,
		c3,
		c4,
		c5,
		c6,
		c7,
		c8,
		c9,
		c10,
		d1,
		d2,
		d3,
		d4,
		d5,
		d6,
		d7,
		d8,
		d9,
		d10,
		e1,
		e2,
		e3,
		e4,
		e5,
		e6,
		e7,
		e8,
		e9,
		e10,
		f1,
		f2,
		f3,
		f4,
		f5,
		f6,
		f7,
		f8,
		f9,
		f10,
		g1,
		g2,
		g3,
		g4,
		g5,
		g6,
		g7,
		g8,
		g9,
		g10,
		h1,
		h2,
		h3,
		h4,
		h5,
		h6,
		h7,
		h8,
		h9,
		h10,
		i1,
		i2,
		i3,
		i4,
		i5,
		i6,
		i7,
		i8,
		i9,
		i10,
		j1,
		j2,
		j3,
		j4,
		j5,
		j6,
		j7,
		j8,
		j9,
		j10
	}//abcdefghij
	public enum Direction
	{
		Left,
		Bottom
	}
	public enum ShootResult
	{
		Miss,   //	Промах
		Wound,  //	Ранение
		Kill,   //	Убил
		Repeat  //	Повтор
	}

	public class Sea
	{
		public char[,] area { get; private set; } = new char[10, 10];
		public char[,] enemy_area { get; private set; } = new char[10, 10];
		public Ship[] Ships { get; init; } = new Ship[10];

		public int X { get; set; }
		public int Y { get; set; }

		public const char Simbol = 'O';

		public Sea(int x = 0, int y = 0)
		{
			X = x;
			Y = y;

			for (int i = 0; i < area.GetLength(0); i++)
				for (int j = 0; j < area.GetLength(1); j++)
					area[i, j] = Simbol;

			for (int i = 0; i < enemy_area.GetLength(0); i++)
				for (int j = 0; j < enemy_area.GetLength(1); j++)
					enemy_area[i, j] = Simbol;

			for (int i = 0; i < Ships.Length; i++)
			{
				if (i < 4)
					Ships[i] = new Ship(this, 1);
				else if (i < 7)
					Ships[i] = new Ship(this, 2);
				else if (i < 9)
					Ships[i] = new Ship(this, 3);
				else
					Ships[i] = new Ship(this, 4);
			}
		}
		public Sea(Random random, int x = 0, int y = 0)
		{
			X = x;
			Y = y;

			for (int i = 0; i < area.GetLength(0); i++)
				for (int j = 0; j < area.GetLength(1); j++)
					area[i, j] = Simbol;

			for (int i = 0; i < enemy_area.GetLength(0); i++)
				for (int j = 0; j < enemy_area.GetLength(1); j++)
					enemy_area[i, j] = Simbol;

			for (int i = Ships.Length - 1; i >= 0; i--)
			{
				if (i < 4)
					Ships[i] = new Ship(this, 1, random);
				else if (i < 7)
					Ships[i] = new Ship(this, 2, random);
				else if (i < 9)
					Ships[i] = new Ship(this, 3, random);
				else
					Ships[i] = new Ship(this, 4, random);
			}
		}

		public void WriteSea()
		{
			for (int i = 0; i < area.GetLength(0); i++)
			{
				for (int j = 0; j < area.GetLength(1); j++)
				{
					if (j == 0)
					{
						Console.SetCursorPosition(X + i * 2 + 2, Y + j);
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write($"{i + 1}");
					}
					if (i == 0)
					{
						Console.SetCursorPosition(X + i * 2, Y + j + 1);
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write($"{(char)(65 + j)}");
					}

					Console.SetCursorPosition(X + i * 2 + 2, Y + j + 1);
					switch (area[i, j])
					{
						case Ship.Simbol:
							if (enemy_area[i, j] == Ship.Simbol)
								Console.ForegroundColor = ConsoleColor.Red;
							else
								Console.ForegroundColor = ConsoleColor.Green;

							Console.Write($"{area[i, j]}");
							break;

						default:
							if (enemy_area[i, j] == '0')
								Console.ForegroundColor = ConsoleColor.Yellow;
							else
								Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.Write($"{area[i, j]}");
							break;
					}
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
		}
		public void WriteSeaEnemy()
		{
			for (int i = 0; i < enemy_area.GetLength(0); i++)
			{
				for (int j = 0; j < enemy_area.GetLength(1); j++)
				{
					if (j == 0)
					{
						Console.SetCursorPosition(X + i * 2 + 2, Y + j);
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write($"{i + 1}");
					}
					if (i == 0)
					{
						Console.SetCursorPosition(X + i * 2, Y + j + 1);
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write($"{(char)(65 + j)}");
					}

					Console.SetCursorPosition(X + i * 2 + 2, Y + j + 1);
					switch (enemy_area[i, j])
					{
						case Ship.Simbol:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.Write($"{enemy_area[i, j]}");
							break;

						case '0':
							Console.ForegroundColor = ConsoleColor.White;
							Console.Write($"{enemy_area[i, j]}");
							break;

						default:
							Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.Write($"{enemy_area[i, j]}");
							break;
					}
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		public ShootResult Shoot(SeaArea pos)
		{
			int x = (int)pos % 10;
			int y = (int)pos / 10;

			if (enemy_area[x, y] == '0' || enemy_area[x, y] == Ship.Simbol)
				return ShootResult.Repeat;  //	Повтор

			if (area[x, y] == Ship.Simbol)
			{
				var result = ShootResult.Wound;
				enemy_area[x, y] = Ship.Simbol;

				foreach (var ship in Ships)
					ship.IsKill(this, ref result);

				return result;
			}
			else
				enemy_area[x, y] = '0'; //	Мимо

			return ShootResult.Miss;
		}

		public bool IsAllShipsKill()
		{
			for (int i = 0; i < area.GetLength(0); i++)
				for (int j = 0; j < area.GetLength(1); j++)
					if (area[i, j] == Ship.Simbol && enemy_area[i, j] != Ship.Simbol)
						return false;

			return true;
		}
	}
	public class Ship
	{
		public int X { get; private set; } = -1;
		public int Y { get; private set; } = -1;

		public int Size { get; init; }
		public Direction Dir { get; init; }

		bool complite;
		bool killed;
		public const char Simbol = 'X';   //	□	▣	▪

		public Ship(Sea sea, int size)
		{
			Size = size;
		}
		public Ship(Sea sea, int size, Random random)
		{
			Size = size;

			while (!complite)
			{
				Dir = (Direction)(random.Next() % 2);
				int height = 9;
				int width = 9;

				if (Dir == Direction.Left)
					width = width + 1 - Size;
				if (Dir == Direction.Bottom)
					height = height + 1 - Size;

				int tmp_x = random.Next() % (width + 1);
				int tmp_y = random.Next() % (height + 1);

				if (CanSetShip(sea, tmp_x, tmp_y))
				{
					X = tmp_x;
					Y = tmp_y;

					if (Dir == Direction.Left)
						for (int i = 0; i < Size; i++)
							sea.area[X, Y + i] = Simbol;

					if (Dir == Direction.Bottom)
						for (int i = 0; i < Size; i++)
							sea.area[X + i, Y] = Simbol;

					complite = true;
				}
			}
		}

		private bool CanSetShip(Sea sea, int tmp_x, int tmp_y)
		{
			int height = 3;
			int width = 3;

			if (Dir == Direction.Left)
				width = 2 + Size;
			if (Dir == Direction.Bottom)
				height = 2 + Size;

			if ((Dir == Direction.Bottom && tmp_x + Size > 9) ||
				(Dir == Direction.Left && tmp_y + Size > 9))
				return false;

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int _x = i + tmp_x - 1;
					int _y = j + tmp_y - 1;
					if (_x < 0 || _y < 0)
						continue;
					if (_x > 9 || _y > 9)
						continue;

					if (sea.area[_x, _y] != Sea.Simbol)
						return false;
				}
			}

			return true;
		}

		public bool IsKill(Sea sea, ref ShootResult result)
		{
			if (!killed)
			{
				int hits_count = 0;
				for (int i = 0; i < Size; i++)
				{
					if (Dir == Direction.Left)
						if (sea.enemy_area[X, Y + i] == Ship.Simbol)
							hits_count++;

					if (Dir == Direction.Bottom)
						if (sea.enemy_area[X + i, Y] == Ship.Simbol)
							hits_count++;
				}
				if (hits_count == Size)
				{
					int height = 3;
					int width = 3;

					if (Dir == Direction.Left)
						width = 2 + Size;
					if (Dir == Direction.Bottom)
						height = 2 + Size;

					for (int i = 0; i < height; i++)
					{
						for (int j = 0; j < width; j++)
						{
							int _x = i + X - 1;
							int _y = j + Y - 1;
							if (_x < 0 || _y < 0)
								continue;
							if (_x > 9 || _y > 9)
								continue;

							if (sea.enemy_area[_x, _y] != Ship.Simbol)
								sea.enemy_area[_x, _y] = '0';
						}
					}
					killed = true;
					result = ShootResult.Kill;
				}
			}
			return killed;
		}
	}
}
