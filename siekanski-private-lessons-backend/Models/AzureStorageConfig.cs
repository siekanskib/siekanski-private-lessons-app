namespace siekanski_private_lessons_backend.Models
{
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }

        public string SasToken { get; set; }
    }
}