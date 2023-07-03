using JobHub.DataAccess.IRepo;
using JobHub.Models;

namespace JobHub.DataAccess
{
    public interface IUnitOfWork
    {
        public IRepository<FileModel> FileModelRepo { get; }
        public IRepository<Job> JobRepo { get; }
        public IRepository<JobApplication> JobApplicationRepo { get; }

        public void SaveChanges();
    }
}
