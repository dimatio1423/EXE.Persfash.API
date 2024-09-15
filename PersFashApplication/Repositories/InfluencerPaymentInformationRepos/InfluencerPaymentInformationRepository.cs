using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.InfluencerPaymentInformationRepos
{
    public class InfluencerPaymentInformationRepository : GenericRepository<InfluencerPaymentInformation>, IInfluencerPaymentInformationRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public InfluencerPaymentInformationRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
