using BusinessLibrary.Model;
using DataModel.Model;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IPatchVersionService
    {
        Task<PatchVersion> GetPatchVersion(string name);
    }
}
