// -----------------------------------------------------------------------
// <copyright file="VoiceText.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace voice2redmine.Recognise
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using vtmblip.wave;
    using System.IO;
    using vtmblip.ext;

    
    
    public class VoiceTextData
    {
        public string Recognised { get; set; }
        public double Rating { get; set; }
    }
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoiceText
    {
        public static VoiceTextData RecordText(int seconds)
        {
            string file = Path.GetTempFileName();// +".flac";
            Recorder rec = new Recorder(file);
            System.Threading.Thread.Sleep(seconds * 1000);
            rec.Stop();
            while (!rec.AllDone) { }
            GC.SuppressFinalize(rec);
            GoogleRecognizer recogn = new GoogleRecognizer();

            GoogleResponse respdata = null;
            try
            {
                respdata = recogn.RecognizeFlac(file);
            }
            catch (Exception e)
            {
                respdata = null;
            }
            if (respdata == null)
                return null;

            File.Delete(file);
            //Json
            return new VoiceTextData() { Rating = respdata.result[0].alternative[0].confidence, Recognised = respdata.result[0].alternative[0].transcript };
        }
    }
}
