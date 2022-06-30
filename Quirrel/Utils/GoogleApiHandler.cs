using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;

namespace Quirrel.Utils
{
    public class GoogleApiHandler: IGoogleApiHandler
    {
        private UserCredential? credential;
        private readonly ILogger<GoogleApiHandler> logger;
        private readonly IConfiguration configuration;
        public GoogleApiHandler(IConfiguration configuration, ILogger<GoogleApiHandler> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }


        public void ListGoogleDrive()
        {

            using (var stream = new FileStream(configuration.GetSection("UserConfig")["PathToCredentials"], FileMode.Open, FileAccess.Read))
            {
                string tokenPath = "token.json";
                string[] Scopes = { DriveService.Scope.DriveReadonly };

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "Quirrel",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true)).Result;

                logger.LogInformation("Credential file saved to: " + tokenPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Quirrel",
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Q = "'16dg5NmFDzUTvFfAKVBg3THFckGrDhW9W' in parents";

            // List files.
            var files = listRequest.Execute().Files;
            StringBuilder stringBuilder = new StringBuilder();

            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    stringBuilder.Append(file.Name);
                    stringBuilder.Append(" ");
                }

                logger.LogInformation(stringBuilder.ToString());
                Console.WriteLine(stringBuilder.ToString());
            }
            else
            {
                Console.WriteLine("No files found.");
            }

        }
    }
}
