using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotsController.Core.Interfaces
{
    public interface ISpeechGenerator
    {
        MemoryStream SynthesizeSpeech(string text);
    }
}
