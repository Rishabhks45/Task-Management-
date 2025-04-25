using SimpleTaskTrackr.Models.SimpleTaskTrackModel;

namespace SimpleTaskTrackr.Repository
{
    public interface IRepository
    {
       Task<Userproperty> CreateUser();
    }
}
