using Kolokwium_1.Models.DTOs;

namespace Kolokwium_1.Repositories;

public interface IBooksRepository
{
    Task<BooksDto> getBooks(int id);
    Task addBook(NewBookDto newBookDto);
}