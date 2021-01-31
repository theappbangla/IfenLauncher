using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IfenLauncher.UpdateCenter
{
    public class IfenGameUpdate
    {

        public IfenGameUpdate()
        {
        }

        async public void fetch(Action<bool, GameUpdate> callback)
        {

            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(GameUtils.GAME_UPDATE_URL);
                    request.Method = HttpMethod.Post;
                    GameUpdate postObj = new GameUpdate();
                    postObj.gameid = GameUtils.GAME_ID;
                    postObj.version = GameUtils.GAME_VERSION;
                    var content = new StringContent(JsonConvert.SerializeObject(postObj), Encoding.UTF8, "application/json");
                    request.Content = content;
                    var httpClient = new HttpClient();
                    using (var response = await httpClient.SendAsync(request))
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<GameUpdate>(data);
                        
                        if (result.version > GameUtils.GAME_VERSION)
                        {
                            callback(true, result);
                        } else
                        {
                            callback(false, result);
                        }
                    }
                }
            } catch (Exception e)
            {
                callback(false, null);
            }
        }

    }
}
