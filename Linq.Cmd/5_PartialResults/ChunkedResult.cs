namespace Linq.Cmd._5_PartialResults;

public class ChunkedResult : QueryRunner
{
    public override void Run()
    {
        //SimpleChunks();
        ChunksWithNumbers();
    }

    /// <summary>
    /// Split the source query into equally sized chunks
    /// </summary>
    void SimpleChunks()
    {
        var sourceMovies = Repository.GetAllMovies();

        var query = 
            from movie in sourceMovies
            where movie.Producers.Count > 1
            select movie;
    }
    
    void ChunksWithNumbers()
    {
        var sourceMovies = Repository.GetAllMovies();

        var query = 
            from movie in sourceMovies
            where movie.Producers.Count > 1
            select movie;

        var chunks = query.Chunk(5);

        foreach (var chunk in chunks)
        {
            Console.WriteLine("CHUNK:");
            foreach (var movie in chunk)
            {
                Console.WriteLine(movie);
            }
            Console.WriteLine(string.Empty);
        }
    }
}