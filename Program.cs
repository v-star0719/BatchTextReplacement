using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Replace
{
    class Program
    {
		private class ReplaceData
		{
			public string oldStr;
			public string newStr;
            public Regex regex;
        }

		private static List<ReplaceData> replaceDatas = new List<ReplaceData>();
		private static int fileCount = 0;
        private static string fileType;
        private static string separator = " ==>> ";
        private static bool replaceSource = false;

		public static void Main(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				Console.WriteLine(args[i]);
			}
			LoadConfig();

			Console.WriteLine();
			Console.WriteLine("按任意键开始替换....");
			Console.WriteLine();
			Console.ReadKey();

			Iterate(Environment.CurrentDirectory);

			Console.WriteLine();
			Console.WriteLine($"处理完成，共{fileCount}个文件");
			Console.WriteLine();
            Console.ReadKey();
        }

		private static void LoadConfig()
		{
			Console.WriteLine("==========================================");
			Console.WriteLine("配置格式：");
            Console.WriteLine("    目标文件，例如*.txt");
            Console.WriteLine("    是否替换原文件，Y/N");
			Console.WriteLine($"    查找的字符串1{separator}替换的字符串1");
			Console.WriteLine($"    查找的字符串2{separator}替换的字符串2");
			Console.WriteLine($"    查找的字符串3{separator}替换的字符串3");
			Console.WriteLine("");
			Console.WriteLine("==========================================");
			Console.WriteLine("");

			try
			{
				FileStream fileStream = new FileStream("config.setting", FileMode.Open);
				byte[] buffer = new byte[fileStream.Length];
				fileStream.Read(buffer, 0, buffer.Length);
				fileStream.Close();

				Console.WriteLine("配置为：");
				string str = System.Text.Encoding.UTF8.GetString(buffer);
                str = str.Replace("\r\n", "\n");
				string[] lines = str.Split('\n');
                fileType = lines[0];
                replaceSource = lines[1] == "Y";
				for (int i = 2; i < lines.Length; i++)
				{
					var start = lines[i].IndexOf(separator);
                    if (start > 0)
                    {
					    ReplaceData data = new ReplaceData();
                        data.oldStr = lines[i].Substring(0, start);
                        data.newStr = lines[i].Substring(start + separator.Length, lines[i].Length - start - separator.Length);
						data.regex = new Regex(data.oldStr);
					    replaceDatas.Add(data);
					    Console.WriteLine($"[{i}]  {data.oldStr} ----> {data.newStr}");
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("配置文件config.txt 加载失败！");
				Console.WriteLine(e.ToString());
			}
			Console.WriteLine("");
		}

		private static void Iterate(string folderPath)
		{
			DirectoryInfo folder = new DirectoryInfo(folderPath);

			//遍历文件夹
			foreach (DirectoryInfo next in folder.GetDirectories())
			{
				Console.WriteLine(next.FullName);
				Iterate(next.FullName);
			}

			//遍历文件
			foreach (FileInfo next in folder.GetFiles(fileType))
			{
                Console.Write("处理：" + next.FullName);
                ReplaceFile(next.FullName);
                Console.WriteLine("......ok");
			}
		}

		private static void ReplaceFile(string filePath)
		{
			fileCount++;
			try
			{
				string str = File.ReadAllText(filePath);
                foreach (var replaceData in replaceDatas)
                {
					//str = str.Replace(replaceDatas[i].oldStr, replaceDatas[i].newStr);
                    str = replaceData.regex.Replace(str, replaceData.newStr);
                }

                if (replaceSource)
                {
                    var fileName = System.IO.Path.GetFileName(filePath);
                    var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    var fileNameNew = fileName.Replace(fileNameWithoutExtension, fileNameWithoutExtension + "_r");
                    filePath = filePath.Replace(fileName, fileNameNew);
                }
				File.WriteAllText(filePath, str);
			}
			catch (Exception e)
			{
				Console.WriteLine("文件" + filePath + "替换失败！");
				Console.WriteLine(e.ToString());
				Console.WriteLine("");
			}
		}
	}
}
