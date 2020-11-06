using BusinessLibrary.Model;
using DataModel.Model;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IUrlService
    {
        Task<UrlResponseModel> GetUrls();
    }
}
