#r "nuget: Newtonsoft.Json, 12.0.1"

using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

var videoUrl = Args[0];
var id = Regex.Match(videoUrl, @"youtube\.com\/watch\?v=([\w-]{11})").Groups[1].Value;

var httpClient = new HttpClient();
var response = await httpClient.GetStringAsync($"https://www.youtube.com/get_video_info?video_id={id}");

var decoded = WebUtility.UrlDecode(response);

var playerResponse = decoded.Split('&')
    .First(p => p.StartsWith("player_response="))
    .Remove(0, "player_response=".Length);

var json = JObject.Parse(playerResponse);

var mp4Url = (string)json["streamingData"]["adaptiveFormats"]
    .FirstOrDefault(f => f["mimeType"].Value<string>().StartsWith("audio/mp4"))["url"];

Console.WriteLine(mp4Url);