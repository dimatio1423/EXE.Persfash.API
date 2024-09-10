﻿using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserSubscriptionRepos
{
    public class CustomerSubscriptionRepository : GenericRepository<CustomerSubscription>, ICustomerSubscriptionRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CustomerSubscriptionRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CustomerSubscription> GetCustomerSubscription(int customerSubscriptionId)
        {
            try
            {

                return await _context.CustomerSubscriptions
                    .Include(x => x.Subscription)
                    .Where(x => x.CustomerSubscriptionId == customerSubscriptionId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CustomerSubscription>> GetCustomerSubscriptionByCustomerId(int customerId)
        {
            try
            {

                return await _context.CustomerSubscriptions
                    .Include(x => x.Subscription)
                    .Where(x => x.CustomerId == customerId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CustomerSubscription>> GetCustomerSubscriptionBySubscriptionId(int subscriptionId)
        {
            try
            {

                return await _context.CustomerSubscriptions.Where(x => x.SubscriptionId == subscriptionId).ToListAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}