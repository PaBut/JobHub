using JobHub.Data;
using JobHub.DataAccess.IRepo;
using JobHub.DataAccess.Repo;
using JobHub.Models;

namespace JobHub.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<FileModel> FileModelRepo { get; private set; }
        public IRepository<Job> JobRepo { get; private set; }
        public IRepository<JobApplication> JobApplicationRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            FileModelRepo = new Repository<FileModel>(_context);
            JobRepo = new Repository<Job>(_context);
            JobApplicationRepo = new Repository<JobApplication>(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
