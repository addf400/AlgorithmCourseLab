using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.SetCover
{
    public abstract class SetCoverSolution : Solution<List<int[]>>
    {
        public List<int[]> sets;
        public int range;

        public SetCoverSolution(string setfile, int range)
        {
            this.range = range;

            sets = new List<int[]>();
            using (var reader = new StreamReader(setfile, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var nums = (from token in line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                                select int.Parse(token)).OrderBy(_ => _).ToArray();

                    sets.Add(nums);
                }
            }
        }
    }

    public class DataGen
    {
        public const int N = 20;

        public static Random rand = new Random(12345678);

        public static int[] GenSet(int[] set, int count)
        {
            if (set.Length < count)
                throw new InvalidDataException();

            var selected = new HashSet<int>();
            while (selected.Count < count)
            {
                int item = set[rand.Next(set.Length)];
                if (!selected.Contains(item))
                    selected.Add(item);
            }

            return selected.ToArray();
        }

        public static List<int[]> GenSets(int range, int count)
        {
            var used = new HashSet<int>();
            var notUsed = new HashSet<int>();
            for (int i = 0; i < range; ++i)
                notUsed.Add(i);

            var sets = new List<int[]>();

            while (notUsed.Count > N)
            {
                int pre = Math.Min(used.Count, rand.Next(N));
                var set = new List<int>();

                set.AddRange(GenSet(used.ToArray(), pre));

                int next = N - pre;
                foreach (var item in GenSet(notUsed.ToArray(), next))
                {
                    set.Add(item);
                    notUsed.Remove(item);
                    used.Add(item);
                }

                sets.Add(set.ToArray());
            }

            if (notUsed.Count > 0)
            {
                var set = new List<int>();

                set.AddRange(GenSet(used.ToArray(), N - notUsed.Count));
                foreach (var item in notUsed.ToArray())
                {
                    set.Add(item);
                    notUsed.Remove(item);
                    used.Add(item);
                }
            }

            while (sets.Count < count)
            {
                sets.Add(GenSet(used.ToArray(), rand.Next(N) + rand.Next(N)));
            }

            return sets;
        }

        public static void GenSetsToFile(int range, int count, string filename)
        {
            using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (var set in GenSets(range, count))
                {
                    writer.WriteLine(String.Join(" ", set));
                }
            }
        }

        public static IEnumerable<Tuple<string, int, int>> Run(string root, IEnumerable<Tuple<int, int>> parameters)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            foreach (var parameter in parameters)
            {
                string filename = Path.Combine(root, String.Format("{0}-sets.range-{1}.txt", parameter.Item2, parameter.Item1));
                if (!File.Exists(filename))
                    GenSetsToFile(parameter.Item1, parameter.Item2, filename);

                yield return new Tuple<string, int, int>(filename, parameter.Item1, parameter.Item2);
            }
        }
    }
}
