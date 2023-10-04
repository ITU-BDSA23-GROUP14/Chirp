using System.Diagnostics;
using Microsoft.Data.Sqlite;
using ViewModel;

public class DBFacade
{
    private readonly string sqlDBFilePath;

    public DBFacade()
    {
        string? chirpDbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");       // Attempt to get the database path from the environment variable

        // If succesfullly located then use the path, otherwise create it
        if (string.IsNullOrEmpty(chirpDbPath))
        {
            //sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");
            sqlDBFilePath = "data/chirp.db";
        }
        else
        {
            sqlDBFilePath = chirpDbPath;
        }
    }

    public List<CheepViewModel> GetCheeps(int pageNum)
    {
        var sqlQuery = @"
            SELECT m.text, u.username, strftime('%m/%d/%Y %H:%M:%S', m.pub_date, 'unixepoch', 'localtime') as time 
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            ORDER by time desc
            LIMIT 32
            OFFSET @pageNum * 32";

        return QueryCheeps(sqlQuery, new List<SqliteParameter> { new SqliteParameter("@pageNum", pageNum) });
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum)
    {
        var sqlQuery = @"
            SELECT m.text, u.username, strftime('%m/%d/%Y %H:%M:%S', m.pub_date, 'unixepoch', 'localtime') as time
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            WHERE u.username = @author
            ORDER BY time DESC
            LIMIT 32
            OFFSET @pageNum * 32";

        return QueryCheeps(sqlQuery, new List<SqliteParameter> { new SqliteParameter("@author", author), new SqliteParameter("@pageNum", pageNum) });
    }

    private List<CheepViewModel> QueryCheeps(string sqlQuery, List<SqliteParameter> parameters)
    {
        List<CheepViewModel> cheeps = new();

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CheepViewModel cheep = new(
                    reader.GetString(1),
                    reader.GetString(0),
                    reader.GetString(2)
                );

                cheeps.Add(cheep);
            }
        }

        return cheeps;
    }
}
