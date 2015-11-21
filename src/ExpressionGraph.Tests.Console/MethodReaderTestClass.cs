using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System;
using ExpressionGraph.Reflection;

namespace ExpressionGraph.Tests.Console
{
    public class MethodReaderTestClass : IMethodRead
    {

        public bool CanRead(object obj, MethodInfo methodInfo)
        {
            return obj is TestClass && methodInfo.GetParameters().Length > 0;
        }

        public IEnumerable<MethodValue> GetValues(object obj, MethodInfo methodInfo)
        {
            if (methodInfo.Name == "Beep")
            {
                Note[] melody = new Note[] {
                    new Note(Note.C, 0, 100),
                    new Note(Note.C, 0, 100),
                    new Note(Note.D, 0, 100),
                    new Note(Note.E, 0, 100),
                    new Note(Note.F, 0, 100),
                    new Note(Note.G, 0, 100),
                    new Note(Note.A, 0, 100),
                    new Note(Note.B, 0, 100),
                    new Note(Note.C, 1, 100),
                    new Note(Note.D, 1, 100),
                    new Note(Note.E, 1, 100),
                    new Note(Note.F, 1, 100),
                    new Note(Note.G, 1, 100),
                    new Note(Note.A, 1, 100),
                    new Note(Note.B, 1, 100),
                    new Note(Note.C, 2, 100)
                };

                var parameters = methodInfo.GetParameters();
                foreach (var note in melody)
                {
                    var value = methodInfo.Invoke("Beep", new object[] { note.Frequency, note.Duration });
                    var parameter = new MethodValueParam(parameters[0].Name, parameters[0], note.Frequency);
                    var parameter1 = new MethodValueParam(parameters[1].Name, parameters[1], note.Duration);
                    yield return new MethodValue(value, parameter, parameter1);
                }
            }
        }

        class Note
        {
            public const int C = -888;
            public const int CSharp = -798;
            public const int DFlat = CSharp;
            public const int D = -696;
            public const int DSharp = -594;
            public const int EFlat = DSharp;
            public const int E = -498;
            public const int F = -390;
            public const int FSharp = -300;
            public const int GFlat = FSharp;
            public const int G = -192;
            public const int GSharp = -96;
            public const int AFlat = GSharp;
            public const int A = 0;
            public const int ASharp = 108;
            public const int BFlat = ASharp;
            public const int B = 204;
            const double ConcertPitch = 440.0;

            public int Key { get; set; }
            public int Octave { get; set; }
            public int Duration { get; set; }

            public Note(int key, int octave, int duration)
            {
                this.Key = key;
                this.Octave = octave;
                this.Duration = duration;
            }

            public int Frequency
            {
                get
                {
                    double factor = Math.Pow(2.0, 1.0 / 1200.0);
                    return ((int)(ConcertPitch * Math.Pow(factor, this.Key + this.Octave * 1200.0)));
                }
                //}

                //public void Play()
                //{
                //    Beep(this.Frequency, this.Duration);
                //}
            }
        }
    }
}