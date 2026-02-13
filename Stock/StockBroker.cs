using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Stock
{
    public class StockBroker
    {
        public string BrokerName { get; set; }
        public List<Stock> stocks = new List<Stock>();

        readonly string destPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lab1_Output.txt");

        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) +
            "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
            // Print the header to console
            Console.WriteLine(titles);

            // Overwrite (false) the file with this same header once
            using (StreamWriter outputFile = new StreamWriter(destPath, false))
            {
                outputFile.WriteLine(titles);
            }
        }

        public void AddStock(Stock stock)
        {
            stocks.Add(stock);
            stock.StockEvent += EventHandler;
        }

        private async void EventHandler(object sender, EventArgs e)
        {
            if (sender is not null)
                // The second parameter needs to be cast to StockNotification
                await Helper(sender, (StockNotification) e);
        }

        public async Task Helper(object sender, StockNotification e)
        {
            // We could cast the sender back to Stock if we needed more info
            Stock newStock = (Stock)sender;
            // Construct the output line
            string message =
            $"{BrokerName.PadRight(10)}" +
            $"{e.StockName.PadRight(15)}" +
            $"{e.CurrentValue.ToString().PadRight(10)}" +
            $"{e.NumChanges.ToString().PadRight(10)}" +
            $"{DateTime.Now}";
            try
            {
                // Append this line to the output file
                using (StreamWriter outputFile = new StreamWriter(destPath, true))
                {
                    await outputFile.WriteLineAsync(message);
                }
                // Also write to console
                Console.WriteLine(message);
            }
            catch (IOException ex)
            {
                // Handle or log any file I/O exceptions if needed
            }
        }

        /*
        public async Task write(Object sender, StockNotification e)
        {
            String line = BrokerName.PadRight(16) + e.StockName.PadRight(16) +
                Convert.ToString(e.CurrentValue).PadRight(16) +
                Convert.ToString(e.NumChanges).PadRight(16) + DateTime.Now;
            try
            {
                if (count == 0)
                {
                    Console.WriteLine(titles);
                    using (StreamWriter outputFile = new StreamWriter(destPath, false))
                    {
                        await outputFile.WriteLineAsync(titles);
                    }
                } //end if
                using (StreamWriter outputFile = new StreamWriter(destPath, true))
                {
                    await outputFile.WriteLineAsync(line);
                }
                Console.WriteLine(line);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
        */
    }
}