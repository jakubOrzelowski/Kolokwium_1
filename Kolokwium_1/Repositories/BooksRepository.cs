using Kolokwium_1.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium_1.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;

    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<BooksDto> getBooks(int id)
    {
        BooksDto booksDto = null;

        await using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"SELECT b.PK AS ID, b.title AS book_title, g.name AS genre_name
                                        FROM books b
                                        JOIN books_genres bg ON b.PK = bg.FK_book
                                        JOIN genres g ON bg.FK_genre = g.PK
                                        WHERE b.PK = @bookId";

            command.Parameters.AddWithValue("@bookId", id);

            await using (var reader = command.ExecuteReader())
            {
                if (await reader.ReadAsync())
                {
                    booksDto = new BooksDto
                    {
                        id = reader.GetInt32(reader.GetOrdinal("ID")),
                        title = reader.GetString(reader.GetOrdinal("book_title")),
                        genres = new List<GenresDto>()
                    };
                    do
                    {
                        booksDto.genres.Add(new GenresDto()
                        {
                            nam = reader.GetString(reader.GetOrdinal("genre_name")),

                        });
                    } while (await reader.ReadAsync());
                }
            }
        }

        if (booksDto == null)
        {
            throw new Exception();
        }

        return booksDto;
    }

    public async Task addBook(NewBookDto newBookDto)
    {
        await using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            await connection.OpenAsync();
            SqlTransaction transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO books (title) VALUES (@title); SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("@title", newBookDto.title);

                int newBookId = Convert.ToInt32(await command.ExecuteScalarAsync());

                foreach (var genreId in newBookDto.genres)
                {
                    command.Parameters.Clear();
                    command.CommandText = @"INSERT INTO books_genres (FK_book, FK_genre) VALUES (@bookId, @genreId);";

                    command.Parameters.AddWithValue("@bookId", newBookId);
                    command.Parameters.AddWithValue("@genreId", genreId.PK);

                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }

                throw;
            }
        }
    }
}