namespace SimpleDB
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using System.Globalization;

    //The following is adapted from: https://joshclose.github.io/CsvHelper/getting-started/
    sealed public class CSVDatabase<T> : IDatabaseRepository<T>
    {
        private static CSVDatabase<T> instance = null; 
        private string filePath;

        public static CSVDatabase Instance(string filePath)
        {
            get
            {
                if (instance == null)
                {
                    instance = new CSVDatabase(filePath);
                }
                this.filePath = filePath;
            }
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