namespace PustokApp.Extensions
{
    public static class FileExtensions
    {
        public static bool CheckFileType(this IFormFile file,string fileType)
        {
            if(file.ContentType.StartsWith(fileType))
            {
                return true;
            }
            return false;
        }

        public static bool CheckFileSize(this IFormFile file, int fileSize)
        {
            if(file.Length < fileSize * 1024)
            {
                return true;
            }
            return false;
        }

        public static async Task<string> SaveFileAsync(this IFormFile file, string root, string client, string folderName, string imageFolder, string productFolder)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = Path.Combine(root, client, folderName, imageFolder, productFolder, uniqueFileName);

            using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            await file.CopyToAsync(fs);
            return uniqueFileName;
        }

        public static async void DeleteFile(this IFormFile file, string root, string client, string folderName, string fileName)
        {

            string path = Path.Combine(root, folderName, client, fileName);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
