using BusinessLibrary.Model;
using DataModel;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IBuildService
    {
        Task<Build> GetBuild(long id);
        Task<BuildResponseModel> GetBuilds(ApplicationUser user, DataTableParameters p);
        Task<Build> CreateBuild(Build build, ApplicationUser user);
        Task<Build> UpdateBuild(Build build);
        Task<bool> DeleteBuild(Build build);
    }
}
