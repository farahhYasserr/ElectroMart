using E_Commerce.Entities.Models;
using myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<ApplicatonUser> Users { get; }
        IRepository<ShoppingCart> ShoppingCarts { get; }
        IRepository<Order> Orders { get; }



        void Save();
    }
}
