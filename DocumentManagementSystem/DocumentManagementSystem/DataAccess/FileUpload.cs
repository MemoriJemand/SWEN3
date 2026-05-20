using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace DocumentManagementSystem.DataAccess
{
    public class FileUpload
    {
                
        public async static Task<string> Run(IMinioClient minio, string file)
        {
            var bucketName = "documents";
            var objectName = Path.GetRandomFileName();
            var filePath = file;
            var contentType = "application/pdf";

            try
            {
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath)
                    .WithFileName(filePath)
                    .WithContentType(contentType);
                await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                Console.WriteLine("Successfully uploaded " + filePath);
                return filePath;
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                return "";
            }
        }
    }
}
