using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using BotsController.Core.Interfaces;

namespace BotsController.Core.Helpers
{
    public class SpeechGenerator: ISpeechGenerator
    {
        public byte[] SynthesizeSpeech(string text)
        {
            var pollyClient = new AmazonPollyClient(
                Environment.GetEnvironmentVariable("POLLY_KEY_ID"),
                Environment.GetEnvironmentVariable("POLLY_SECRET_KEY"),
                RegionEndpoint.EUWest2);

            var request = new SynthesizeSpeechRequest
            {
                Text = text,
                OutputFormat = OutputFormat.Mp3,
                VoiceId = VoiceId.Maxim
            };
            var result = pollyClient.SynthesizeSpeechAsync(request).Result;

            return StreamToBytes(result.AudioStream);
        }

        private static byte[] StreamToBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}
