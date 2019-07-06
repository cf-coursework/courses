﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqToObjects
{
	class Program
	{
		static void Main(string[] args)
		{
			///////////////////////////////////////////
			// Count occurrences of a word in a string
			///////////////////////////////////////////
			
			Console.WriteLine("COUNT OCCURRENCES OF A WORD IN A STRING\n");

			string text = @"Historically, the world of data and the world of objects" + // @ sign lets you specify string w/o having to escape anything
			  @" have not been well integrated. Programmers work in C# or Visual Basic" +
			  @" and also in SQL or XQuery. On the one side are concepts such as classes," +
			  @" objects, fields, inheritance, and .NET Framework APIs. On the other side" +
			  @" are tables, columns, rows, nodes, and separate languages for dealing with" +
			  @" them. Data types often require translation between the two worlds; there are" +
			  @" different standard functions. Because the object world has no notion of query, a" +
			  @" query can only be represented as a string without compile-time type checking or" +
			  @" IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to" +
			  @" objects in memory is often tedious and error-prone.";

			string searchTerm = "data";

			// Convert string into array of words
			string[] source = text.Split(new char[] { ' ', '.', ',', ':', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

			// Create query, using ToLowerInvariant to match "data" and "Data"
			var matchQuery = from word in source
							 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
							 select word;

			int wordCount = matchQuery.Count();
			Console.WriteLine("{0} occurrence(s) of the search term \"{1}\" were found.", wordCount, searchTerm);

			///////////////////////////////////////////
			// Query for sentences containing specified sets of words
			///////////////////////////////////////////

			Console.WriteLine("\nQUERY FOR SENTENCES CONTAINING SPECIFIED SETS OF WORDS\n");

			string text1 = @"Historically, the world of data and the world of objects " +
		@"have not been well integrated. Programmers work in C# or Visual Basic " +
		@"and also in SQL or XQuery. On the one side are concepts such as classes, " +
		@"objects, fields, inheritance, and .NET Framework APIs. On the other side " +
		@"are tables, columns, rows, nodes, and separate languages for dealing with " +
		@"them. Data types often require translation between the two worlds; there are " +
		@"different standard functions. Because the object world has no notion of query, a " +
		@"query can only be represented as a string without compile-time type checking or " +
		@"IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to " +
		@"objects in memory is often tedious and error-prone.";

			// Split text block into sentences
			string[] sentences = text1.Split(new char[] { '.', '?', '!' });

			// Define search terms (or could populate dynamically at runtime)
			string[] searchTerms = { "Historically", "data", "integrated" };

			// Find sentences containing all terms in search terms array
			var sentenceQuery = from sentence in sentences
								let words = sentence.Split(new char[] { ' ', '.', ',', ':', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
								where words.Distinct().Intersect(searchTerms).Count() == searchTerms.Count() // Compares count of unique words matching search terms (case-sensitively) to count of search term array
								select sentence;

			foreach (string sent in sentenceQuery)
			{
				Console.WriteLine(sent);
			}

			///////////////////////////////////////////
			// Query for characters in a string
			///////////////////////////////////////////

			Console.WriteLine("\nQUERY FOR CHARACTERS IN A STRING\n");

			string aString = "ABCDE99F-J74-12-06A";
			Console.WriteLine("Original string: {0}", aString);

			// Select only the chars that are numbers
			IEnumerable<char> stringQuery = from ch in aString
											where Char.IsDigit(ch)
											select ch;

			// Execute query
			foreach (char c in stringQuery)
			{
				Console.Write("{0} ", c);
			}
			Console.WriteLine();

			int count = stringQuery.Count();
			Console.WriteLine("Count = {0}", count);

			// Select all chars before dash
			IEnumerable<char> charsBeforeDash = aString.TakeWhile(c => c != '-');

			// Execute second query
			foreach (char c in charsBeforeDash)
			{
				Console.Write(c);
			}
			Console.WriteLine();

			///////////////////////////////////////////
			// Combine LINQ with Regular Expressions
			///////////////////////////////////////////

			Console.WriteLine("COMBINE LINQ WITH REGEXES");

			string startFolder = @"C:\Program Files (x86)\Microsoft Visual Studio\2017";

			IEnumerable<System.IO.FileInfo> fileList = GetFiles(startFolder);

			// Create Regex to find all things Visual
			System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"Visual (Basic|C\+\+|C#|Studio)");

			var queryMatchingFiles = from file in fileList
									 where file.Extension == ".htm"
									 let fileText = System.IO.File.ReadAllText(file.FullName)
									 let matches = rgx.Matches(fileText)
									 where matches.Count > 0
									 select new
									 {
										 name = file.FullName,
										 matchedValues = from System.Text.RegularExpressions.MatchCollection match in matches
														 select match
									 };

			// Execute query
			Console.WriteLine("The term \"{0}\" was found in:", rgx.ToString());
			foreach (var hit in queryMatchingFiles)
			{
				// Trim path, then write file name
				string s = hit.name.Substring(startFolder.Length - 1);
				Console.WriteLine(s);

				// For current file, write out all the matching strings
				foreach (var matchItem in hit.matchedValues)
				{
					Console.WriteLine(" {0}", matchItem);
				}
			}
		}

		public static IEnumerable<System.IO.FileInfo> GetFiles(string path)
		{
			if (!System.IO.Directory.Exists(path))
			{
				throw new System.IO.DirectoryNotFoundException();
			}

			string[] fileNames = null;
			List<System.IO.FileInfo> files = new List<System.IO.FileInfo>();

			fileNames = System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.AllDirectories);
			foreach (string name in fileNames)
			{
				files.Add(new System.IO.FileInfo(name));
			}

			return files;
		}
	}
}