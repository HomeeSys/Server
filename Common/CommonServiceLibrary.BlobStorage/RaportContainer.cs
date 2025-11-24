public class RaportContainer
{
    private IConfiguration _configuration;
    private BlobServiceClient _blobServiceClient;
    private BlobContainerClient _containerClient;

    public RaportContainer(IConfiguration configuration)
    {
        _configuration = configuration;

        string connectionString = _configuration.GetValue<string>("AzureBlob:ConnectionString");
        string containerName = _configuration.GetValue<string>("AzureBlob:ContainerName");

        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
    }

    //public async Task<bool> UploadDocumentAsync(Document document)
    //{
    //    if (document == null)
    //    {
    //        throw new Exception();
    //    }

    //    string title = $"{document.GetMetadata().Title!}.pdf";

    //    using var stream = new MemoryStream();
    //    document.GeneratePdf(stream);

    //    stream.Position = 0;
    //    var response = await _containerClient.UploadBlobAsync(title, stream);

    //    return true;
    //}

    //public async Task<string[]> GetExistingRaportsNames()
    //{
    //    List<string> names = new List<string>();
    //    var blobs = _containerClient.GetBlobsAsync().AsPages();

    //    await foreach (var blob in blobs)
    //    {
    //        foreach (var item in blob.Values)
    //        {
    //            names.Add(item.Name);
    //        }
    //    }

    //    return names.ToArray();
    //}
}