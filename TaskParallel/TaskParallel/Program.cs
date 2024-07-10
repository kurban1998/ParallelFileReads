using System.Diagnostics;

namespace TaskParallel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ReadFiles("your path");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        private static async void ReadFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                Console.WriteLine("Нет файлов");
                return;
            }

            int index = 0;
            var tasks = new List<Task<int>>();
            foreach (string file in files)
            {
                var task = Task.Run(() =>
                {
                    int spaceCount = 0;
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            foreach (var symbol in line)
                            {
                                if (symbol == ' ')
                                {
                                    spaceCount++;
                                }
                            }
                        }
                    }

                    return spaceCount;
                });

                tasks.Add(task);
            }

            int spaceCountSum = 0;
            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                spaceCountSum += await task;
            }

            Console.WriteLine(spaceCountSum);
        }
    }
}
