using System.Collections;
using System.Collections.Concurrent;

public class Program
{
    static ConcurrentQueue<string> kelimeler = new ConcurrentQueue<string>();

    public static void Main(string[] args)
    {

        KelimeEkle();
        ThreadDuzenleyici();
        UzunlukBul();
    }

    public static void UzunlukBul()
    {
        while (true)
        {
            string kelime = null;
            lock (kelimeler)
            {
                while (kelimeler.Count == 0)
                {
                    Monitor.Wait(kelimeler);
                }
                foreach (var s in kelimeler)
                {
                    Console.WriteLine($" {s} : {s.Length}");
                    string t;
                    kelimeler.TryDequeue(out t);
                }
            }
        }
    }

    public static void ThreadDuzenleyici()
    {
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < 5; i++)
        {
            Thread thread = new Thread(UzunlukBul);
            thread.Start();
            threads.Add(thread);
        }
    }

    public static void KelimeEkle()
    {
        string dosyaYolu = @"C:\Users\pc\Desktop\kelimedosyasi.txt";
        string[] satırlar = File.ReadAllLines(dosyaYolu);
        foreach (var s in satırlar)
        {
            kelimeler.Enqueue(s);
        }
    }
}