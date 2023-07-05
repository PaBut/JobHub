namespace JobHub.Contracts
{
    public interface IFileDbUploader
    {
        public Task<Guid?> UploadFileAsync(IFormFile? file);
    }
}
