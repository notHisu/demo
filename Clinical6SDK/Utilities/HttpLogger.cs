using System;
using System.Collections.Generic;
using System.Text;

namespace Clinical6SDK.Utilities
{
    public class HttpLogger
    {
        public static void Output(string request, string response)
        {
            const int LINE_MAX = 1024;
            int lines = response.Length / LINE_MAX + 1;

            Console.WriteLine($"P6-Request:REQ-START(RES={lines}){request}REQ-END");

            int i;
            for (i = 0; i < lines; i++)
            {
                int remain = LINE_MAX;
                if (i == lines - 1)
                    remain = response.Length % LINE_MAX;

                string contentI = response.Substring(LINE_MAX * i, remain);
                Console.WriteLine($"P6-Response:RES-START{i}{contentI}RES-END{i}");
            }
        }
    }
}
