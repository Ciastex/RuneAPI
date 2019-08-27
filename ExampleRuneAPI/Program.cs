using RuneAPI;
using RuneAPI.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleRuneAPI
{
    class AutoCompletionHandler : IAutoCompleteHandler
    {
        private string[] AutoCompletes { get; }
        public char[] Separators { get; set; } = new char[0];

        public AutoCompletionHandler(string[] autocompletes)
        {
            AutoCompletes = autocompletes;
        }

        public string[] GetSuggestions(string text, int index)
        {
            return AutoCompletes.Where(x => x.ToLower().StartsWith(text.ToLower())).ToArray();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ScrapeForDb();

            var db = File.ReadAllText("./items.json").ToLower();
            var ge = new GrandExchange(db);

            ReadLine.AutoCompletionHandler = new AutoCompletionHandler(ge.ItemDb.Keys.ToArray());

            while (true)
            {
                try
                {
                    var line = ReadLine.Read("#id> ");
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                        continue;

                    var cmdline = line.Split(' ');
                    if (int.TryParse(cmdline[0], out int id))
                    {
                        var item = ge.GetItemDetails(id);
                        if (item != null)
                            Console.WriteLine(item.ToString());
                    }
                    else if(cmdline[0] == "/graph")
                    {
                        if (cmdline.Length != 2)
                            continue;

                        if (!int.TryParse(cmdline[1], out int param))
                            continue;

                        var data = ge.GetGraphData(param);

                        if (data == null)
                            continue;

                        foreach(var kvp in data)
                        {
                            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                        }
                    }
                    else if(cmdline[0] == "/latest")
                    {
                        if (cmdline.Length != 2)
                            continue;

                        if (!int.TryParse(cmdline[1], out int param))
                            continue;

                        var latestPrice = ge.GetLatestPrice(param);
                        if (latestPrice < 0)
                            continue;

                        Console.WriteLine($"Current GE price for this item is {latestPrice}gp.");
                    }
                    else
                    {
                        var item = ge.GetItemDetails(line.ToLower());
                        if (item != null)
                            Console.WriteLine(item.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void ScrapeForDb()
        {
            Console.WriteLine("Scraping...");

            var ge = new GrandExchange();
            var dict = ge.GetAlphabeticalCatalogue();

            using (var sw = new StreamWriter("./items.json"))
            {
                sw.WriteLine("{");
                foreach (var kvp in dict)
                {
                    var pageCount = Math.Ceiling(kvp.Value / 12d);

                    for (var i = 1; i <= pageCount; i++)
                    {
                        var itemList = ge.GetCataloguePage(kvp.Key, i);

                        while (itemList == null)
                        {
                            itemList = ge.GetCataloguePage(kvp.Key, i);
                            Thread.Sleep(1000);
                        }

                        foreach (var item in itemList.Items)
                        {
                            Console.WriteLine($"{item.Name}: {item.Id}");
                            sw.WriteLine($"    {{\"{item.Name}\": {item.Id}}},");
                        }
                        Thread.Sleep(1000);
                    }
                }
                sw.WriteLine("}");
            }
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
