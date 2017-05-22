using System;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

using System.Net;
using System.Text;
using Http.param;
using System.Web;
namespace Http
{
	
	public class SimpleRequestClient
	{
		private HttpWebRequest httpWebRequst = null;

		public String HttpPostRequst(String url, Object obj, Hashtable headers)
		{

			if (obj == null)
			{
				return null;
			}
			String json = ConvertToJson(obj);
			httpWebRequst = (HttpWebRequest)HttpWebRequest.Create(url);
			httpWebRequst.Method = Param.POST;
			httpWebRequst.ContentType = "application/json; charset=utf-8";
			foreach (DictionaryEntry dictionaryEntryDate in headers)
			{
				httpWebRequst.Headers.Add(dictionaryEntryDate.Key.ToString()
										  , dictionaryEntryDate.Value.ToString());
			}
			using (var stremWriter = new StreamWriter(httpWebRequst.GetRequestStream()))
			{
				stremWriter.Write(json);
				stremWriter.Flush();
			}
			var HttpReponse = (HttpWebResponse)httpWebRequst.GetResponse();
			if (HttpReponse.StatusCode != HttpStatusCode.OK)
			{
				return null;
			}
			using (var streamReader = new StreamReader(httpWebRequst.GetRequestStream()))
			{
				var result = streamReader.ReadToEnd();
				return result;
			}

		}
		public String HttpGetRequst(String url, Hashtable parameters, Hashtable headers)
		{
			String result = String.Empty;


			if (parameters == null)
			{
				return null;
			}	
			String urlParameters = CreateParameters(url,parameters);
			httpWebRequst = (HttpWebRequest)HttpWebRequest.Create(urlParameters);
			httpWebRequst.Method = Param.GET;
			httpWebRequst.ContentType = "charset=utf-8";
			/*
			foreach (DictionaryEntry dictionaryEntryDate in headers)
			{
				httpWebRequst.Headers.Add(dictionaryEntryDate.Key.ToString()
										  , dictionaryEntryDate.Value.ToString());
			}
			*/
			using (var responce = (HttpWebResponse)httpWebRequst.GetResponse())
			{
				if (responce.StatusCode != HttpStatusCode.OK)
				{
					return null;
				}
				using (Stream stream = responce.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
					{
						result = reader.ReadToEnd();
					}
				}
			}

			//System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");
			//result = HttpUtility.UrlDecode(result,enc);
			return result;
		}
		public String ConvertToJson(Object obj)
		{
			string jsonString = JsonConvert.SerializeObject(obj);
			return jsonString;
		}
		public String CreateParameters(String url, Hashtable parameter)
		{
			String urlParameters = String.Empty;
			urlParameters += urlParameters + url + "?";
			String question = "?";
			String and = "&";
			String equal = "=";
			//urlParameters += question;
			foreach (DictionaryEntry dictionaryEntry in parameter) 
			{
				if (urlParameters.EndsWith(question))
				{
					String key = dictionaryEntry.Key.ToString();
					String value = dictionaryEntry.Value.ToString();
					urlParameters = urlParameters + key + equal + value;
				}
				else
				{
					String key = dictionaryEntry.Key.ToString();
					String value = dictionaryEntry.Value.ToString();
					urlParameters = urlParameters + and + key + equal + value;
				}

			}
			return urlParameters;
		}
	}

}
