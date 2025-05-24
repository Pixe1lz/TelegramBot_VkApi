using Newtonsoft.Json.Linq;

namespace First_TelegramBot
{
    class ExtractImageClass
    {
        private readonly string tokenVK;
        private readonly string groupId;
        private string lastPostId;

        public List<string> photoUrls = new List<string>();

        public ExtractImageClass(string tokenVK, string groupId)
        {
            this.tokenVK = tokenVK;
            this.groupId = groupId;
        }

        private static readonly HttpClient client = new HttpClient();

        public async Task Run()
        {
            await GetPhotosFromGroupAsync(groupId, tokenVK);
            Console.WriteLine("Запрос на фото был отправлен");
        }

        private async Task GetPhotosFromGroupAsync(string groupId, string tokenVk)
        {
            try
            {
                string url = $"https://api.vk.com/method/wall.get?owner_id=-{groupId}&count=1&access_token={tokenVk}&v=5.131";
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);

                if (json["response"] != null)
                {
                    foreach (var post in json["response"]["items"])
                    {
                            string currentPostId = post["id"].ToString();

                        if (lastPostId != currentPostId)
                        {
                            lastPostId = currentPostId;

                            var attachments = post["attachments"] as JArray;

                            if (attachments != null)
                            {
                                foreach (var attachment in attachments)
                                {
                                    if (attachment["type"].ToString() == "photo")
                                    {
                                        var photoSizes = attachment["photo"]["sizes"] as JArray;

                                        if (photoSizes != null && photoSizes.Count > 0)
                                        {
                                            string photoUrl = photoSizes[photoSizes.Count - 1]["url"]?.ToString();
                                            photoUrls.Add(photoUrl);
                                        }
                                    }
                                }
                            }
                        } 
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка при получении данных: " + json["error"]["error_msg"]);
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        public List<string> SendGetterPhotoListURL()
        {
            return photoUrls;
        }
    }
}