using Kolokwium_1.Models.DTOs;
using Kolokwium_1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _booksRepository.getBooks(id);

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(NewBookDto newBookDto)
    {
        await _booksRepository.addBook(newBookDto);
        return Created();
    }


}