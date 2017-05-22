using System;
using Newtonsoft.Json; 
using System.Net.Http; 
using System.Net.Http.Headers; 
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
namespace Http
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			SimpleRequestClient client = new SimpleRequestClient();
			//?city=400040
			String url = "http://weather.livedoor.com/forecast/webservice/json/v1";
			Hashtable table = new Hashtable();
			table["city"] = "400040";
			String json = client.HttpGetRequst(url, table, null);
			Console.WriteLine(json);
			Console.ReadKey();
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
		public static async Task<string> GetAsync(this HttpClient self)
		{
			var req = new HttpRequestMessage()
			{
				RequestUri = new Uri("http://google.com"),
				Method = HttpMethod.Get    
			};
			/*
			foreach (DictionaryEntry de in header)
			{
				String keyName = (String)de.Key;
				String valueName = (String)de.Value;
				req.Headers.Add(keyName, valueName);
			}
			*/
			HttpResponseMessage ht = new HttpResponseMessage();
			var responce = new HttpResponseMessage();
			Task<HttpResponseMessage> task = null;
			task = self.SendAsync(req);
			Console.WriteLine(task);
			Console.ReadKey();
			return null;


			/*
			return self.SendAsync(req).ContinueWith(request =>
			{
				var response = request.Result;
				response.EnsureSuccessStatusCode();
				return response.Content.ReadAsJsonAsync(response);

			}).Unwrap();
			*/
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
		/*
		public static string GetAny(string url, List<KeyValuePair<string, string>> parameters,Hashtable header)
		{
			string res = "";

			try
			{
				//IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

				//Prepare url
				Uri mainurl = new Uri("http://zip.cgis.biz/");
				Uri requesturl = new Uri(mainurl, url);

				var httpClient = new HttpClient();
				var httpContent = new HttpRequestMessage(HttpMethod.Get, requesturl);
				// httpContent.Headers.ExpectContinue = false;
				foreach (DictionaryEntry de in header)
				{
					String keyName = (String)de.Key;
					String valueName = (String)de.Value;
					httpContent.Headers.Add(keyName, valueName);
				} 
				httpContent.Content = new FormUrlEncodedContent(parameters);

				HttpResponseMessage response = httpClient.SendAsync(httpContent);

				var result = response.Content.ReadAsStringAsync();
				res = result.ToString();

				response.Dispose();
				httpClient.Dispose();
				httpContent.Dispose();
			}
			catch (Exception ex)
			{
				//Logger l = new Logger();
				//l.LogInfo("Get_API_Result_String: " + url + ex.Message.ToString());
				ex = null;
				//l = null;
			}

			return res;
		}
	*/
	}

	public class Person
	{
		public int Id { get; set; }
		public String Name { get; set; }
	}

}

