using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public  class LexOfficeAPIHandler
    {
        public string accessToken = "8Dpt14C2Xf4wOCRswOsnXjkKbhA";

        public void UploadBasicFile(string filePath)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() != DialogResult.OK)
            //{
            //    return;
            //}
            //string filePath = openFileDialog.FileName;
            string resourceUrl = "https://api.lexoffice.io";

            var httpClient = new HttpClient();

            var formData = new MultipartFormDataContent();
            var fileStream = File.OpenRead(filePath);

            formData.Add(new StreamContent(fileStream), "file", Path.GetFileName(filePath));
            formData.Add(new StringContent("voucher"), "type");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var responseTask = httpClient.PostAsync($"{resourceUrl}/v1/files", formData);
            responseTask.Wait();

            var response = responseTask.Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"File upload failed: {response.StatusCode}");
            }

            fileStream.Close();
            httpClient.Dispose();
            formData.Dispose();

        }
    }
}
