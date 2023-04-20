using System;

namespace LAB_8_SEARCH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[10000];
            Random rnd = new Random();
            for (int i = 0; i< arr.Length; i++)
            {

                arr[i] = rnd.Next(0, 1000);

            }
            arr = Sort(arr);
            int search = 300;

            Console.WriteLine();

            var a = BinarySearch(arr, search);

            Console.WriteLine("pos " + a.pos);
            Console.WriteLine("time " + a.workTime);
            Console.WriteLine("count " + a.count);

            a = LinSearch(arr, search);

            Console.WriteLine("pos " + a.pos);
            Console.WriteLine("time " + a.workTime);
            Console.WriteLine("count " + a.count);

            a = InterpolationSearch(arr, search);

            Console.WriteLine("pos " + a.pos);
            Console.WriteLine("time " + a.workTime);
            Console.WriteLine("count " + a.count);


            Console.WriteLine("Task 2 ------------------ 2 ksaT");

            string[] strArr = new string[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {

                strArr[i] = rnd.Next(10000, 1000000).ToString();

            }
            Console.WriteLine("КМП--ПМК");
            KMPSearch("2546", strArr);
            Console.WriteLine("БМ--МБ");
            BMsearch("2546", strArr);
        }


        #region FirstTask
        static (int pos, long workTime, int count) BinarySearch(int[] arr, int target)
        {
            int left = 0;
            int right = arr.Length - 1;

            int count = 0;

            long startTime = DateTime.Now.Ticks;

            while (left <= right)
            {
                count++;
                int mid = left + (right - left) / 2;
                long a = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                if (arr[mid] == target)
                {
                    return (mid, a, count);
                }
                else if (arr[mid] < target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return (-1, DateTime.Now.Ticks - startTime, count); // элемент не найден
        }

        static (int pos, long workTime, int count) LinSearch(int[] arr, int target)
        {

            int count = 0;

            long startTime = DateTime.Now.Ticks;
            for (int i = 0; i < arr.Length; i++)
            {
                long a = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                count++;
                if (arr[i] == target)
                {
                    
                    return (i, a, count); // если нашли элемент, возвращаем его индекс в массиве
                    
                }
            }
            return (-1, DateTime.Now.Ticks - startTime, count); // если элемент не найден, возвращаем -1
        }

        static (int pos, long workTime, int count) InterpolationSearch(int[] arr, int value)
        {

            int count = 0;

            long startTime = DateTime.Now.Ticks;
            int low = 0;
            int high = arr.Length - 1;
            while (low <= high && value >= arr[low] && value <= arr[high])
            {
                count++;
                long a = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                int pos = low + ((value - arr[low]) * (high - low)) / (arr[high] - arr[low]);
                if (arr[pos] == value)
                    return (pos, a, count);
                if (arr[pos] < value)
                    low = pos + 1;
                else
                    high = pos - 1;
            }
            return (-1, DateTime.Now.Ticks - startTime, count); // если элемент не найден, возвращаем -1
        }


        static int[] Sort(int[] arr)
        {
            int[] sortedArr = new int[arr.Length];
            Array.Copy(arr, sortedArr, arr.Length); // копируем исходный массив, чтобы не изменять его

            for (int i = 0; i < sortedArr.Length - 1; i++)
            {
                for (int j = i + 1; j < sortedArr.Length; j++)
                {
                    if (sortedArr[j] < sortedArr[i])
                    {
                        // меняем местами элементы, если они стоят в неправильном порядке
                        int temp = sortedArr[i];
                        sortedArr[i] = sortedArr[j];
                        sortedArr[j] = temp;
                    }
                }
            }

            return sortedArr;
        }
        #endregion


        #region SecondTask
        static void KMPSearch(string pattern, string[] texts)
        {
            int count = 0;

            long startTime = DateTime.Now.Ticks;
            int m = pattern.Length;

            int[] lps = new int[m];
            ComputeLPSArray(pattern, m, lps);

            foreach (string text in texts)
            {
                int n = text.Length;
                int i = 0; // индекс для text[]
                int j = 0; // индекс для pattern[]
                while (i < n)
                {
                    count++;
                    long a = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                    if (pattern[j] == text[i])
                    {
                        j++;
                        i++;
                    }

                    if (j == m)
                    {
                        Console.WriteLine($"Time = {a} Count = {count}");
                        Console.WriteLine("Найден шаблон " + "по индексу " + (i - j) + " в тексте: " + text);
                        j = lps[j - 1];
                    }

                    else if (i < n && pattern[j] != text[i])
                    {
                        if (j != 0)
                            j = lps[j - 1];
                        else
                            i = i + 1;
                    }
                }
            }
        }

        static void ComputeLPSArray(string pattern, int m, int[] lps)
        {

            int len = 0;
            int i = 1;
            lps[0] = 0;

            while (i < m)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len])
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else // if (len == 0)
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }


        //БМБМБМБМБМБМБМБМБМБМБМБМБМ


        static int NO_OF_CHARS = 256;

        static void BMsearch(string pattern, string[] texts)
        {
            int count = 0;

            long startTime = DateTime.Now.Ticks;
            int m = pattern.Length;

            int[] badchar = new int[NO_OF_CHARS];
            badCharHeuristic(pattern, m, badchar);

            foreach (string text in texts)
            {
                int n = text.Length;
                int s = 0; // сдвиг текста
                while (s <= (n - m))
                {
                    count++;
                    long a = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                    int j = m - 1;

                    while (j >= 0 && pattern[j] == text[s + j])
                        j--;

                    if (j < 0)
                    {
                        Console.WriteLine($"Time = {a} Count = {count}");
                        Console.WriteLine("Найден шаблон " + "по индексу " + s + " в тексте: " + text);
                        s += (s + m < n) ? m - badchar[text[s + m]] : 1;
                    }

                    else
                    {
                        s += Math.Max(1, j - badchar[text[s + j]]);
                    }
                }
            }
        }

        static void badCharHeuristic(string str, int size, int[] badchar)
        {
            for (int i = 0; i < NO_OF_CHARS; i++)
                badchar[i] = -1;

            for (int i = 0; i < size; i++)
                badchar[(int)str[i]] = i;
        }
        #endregion
    }
}
