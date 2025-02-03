using System.Xml;
using Linq.Data.Models;
using Microsoft.Data.SqlClient;

namespace Dometrain.Linq.Cmd._2_Introduction;

public class NonLinqCodeSamples
{
    public void Run()
    {





























        // Working with in-memory objects
        var objectsResult = new List<Movie>();

        var movieObjects = GetMovies();

        foreach (var movie in movieObjects)
        {
            if (movie.Name.Contains("Spider"))
            {
                objectsResult.Add(movie);
            }
        }




























        // Working with SQL databases
        var sqlResult = new List<Movie>();

        var connection = new SqlConnection(GetConnectionString());
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT Id, Name, ReleaseDate
                              FROM dbo.Movies
                              WHERE Name LIKE '%Spider%'
                              """;

        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            sqlResult.Add(new Movie
            {
                MovieId = (Guid)reader["Id"],
                Name = (string)reader["Name"],
                ReleaseDate = (DateOnly)reader["ReleaseDate"]
            });
        }

















        // Working with XML
        var xmlResult = new List<Movie>();

        var document = new XmlDocument();
        document.Load("movies.xml");

        var namespaceMgr = new XmlNamespaceManager(document.NameTable);
        namespaceMgr.AddNamespace("mcu", "urn:mcu-movie-schema");

        var movieNodes = document.DocumentElement!.SelectNodes(
            "descendant::mcu:movie[mcu:Name[contains(text(),'Spider')]]", namespaceMgr);
        foreach (XmlNode movieNode in movieNodes!)
        {
            xmlResult.Add(new Movie
            {
                MovieId = Guid.Parse(movieNode
                    .SelectSingleNode("descendant::mcu:movie-id", namespaceMgr)!.InnerText),
                Name = movieNode
                    .SelectSingleNode("descendant::mcu:movie-name", namespaceMgr)!.InnerText,
                ReleaseDate = DateOnly.Parse(movieNode
                    .SelectSingleNode("descendant::mcu:movie-release", namespaceMgr)!.InnerText)
            });
        }

















        // Working with LINQ
        var movies = GetMovies();

        var linqQueryResult =
                from movie in movies
                where movie.Name.Contains("Spider")
                select movie;

        var linqFluentResult = movies
            .Where(movie => movie.Name.Contains("Spider"))
            .Select(movie => movie);





















        // Just to make the code highlighting look OK, and the results are used
        Console.WriteLine(objectsResult.Count);
        Console.WriteLine(sqlResult.Count);
        Console.WriteLine(xmlResult.Count);
        Console.WriteLine(linqQueryResult.Count());
        Console.WriteLine(linqFluentResult.Count());
    }

    /// <summary>
    /// Just to make the code highlighting look OK.
    /// Should return actual movies
    /// </summary>
    /// <returns>An empty list of movies.</returns>
    List<Movie> GetMovies()
    {
        //
        return new List<Movie>();
    }

    /// <summary>
    /// Just to make the code highlighting look OK.
    /// Should return and actual connection string
    /// </summary>
    /// <returns>An empty string.</returns>
    string GetConnectionString()
    {
        return string.Empty;
    }
}