using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Quirrel.Utils
{
    public class GoogleApiHandler
    {
        private UserCredential? credential;
        public GoogleApiHandler()
        {
        }

        public void ListGoogleDrive(IConfiguration configuration)
        {

            Console.WriteLine();
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
                Console.WriteLine("Credential file saved to: " + tokenPath);
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
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine($"{file.Name} ({file.Id})");
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();

        }
    }
}
