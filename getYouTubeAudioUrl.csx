#r "nuget: Newtonsoft.Json, 12.0.1"

using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

var id = Regex.Match(Args[0], @"youtube\.com\/watch\?v=([\w-]{11})").Groups[1].Value;

var response = await new HttpClient()
    .GetStringAsync($"https://www.youtube.com/get_video_info?video_id={id}");

var decoded = WebUtility.UrlDecode(response);

var key = "player_response=";
var playerResponse = decoded.Split('&')
    .First(p => p.StartsWith(key))
    .Remove(0, key.Length);

var json = JObject.Parse(playerResponse);

var mp4Url = (string)json["streamingData"]["adaptiveFormats"]
    .FirstOrDefault(f => ((string)f["mimeType"]).StartsWith("audio/mp4"))["url"];

Console.WriteLine(mp4Url);