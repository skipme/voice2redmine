// -----------------------------------------------------------------------
// <copyright file="Recognizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace vtmblip.ext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Collections.Specialized;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRecognizer
    {
        GoogleResponse RecognizeFlac(string flacpath);
    }

    /*[JsonObject(MemberSerialization.OptIn)]
    public class GoogleHypothesa
    {
        [JsonProperty]
        public string utterance { get; set; }

        [JsonProperty]
        public double confidence { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleResponse
    {
        [JsonProperty]
        public int status { get; set; }
        [JsonProperty]
        public string id { get; set; }
        [JsonProperty]
        public GoogleHypothesa[] hypotheses { get; set; }
    }*/

    public class Alternative
    {
        public string transcript { get; set; }
        public double confidence { get; set; }
    }

    public class Result
    {
        public List<Alternative> alternative { get; set; }
        public bool final { get; set; }
    }

    public class GoogleResponse
    {
        public List<Result> result { get; set; }
        public int result_index { get; set; }
    }
    public class GoogleRecognizer : IRecognizer
    {

        public GoogleResponse RecognizeFlac(string flacpath)
        {
            NameValueCollection parameters = new NameValueCollection();
            //parameters.Add("lang", "ru");
            //parameters.Add("client", "chromium");
            //?lang=ru&client=chromium
            // "http://www.google.com/speech-api/v1/recognize?lang=ru&client=chromium"
            string url = "https://www.google.com/speech-api/v2/recognize?output=json&lang=ru&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw&results=1&pfilter=2";
            string result = WebUpload.UploadFileEx(flacpath, url,
                 "file", "audio/x-flac; rate=16000", parameters, null);
            string[] answers = result.Split(new char[] { '\n' });
            GoogleResponse answr = null;
            for (int i = 0; i < answers.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(answers[i])) continue;
                answr = JsonConvert.DeserializeObject<GoogleResponse>(answers[i]);
                if (answr.result.Count > 0 && answr.result[0].alternative.Count > 0)
                {
                    break;
                }
                else
                {
                    answr = null;
                }
            }
            return answr;
        }
    }
}
