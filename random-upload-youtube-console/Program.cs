using random_upload_youtube_console.Model;
using random_upload_youtube_console.RandomJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


List<string> list = new List<string>();
list.Add("fdsfdsahfdsafsdgfdhgsfsfd");
list.Add("fdsfsfdsfdsafsdfsfsdafsdfd");
list.Add("fdsfsfdsfsdfsdfdsfdsafdsfd");
list.Add("fdsfdsfasdagfdgdfdgdffasfsfd");
RandomUploadJobInput randomInput = new RandomUploadJobInput();
randomInput.UserId = "801391c1-ac24-4593-b97e-7575752a3fb5";
randomInput.Timer = 1000;
randomInput.ListChannelID = list;
RandomProcess randomProcess = new RandomProcess(randomInput);
await randomProcess.RunRandomUpload();

//Console.ReadKey();

//namespace random_upload_youtube_console
//{
//    class Program
//    {
//        async static void Main(string[] args)
//        {
//            RandomUploadJobInput randomInput = new RandomUploadJobInput();
//            randomInput.UserId = "801391c1-ac24-4593-b97e-7575752a3fb5";
//            randomInput.Timer = 0;
//            RandomProcess randomProcess = new RandomProcess(randomInput);
//            await randomProcess.RunRandomUpload();
//        }
//    }
//}