using System;
using System.IO;

namespace Snapper
{
	class Matrix
	{
		private bool[,] items;
		private string name;

		public Matrix(string fileName)
		{
			if (File.Exists(fileName))
			{
				name = Path.GetFileNameWithoutExtension(fileName);
				string[] lines = File.ReadAllLines(fileName);
				items = new bool[lines.Length, lines[0].ToCharArray().Length];
				for (int i = 0; i < lines.Length; i++)
				{
					var chars = lines[i].ToCharArray();
					for (int j = 0; j < chars.Length; j++)
					{
						items[i, j] = chars[j] == '+';
					}
				}
			}
		}

		public bool[,] Items
		{
			get { return items; }
		}

		public string Name
		{
			get { return name; }
		}
	}
}
