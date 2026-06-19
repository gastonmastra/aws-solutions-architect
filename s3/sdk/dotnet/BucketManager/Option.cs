using Amazon.S3;
using Amazon.S3.Model;

namespace BucketManager;

public abstract class Option(string description)
{
    private string Description { get; } = description;
    public override string ToString()
    {
        return Description;
    }


    protected static string ReadUserInput(string placeholder)
    {
        string? filePath;
        do
        {
            Console.Write(placeholder);
            filePath = Console.ReadLine();
        } while (string.IsNullOrEmpty(filePath));
        return filePath;
    }

    public abstract Task RunAsync();
}

public class CreateBucket : Option
{
    public CreateBucket() : base("Create Bucket") { }

    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter the bucket name: ");
        var client = new AmazonS3Client();
        try
        {
            await client.PutBucketAsync(bucketName);
            Console.WriteLine($"Bucket {bucketName} created succesfully.");
        }catch (Exception ex)
        {
            Console.WriteLine($"Error creating the new bucket {bucketName}\n{ex.Message}");
        }
    }
}

public class ListBuckets : Option
{
    public ListBuckets() : base("List buckets") { }

    public override async Task RunAsync()
    {
        Console.WriteLine("Getting buckets...");
        var client = new AmazonS3Client();
        var response = await client.ListBucketsAsync();
        var buckets = response.Buckets;
        Console.WriteLine($"Number of Buckets: {buckets.Count}");
        foreach(var bucket in buckets)
        {
            Console.WriteLine($"{bucket.BucketName}");
        }
    }
}

public class Exit : Option
{
    public Exit() : base("Exit program") {}

    public override async Task RunAsync()
    {
        Console.WriteLine("Bye bye!");
    }
}

public class DeleteBucket : Option
{
    public DeleteBucket() : base("Delete bucket") {}
    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter the bucket name: ");
        var client = new AmazonS3Client();
        try
        {
            await client.DeleteBucketAsync(bucketName);
            Console.WriteLine($"Bucket {bucketName} deleted succesfully");
        }catch (Exception ex)
        {
            Console.WriteLine($"Error deleting bucket {bucketName}.\n{ex.Message}");
        }
    }
}

public class EmptyBucket : Option
{
    public EmptyBucket() : base("Empty bucket") {}
    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter bucket name: ");
        var client = new AmazonS3Client();
        try
        {
            var listObejctsRequest = new ListObjectsV2Request()
            {
                BucketName = bucketName
            };
            var response = await client.ListObjectsV2Async(listObejctsRequest);
            var objects = response.S3Objects;
            if (objects is not null)
            {
                Console.WriteLine($"The bucket {bucketName} has {objects.Count} objects");
                List<KeyVersion> objectKeys = objects.Select(o => new KeyVersion
                {
                    Key = o.Key
                }).ToList();
                var request = new DeleteObjectsRequest()
                {
                    Objects = objectKeys,
                    BucketName = bucketName
                };
                await client.DeleteObjectsAsync(request);
                Console.WriteLine($"The bucket {bucketName} is now empty.");
            }
            else
            {
                Console.WriteLine($"No objects found in the bucket {bucketName}.");
            }
        }catch (Exception ex)
        {
            Console.WriteLine($"Error deleting bucket {bucketName}.\n{ex.Message}");
        }
    }
}

public class PutObject : Option
{
    public PutObject() : base("Put object") {}

    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter bucket name: ");
        var filePath = ReadUserInput("Enter file path: ");
        var objectKey = ReadUserInput("Enter object key: ");
        var client = new AmazonS3Client();
        var putObjectRequest = new PutObjectRequest
        {
            Key = objectKey,
            FilePath = filePath,
            BucketName = bucketName
        };
        try
        {
            await client.PutObjectAsync(putObjectRequest);
            Console.WriteLine($"Object {objectKey} has been succesfully uploaded to {bucketName} bucket.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error puting object {objectKey}.\n{ex.Message}");
        }
    }
}

public class ListObjects : Option
{
    public ListObjects() : base("List objects") { }

    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter bucket name: ");
        var client = new AmazonS3Client();

        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };
            var response = await client.ListObjectsV2Async(request);
            var objects = response.S3Objects;
            if (objects is not null)
            {
                Console.WriteLine($"{objects.Count} objects found.");
                foreach (var s3Object in objects)
                {
                    Console.WriteLine($"{s3Object.Key}, size: {s3Object.Size}, ETag: {s3Object.ETag}");
                }
            }
            else
            {
                Console.WriteLine($"No objects found in bucket ${bucketName}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error listing objects for bucket {bucketName}.\n{ex.Message}");
        }
    }
}

public class GetObject : Option
{
    public GetObject() : base("Get object") { }

    public override async Task RunAsync()
    {
        var bucketName = ReadUserInput("Enter bucket name: ");
        var objectKey = ReadUserInput("Enter object key: ");
        var client = new AmazonS3Client();
        try
        {
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = objectKey
            };
            var response = await client.GetObjectAsync(request);
            using var reader = new StreamReader(response.ResponseStream);
            string content = await reader.ReadToEndAsync();
            Console.WriteLine($"Content: {content}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error getting object {objectKey} in {bucketName}.\n{ex.Message}");
        }
    }
}