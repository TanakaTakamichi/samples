using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
//赤くなる
using System.Json.JsonObject;
/*
現状は、APIのリクエスト部分のPOSTメソッド完了。
GETリクエストに関しては、レスポンスをJSONに変換したいが、
わからない。
POSTもレスポンスをJSONに変換する予定。

現在のソースは、MSDNからソースコードをみて改変して、HttpClientの使いやすくするクラスを
作成していた。
参考にしたサイト
https://code.msdn.microsoft.com/Portable-Class-LibraryHttpC-0f1499bb#content

http://stackoverflow.com/questions/12022965/adding-http-headers-to-httpclient

*/
namespace Http
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");
			var root = "http://localhost/";
			HttpClient httpclient = new HttpClient();


			Hashtable header = new Hashtable();
			header["Content-Type"] = "application/json";
			header["X-Auth-Token"] = "Token";

			var i = HttpClientExtensions.PostAsJsonAsync(httpclient,root, new Person { Id = -1, Name = "くらいあんとたろう" }, header);

		}
	}

	/// <summary>
	/// HttpClientがシームレスにJSONと連携するための拡張メソッドを提供します。
	/// </summary>
	public static class HttpClientExtensions
	{
		/// <summary>
		/// ボディ部にJSONを含むHTTPリクエストをPOSTします。
		/// </summary>
		/// <typeparam name="T">JSONにシリアライズする型</typeparam>
		/// <param name="self">拡張元のクラス</param>
		/// <param name="uri">リクエストを送信する先のURI</param>
		/// <param name="obj">ボディ部にJSONにシリアライズして含めるオブジェクト</param>
		/// <returns>レスポンス</returns>
		public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient self, string uri, T obj, Hashtable header)
		{

			var content = CreateHttpContentFromObject(obj, header);
			return self.PostAsync(uri, content);
		}
		/// <summary>
		/// API GETリクエスト投げて、レスポンス返す。
		/// Header情報追加
		/// </summary>
		public static Task<String> GetAsync<T>(this HttpClient self, string uri, T obj, Hashtable header)
		{
			var request = new HttpRequestMessage()
			{
				RequestUri = new Uri(uri),
				Method = HttpMethod.Get
			};
			foreach (DictionaryEntry de in header)
			{
				String keyName = (String)de.Key;
				String valueName = (String)de.Value;
				request.Headers.Add(keyName, valueName);
			}
			var task = self.SendAsync(request).ContinueWith((taskwithmsg) =>
			{
				var response = taskwithmsg.Result;
				//JSONに変換したいが、JsonObject赤文字になる
        //パッケージを追加しても赤文字（パッケージが間違っている可能性なのか、分からない。）
				var jsonTask = response.Content.ReadAsJsonAsync<JsonObject>();
				jsonTask.Wait();
				var jsonObject = jsonTask.Result;
			});
			task.Wait();

		}
		/// <summary>
		/// オブジェクトからJSONを含んだHttpContentを作成する
		/// </summary>
		private static HttpContent CreateHttpContentFromObject(object obj, Hashtable header)
		{

			var jsonText = JsonConvert.SerializeObject(obj);
			var content = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonText));
			foreach (DictionaryEntry de in header)
			{
				String keyName = (String)de.Key;
				String valueName = (String)de.Value;
				content.Headers.Add(keyName, valueName);
			}
			return content;
		}
		/// <summary>
		/// HttpResponseMessageのContentからJSONをオブジェクトにデシリアライズするメソッド
		/// </summary>
		/// <typeparam name="T">JSONをデシリアライズする型</typeparam>
		/// <param name="content">HttpContent</param>
		/// <returns>HttpContentから読み込んだJSONをデシリアライズした結果のオブジェクト</returns>
		public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
		{
			var binary = await content.ReadAsByteArrayAsync();
			var jsonText = Encoding.UTF8.GetString(binary, 0, binary.Length);
			return JsonConvert.DeserializeObject<T>(jsonText);
		}

	}
	public class Person
	{
		public int Id { get; set; }
		public String Name { get; set; }
	}

}
