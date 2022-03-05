﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using LibApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;

        public BooksController(ApplicationDbContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            //var books = _context.Books
            //    .Include(b => b.Genre)
            //    .ToList();

            var books = _bookRepository.GetBooks();

            return View(books);
        }

        public IActionResult Details(int id)
        {
            //var book = _context.Books
            //    .Include(b => b.Genre)
            //    .SingleOrDefault(b => b.Id == id);

            var book = _bookRepository.GetBooks().SingleOrDefault(b => b.Id == id);

            return View(book);
        }
        [Authorize(Roles = "Owner")]
        [Authorize(Roles = "StoreManager")]
        public IActionResult Edit(int id)
        {
            //var book = _context.Books.SingleOrDefault(b => b.Id == id);
            var book = _bookRepository.GetBooks().SingleOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookFormViewModel
            {
                Book = book,
                Genres = _context.Genre.ToList()
            };

            return View("BookForm", viewModel);
        }

        public IActionResult New()
        {
            var viewModel = new BookFormViewModel
            {
                Genres = _context.Genre.ToList()
            };

            return View("BookForm", viewModel);
        }

        public void DeleteCustomer(int id)
        {
            var bookInDb = _bookRepository.GetBookByID(id);
            if (bookInDb == null)
            {
                throw new Exception();
            }

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult Save(Book book)
        {
            if (book.Id == 0)
            {
                book.DateAdded = DateTime.Now;
                //_context.Books.Add(book);
                _bookRepository.AddBook(book);
            }
            else
            {
                //var bookInDb = _context.Books.Single(b => b.Id == book.Id);
                var bookInDb = _bookRepository.GetBooks().Single(b => b.Id == book.Id);
                bookInDb.Name = book.Name;
                bookInDb.AuthorName = book.AuthorName;
                bookInDb.GenreId = book.GenreId;
                bookInDb.ReleaseDate = book.ReleaseDate;
                bookInDb.DateAdded = book.DateAdded;
                bookInDb.NumberInStock = book.NumberInStock;
            }

            try
            {
                //_context.SaveChanges();
                _bookRepository.Save();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Books");
        }
        [HttpGet]
        [Route("api/books")]
        [Authorize(Roles = "User, StoreManager")]
        public IList<Book> GetBooks()
        {
            return _bookRepository.GetBooks().ToList();
        }




    }
}
