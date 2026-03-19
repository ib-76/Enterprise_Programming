using Common.Interfaces;
using Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Decorator design pattern
    //This class is a decorator for the IOrdersRepository,
    //it adds logging functionality to the existing implementation of IOrdersRepository without modifying its code.

    public class LoggingOrdersRepository : IOrdersRepository
    {

        private IOrdersRepository _innerOrdersRepository;
        private ILogger<LoggingOrdersRepository> _logging;

        public LoggingOrdersRepository(IOrdersRepository innerOrdersRepository,
            ILogger<LoggingOrdersRepository> logger)
        {
            _innerOrdersRepository = innerOrdersRepository;
            _logging = logger;
        }

        public void Checkout(List<OrderItem> orderItems, string username)
        {
            _logging.LogInformation($"About to checkout on {orderItems.Count}");

            _innerOrdersRepository.Checkout(orderItems, username);

            _logging.LogInformation($"checkout ready on {orderItems.Count}");
        }
    }
}