using E_Commerce.DataAccess.Data;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using Microsoft.EntityFrameworkCore;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Category> _CategoryRepository;
        private IRepository<Product> _ProductRepository;
        private IRepository<ApplicatonUser> _ApplicationUserRepository;
        private IRepository<ShoppingCart> _ShoopingCartRepository;
        private IRepository<Order> _OrderRepository;




        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Category> Categories
        {
            get { return _CategoryRepository ??= new Repository<Category>(_context); }
        }


        public IRepository<Product> Products
        {
            get { return _ProductRepository ??= new Repository<Product>(_context); }
        }

        public IRepository<ApplicatonUser> Users
        {
            get { return _ApplicationUserRepository ??= new Repository<ApplicatonUser>(_context); }
        }
        public IRepository<ShoppingCart> ShoppingCarts
        {
            get { return _ShoopingCartRepository ??= new Repository<ShoppingCart>(_context); }
        }

        public IRepository<Order> Orders
        {
            get { return _OrderRepository ??= new Repository<Order>(_context); }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
