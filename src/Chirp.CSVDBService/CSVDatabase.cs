namespace CSVDatabase
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using System;
    using System.Globalization;
    using DatabaseInterface;
    
    public sealed class CSVDatabase<T> : IDatabaseRepository<T>
    {
        private static CSVDatabase<T>? instance = null; 
        private string filePath;

        private CSVDatabase(string filePath)
        {
            this.filePath = filePath;
        }

        public static CSVDatabase<T> Instance(string filePath)
        { 
            if (instance == null)
            {
                instance = new CSVDatabase<T>(filePath);
            }
            return instance;
        }

        public IEnumerable<T> Read(int? limit = null)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>().ToList();
            return records;
        }

        public void Store(T record)
        {
            using var writer = new StreamWriter(filePath, true);
            writer.WriteLine();
            
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = args => false
            };
            using var csv = new CsvWriter(writer, csvConfig);
            
            csv.WriteRecord(record);
        }
    }
}