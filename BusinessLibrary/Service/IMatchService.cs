using BusinessLibrary.Model;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IMatchService
    {
        Task<MatchResponseModel> GetMatches();
        Task<Match> GetMatch(long id);
    }
}
