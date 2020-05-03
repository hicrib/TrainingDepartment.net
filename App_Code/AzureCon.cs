
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AviaTrain.App_Code
{
    public static class AzureCon
    {
        public static string acname = "hicristorageaccount";
        public static string acckey = "xTJ3lmsqTgnh1guj9KfMhgaMtCGCItqgBgsRWp09Nh6pg2nL4cQuuZ7LTUyMC0nuc2YZS04WfCQvh3YYtRP/uA==";
        public static string general_container = "trainingdepartmentcontainer";
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=hicristorageaccount;AccountKey=xTJ3lmsqTgnh1guj9KfMhgaMtCGCItqgBgsRWp09Nh6pg2nL4cQuuZ7LTUyMC0nuc2YZS04WfCQvh3YYtRP/uA==;EndpointSuffix=core.windows.net";
        public static string general_container_url = "https://hicristorageaccount.blob.core.windows.net/trainingdepartmentcontainer/";
        public static bool upload_ToBlob_fromFile(string fileToUpload, string azure_ContainerName = "")
        {
            try
            {
                string file_extension, filename_withExtension;
                Stream file;

                azure_ContainerName = azure_ContainerName == "" ? general_container : azure_ContainerName;

                // << reading the file as filestream from local machine >>    
                file = new FileStream(fileToUpload, FileMode.Open);

                CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);

                //checking the container exists or not  
                if (container.CreateIfNotExists())
                {
                    container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess =
                      BlobContainerPublicAccessType.Blob
                    });
                }

                //reading file name & file extension    
                file_extension = Path.GetExtension(fileToUpload);
                filename_withExtension = Path.GetFileName(fileToUpload);

                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename_withExtension);
                cloudBlockBlob.Properties.ContentType = file_extension;

                cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>  

                return true;
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }

            return false;
        }

        public static bool upload_ToBlob_fromStream(string fileName,  Stream myStream , string azure_ContainerName = "")
        {
            try
            {
                azure_ContainerName = azure_ContainerName == "" ? general_container : azure_ContainerName;
                // << reading the file as filestream from local machine >>    
                //file = new FileStream(fileToUpload, FileMode.Open);

                CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);

                //checking the container exists or not  
                if (container.CreateIfNotExists())
                {
                    container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess =
                      BlobContainerPublicAccessType.Blob
                    });
                }

                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName );
                cloudBlockBlob.Properties.ContentType = Utility.getExtension(fileName) ;

                cloudBlockBlob.UploadFromStreamAsync(myStream); // << Uploading the file to the blob >>  
                return true;
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }

            return false;
        }

        public static bool download_FromBlob(string filetoDownload, string filepath, string azure_ContainerName = "")
        {
            try
            {
                azure_ContainerName = azure_ContainerName == "" ? general_container : azure_ContainerName;

                CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);
                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetoDownload);

                // provide the file download location below            
                Stream file = File.OpenWrite(filepath + "\\" + filetoDownload); ;

                cloudBlockBlob.DownloadToStream(file);

                //now it can be read from filepath + filetoDownload
                return true;

            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return false;
        }







        public static bool uploadSuccess_deleteTemp()
        {
            return false;
        }
        public static bool downloadSuccess_deleteTemp()
        {
            return false;
        }

    }
}