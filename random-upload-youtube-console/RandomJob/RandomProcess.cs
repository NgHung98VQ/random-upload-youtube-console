using Newtonsoft.Json;
using random_upload_youtube_console.Model;
using random_upload_youtube_console.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace random_upload_youtube_console.RandomJob
{
    public class RandomProcess
    {

        private RandomUploadJobInput _inputRandom;
        private List<Detail> listDetail;
        public RandomProcess(RandomUploadJobInput inputRandom)
        {
            _inputRandom = inputRandom;
        }
        public async Task RandomUploadPost(Detail detail, string channelID, string jobId)
        {
            string tag = detail.tags;
            List<string> listTags = new List<string>();
            string[] tags = tag.Split(',');
            foreach (var h in tags)
            {
                listTags.Add(h);
            }
            var uploadJobInput = new UploadJobInput();
            uploadJobInput.ProfilePath = _inputRandom.ProfilePath;
            uploadJobInput.ExecutablePath = _inputRandom.ExecutablePath;
            uploadJobInput.ChannelName = null;
            uploadJobInput.ChannelID = channelID;
            uploadJobInput.VideoFilePath = null;
            uploadJobInput.VideoTitle = detail.title;
            uploadJobInput.VideoDescription = detail.description;
            uploadJobInput.PlaylistName = null;
            uploadJobInput.IsForKid = true;
            uploadJobInput.IsLimitedOld = true;

            uploadJobInput.Tags = listTags;
            uploadJobInput.Category = null;
            uploadJobInput.VideoLanguage = null;
            uploadJobInput.DisplayVideoMode = DisplayVideoModeType.Public;
            uploadJobInput.Schedule = null;
            uploadJobInput.IP = null;
            uploadJobInput.Port = null;
            uploadJobInput.ProxyUsername = null;
            uploadJobInput.ImageThumbGoogleDriveURL = detail.linkImgThumb;
            uploadJobInput.VideoGoogleDriveURl = detail.linkVideo;
            uploadJobInput.CookieYoutube = _inputRandom.ProfilePath + "\\" + channelID + ".txt";
            uploadJobInput.ProxyPassword = null;


            Input input = new Input();
            input.serviceName = "Upload Service";
            input.input = uploadJobInput;
            input.timeoutMilisecond = 10000000;
            input.jobId = jobId;

            var body = JsonConvert.SerializeObject(input);
            var data = new StringContent(body, Encoding.UTF8, "application/json");

            //var url = "http://localhost:6002/api/job/add";

            //await PostRequest(url, data);
        }

        public async Task RunRandomUpload()
        {
            //Request link google drive
            LogMessage.Log("Đang lấy link Google drive.....!");

            var userId = _inputRandom.UserId;
            var request = new RequestDetail();
            listDetail = await request.RunRequest(userId);
            if (_inputRandom.Timer != 0)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                Task timerTask = RunPeriodically(sendRequest, TimeSpan.FromSeconds(_inputRandom.Timer), tokenSource.Token);
            }
            else
            {
                sendRequest();
            }
        }

        static async Task RunPeriodically(Action action, TimeSpan interval, CancellationToken token)
        {
            while (true)
            {
                action();
                await Task.Delay(interval, token);
            }
        }

        public async void sendRequest()
        {
            try
            {
                List<Detail> listItems = listDetail;
                foreach (var channelID in _inputRandom.ListChannelID)
                {
                    LogMessage.Log($"Đang chọn random 1 video lên kênh {channelID}.....!");

                    Detail item;

                    int countTitle = 0;
                    int countDescription = 0;
                    int countTags = 0;
                    string idVideoGGDriver = "";
                    string idImgGGDriver = "";

                    do
                    {
                        Random random = new Random();
                        int someRandomNumber = random.Next(0, listItems.Count());
                        item = listItems[someRandomNumber];
                        countTitle = item.title.Length;
                        countDescription = item.description.Length;
                        countTags = item.tags.Length;
                        idImgGGDriver = item.linkImgThumb;
                        idVideoGGDriver = item.linkVideo;


                    } while (countTitle > 100 || countDescription > 5000 || countTags > 500 || idImgGGDriver == "" || idVideoGGDriver == "");

                    var now = DateTime.Now.Ticks;
                    //log(jobId);

                    LogMessage.Log($"Đang random 1 video lên kênh {channelID}....:{item.linkVideo}||{DateTime.Now}");
                    ////random upload process
                    //await Task.Run(() => RandomUploadPost(item, channelID, jobId));
                    //await RandomUploadPost(item, channelID, jobId);
                    //random video 
                }
            }
            catch(Exception ex)
            {
                LogMessage.Log(ex.Message);
            }
           
        }

        public async Task PostRequest(string url, StringContent data)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;

        }

       
    }
}
