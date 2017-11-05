using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System.IO;

namespace WindowsFormsApp1
{
    internal class FileDotEngine : IDotEngine
    {
        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            string executable = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe";
            string output = @"C:\Users\Jesper\source\repos\Considition\Considition\bin\Debug\output";
            File.WriteAllText(output + ".dot", dot);

            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Stop the process from opening a new window
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Setup executable and parameters
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = string.Format(@"{0}.dot -Tpng -o {0}.png", output);

            // Go
            process.Start();
            // and wait dot.exe to complete and exit
            process.WaitForExit();
            return output;
        }
    }
}