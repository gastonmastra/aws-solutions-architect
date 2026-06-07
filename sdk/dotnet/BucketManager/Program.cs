using Amazon.S3.Model;
using BucketManager;

int ShowOptions(List<Option> options)
{
    Console.WriteLine("\n========== S3 Manager ==========\n");
    for (var i = 0; i < options.Count; i++)
        Console.WriteLine($"{i + 1}. {options[i]}");
    var option = ReadId(options);
    return option;

}

static int ReadId(List<Option> options)
{
    Console.Write("What would you like to do?: ");
    var userInput = Console.ReadLine();
    int id;
    while (!int.TryParse(userInput, out id) | id < 1 | id > options.Count)
    {
        Console.Write("Please, enter a valid number:");
        userInput = Console.ReadLine();
    }
    return id - 1;
}

async Task Main(List<Option> options)
{
    Console.WriteLine("Welcome to the Bucket Manager!");
    int option;
    do
    {
        option = ShowOptions(options);
        string optionName = options[option].ToString();
        Console.WriteLine($"Option {optionName} selected.\n");
        await options[option].RunAsync();
        Console.WriteLine($"Option {optionName} completed.\n");
    }
    while (option != options.Count - 1);
}

var options = new List<Option>()
{
    new CreateBucket(),
    new ListBuckets(),
    new DeleteBucket(),
    new EmptyBucket(),
    new PutObject(),
    new ListObjects(),
    new GetObject(),
    new Exit(),
};

await Main(options);