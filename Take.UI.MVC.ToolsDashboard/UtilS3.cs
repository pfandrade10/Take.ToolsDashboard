using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard
{
    public class UtilS3
    {
        private string accessKey = "AKIA5Q47CMMB6TNOM6WZ";
        private string accessSecret = "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";
        private string bucket;

        public UtilS3(string bucket)
        {
            this.bucket = bucket;
        }

        public async void UploadObject(IFormFile file, string path, string fileName)
        {
            try
            {
                // connecting to the client
                var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast1);

                // get the file and convert it to the byte[]
                byte[] fileBytes = new Byte[file.Length];
                file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

                // create unique file name for prevent the mess

                using (var stream = new MemoryStream(fileBytes))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucket + '/' + path,
                        Key = fileName,
                        InputStream = stream,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await client.PutObjectAsync(request);
                };
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error encountered ***. Message:'{e.Message}' when writing an object");
            }
            catch (Exception e)
            {
                throw new Exception($"Error encountered ***. Message:'{e.Message}' when writing an object");
            }
        }

        public async void DeleteObject(string path, string fileName)
        {
            try
            {
                // connecting to the client
                var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast1);

                // create unique file name for prevent the mess

                var request = new DeleteObjectRequest
                {
                    BucketName = bucket + '/' + path,
                    Key = fileName,
                };

                await client.DeleteObjectAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when deleting an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }

        public async Task<bool> VerifyObjectAsync(string path)
        {
            try
            {
                using (var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast1))
                {
                    ListObjectsRequest request = new ListObjectsRequest
                    {
                        BucketName = bucket,
                        Prefix = path
                    };
                    var response = await client.ListObjectsAsync(request);

                    if (response.S3Objects.Any())
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when finding an object", e.Message);
                return false;
            }
        }
    }
}
