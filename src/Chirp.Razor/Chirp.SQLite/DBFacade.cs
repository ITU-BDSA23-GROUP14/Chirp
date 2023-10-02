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
            sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");
        }
        else
        {
            sqlDBFilePath = chirpDbPath;
        }
    }

    public List<CheepViewModel> GetCheeps(int pageNum)
    {
        var sqlQuery = @"
            SELECT * 
            FROM message 
            ORDER by message.pub_date desc
            LIMIT 5
            OFFSET @pageNum * 5";

        return QueryCheeps(sqlQuery);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum)
    {
        var sqlQuery = @"
            SELECT m.text, u.username, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            WHERE u.username = @author
            ORDER BY m.pub_date DESC
            LIMIT 5
            OFFSET @pageNum * 5";

        return QueryCheeps(sqlQuery, new SqliteParameter("@author", author));
    }

    private List<CheepViewModel> QueryCheeps(string sqlQuery, SqliteParameter? parameter = null)
    {
        List<CheepViewModel> cheeps = new();

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            if (parameter != null)
            {
                command.Parameters.Add(parameter);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CheepViewModel cheep = new(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2)
                );

                cheeps.Add(cheep);
            }
        }

        return cheeps;
    }
}
