using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;

namespace LedloveBackend.Daemon
{
    public class Twitter
    {
        public static String GetLatest(String screen_name) {
            String url = "https://api.twitter.com/1/statuses/user_timeline.json?screen_name=" + screen_name;
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 1;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
                //Console.WriteLine("Connecting to {0} - length is {1}", url, response.ContentLength);
                //Console.WriteLine("Content type is {0}", response.ContentType);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                ArrayList hashtags = new ArrayList();

                // Reading
                JArray jsonDat = JArray.Parse(readStream.ReadToEnd());
                response.Close();
                readStream.Close();

                if (jsonDat.Count() > 0) {
                    return jsonDat[0]["text"].ToString();
                } else {
                    return null;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error fetching remote data", ex.Message);
                return null;
            }

            /*
            for (int x = 0; x < jsonDat.Count(); x++)
            {
                    //JObject.Parse(jsonDat[x]["text"].ToString());
                JArray arrHashtags = JArray.Parse(entity["hashtags"].ToString());
                for (int i = 0; i < arrHashtags.Count(); i++)
                {
                    JObject hashtagstuff = JObject.Parse(arrHashtags[i].ToString());
                    hashtags.Add(hashtagstuff["text"].ToString());
                }
            }
             * */

        }
    }
}