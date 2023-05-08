using System;
using System.Linq;

namespace Snapper
{
	class Program
	{
		/// <summary>
		/// Main method init 2 matrix object and compare them
		/// </summary>
		static void Main()
		{
			string path = String.Format("{0}", System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\")));
			var data = new Matrix(String.Format("{0}input\\{1}", path, "TestData.blf"));
			Console.WriteLine("Select target");
			Console.WriteLine("1.NuclearTorpedo");
			Console.WriteLine("2.Starship");

			string name = Console.ReadKey().Key == ConsoleKey.D1 ? "NuclearTorpedo.blf" : "Starship.blf";
			Console.WriteLine(String.Format("Search for {0}", name));

			Console.WriteLine("Enter matching percentage value must be between 0 and 1 in 0.00 format");
			double threshold;
			string line = Console.ReadLine();
			if (!System.Double.TryParse(line, out threshold) || (threshold > 1))
			{
				threshold = .75;
				Console.WriteLine(String.Format("Wrong input {0} using default threshold {1}", line, threshold));
			}
			else
			{
				Console.WriteLine(String.Format("Using threshold {0}", threshold));
			}

			Console.WriteLine("Enter tolerance Value must be between 0 and 1 in 0.00 format");
			double tolerance;
			line = Console.ReadLine();
			if (!System.Double.TryParse(line, out tolerance) || (tolerance > 1))
			{
				tolerance = .25;
				Console.WriteLine(String.Format("Wrong input {0} using default tolerance {1}", line, tolerance));
			}
			else
			{
				Console.WriteLine(String.Format("Using tolerance {0}", tolerance));
			}
			
			var target = new Matrix(String.Format("{0}input\\{1}", path, name));

			var dataRowCount = data.Items.GetLength(0);
			var dataColCount = data.Items.GetLength(1);

			var targetRowCount = target.Items.GetLength(0);
			var targetColCount = target.Items.GetLength(1);

			var stepRows = dataRowCount - targetRowCount;
			var stepCols = dataColCount - targetColCount;

			bool isMatchFound = false;
			// Iterate over all submatrices of the larger matrix
			for (int i = 0; i <= stepRows; i++)
			{
				for (int j = 0; j <= stepCols; j++)
				{
					// Extract the submatrix from the larger matrix
					bool[,] subMatrix = new bool[targetRowCount, targetColCount];
					for (int k = 0; k < targetRowCount; k++)
					{
						for (int l = 0; l < targetColCount; l++)
						{
							subMatrix[k, l] = data.Items[i + k, j + l];
						}
					}

					// Compare the submatrix with the smaller matrix
					if (percentageMatch(subMatrix, target.Items, threshold))
					{
						Console.WriteLine("Match found for threshold ([{0}-{1}][{2}-{3}])", i, j, i + targetColCount, j + targetRowCount);
						var output=String.Format("0001.{4}[{0}-{1}][{2}-{3}].blf", i, j, i + targetColCount, j + targetRowCount, target.Name);
						writeFile(path, output, subMatrix);
						isMatchFound = true;
					}

					if (averageMatch(subMatrix, target.Items, tolerance))
					{
						Console.WriteLine("Match found tolerance ([{0}-{1}][{2}-{3}])", i, j, i + targetColCount, j + targetRowCount);
						var output=String.Format("0002.{4}[{0}-{1}][{2}-{3}].blf", i, j, i + targetColCount, j + targetRowCount, target.Name);
						writeFile(path, output, subMatrix);
						isMatchFound = true;
					}
				}
			}
			if (!isMatchFound)
			{
				Console.WriteLine("No match found");
			}
			Console.WriteLine("Completed");
			Console.ReadKey();
		}

		/// <summary>
		/// Check if at least threshold percent of element are match
		/// </summary>
		/// <param name="m1">binary matrix</param>
		/// <param name="m2">binary matrix</param>
		/// <param name="threshold">max alowed match percentage</param>
		/// <returns>true if matchingElements / totalElements >= threshold </returns>
		static bool percentageMatch(bool[,] m1, bool[,] m2, double threshold)
		{
			if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
			{
				return false;
			}

			int totalElements = m1.GetLength(0) * m1.GetLength(1);
			int matchingElements = 0;

			for (int i = 0; i < m1.GetLength(0); i++)
			{
				for (int j = 0; j < m1.GetLength(1); j++)
				{
					if (m1[i, j] == m2[i, j])
					{
						matchingElements++;
					}
				}
			}
			return (double)matchingElements / totalElements >= threshold;
		}


		/// <summary>
		/// Check if average percentage of matching elements across all rows are less then 1- tolerance
		/// </summary>
		/// <param name="m1">binary matrix</param>
		/// <param name="m2">binary matrix</param>
		/// <param name="tolerance">min alowed tolerance</param>
		/// <returns>true if avgMatchPercent >= (1 - tolerance) </returns>
		static bool averageMatch(bool[,] m1, bool[,] m2, double tolerance)
		{
			if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
			{
				return false;
			}

			var numRows = m1.GetLength(0);
			var numCols = m1.GetLength(1);

			// Compute percentage of matching elements in each row
			var rowMatchPercents = new double[numRows];
			for (int i = 0; i < numRows; i++)
			{
				var numMatching = 0;
				for (int j = 0; j < numCols; j++)
				{
					if (m1[i, j] == m2[i, j])
					{
						numMatching++;
					}
				}
				rowMatchPercents[i] = (double)numMatching / numCols;
			}

			// Compute average percentage of matching elements across all rows
			var avgMatchPercent = rowMatchPercents.Average();

			return (avgMatchPercent >= (1 - tolerance));
		}

		/// <summary>
		/// Write matched matrix to file for visual inspection
		/// </summary>
		/// <param name="path">path to output dir</param>
		/// <param name="filename">custom file name</param>
		/// <param name="m">matrix to process</param>
		static void writeFile(string path, string filename, bool[,] m)
		{
			using (var sw = new System.IO.StreamWriter(String.Format("{0}output\\{1}", path, filename)))
			{
				for (int i = 0; i < m.GetLength(0); i++)
				{
					for (int j = 0; j < m.GetLength(1); j++)
					{
						var ch = m[i, j] ? '+' : ' ';
						sw.Write(ch);
					}
					sw.Write("\n");
				}

				sw.Flush();
				sw.Close();
			}
		}
	}
}