using JobHub.Contracts;
using JobHub.DataAccess;
using JobHub.Models;

namespace JobHub.Services
{
    public class FileDbUploader : IFileDbUploader
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileDbUploader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid?> UploadFileAsync(IFormFile? file)
        {
            if (file == null) return null;
            Guid? fileId;
            using(MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                FileModel fileModel = new FileModel()
                {
                    Name = file.FileName,
                    Data = stream.ToArray(),
                    Extention = file.ContentType
                };
                _unitOfWork.FileModelRepo.Add(fileModel);
                await _unitOfWork.SaveChangesAsync();
                fileId = fileModel.Id;
            }
            return fileId;
        }
    }
}
