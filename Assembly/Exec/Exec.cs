using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace PetSerAl.Build {
    internal static class Exec {
        private static readonly UTF8Encoding utf8WithoutBom = new UTF8Encoding(false);
        public static string CatFile(string @object) => InvokeCommand($"git cat-file -t -- {EscapeArgument(@object)}");
        public static string CommitTree(string tree, string[] parents, string[] message) {
            string messageString = string.Concat(Array.ConvertAll(message, m => $" -m {EscapeArgument(m)}"));
            string parentsString = string.Concat(Array.ConvertAll(parents, p => $" -p {EscapeArgument(p)}"));
            return InvokeCommand($"git commit-tree{messageString}{parentsString} {EscapeArgument(tree)}");
        }
        public static string HashObject(string path, string diskPath) => InvokeCommand($"git hash-object -w --path {EscapeArgument(path)} -- {EscapeArgument(diskPath)}");
        public static string Mktree(string input) => InvokeCommand("git mktree", input);
        public static string RevParse(string arg) => InvokeCommand($"git rev-parse --verify {EscapeArgument(arg)}");
        private static string EscapeArgument(string arg) => $"\"{Regex.Replace(arg, "\"|\\\\(?=\\\\*(\"|$))", "\\$&", RegexOptions.ExplicitCapture)}\"";
        private static string InvokeCommand(string command) => InvokeCommand(command, string.Empty);
        private static string InvokeCommand(string command, string input) {
            if(Console.InputEncoding is UTF8Encoding&&Console.InputEncoding.GetPreamble().Length!=0) {
                Console.InputEncoding=utf8WithoutBom;
            }
            using(Process process = Process.Start(new ProcessStartInfo("cmd", $"/s /c \"{command}\"") {
                UseShellExecute=false,
                RedirectStandardInput=true,
                RedirectStandardOutput=true
            })) {
                string result = WriteRead(input, process.StandardInput.BaseStream, process.StandardOutput.BaseStream).Trim();
                process.WaitForExit();
                if(process.ExitCode!=0) {
                    throw new ExitCodeException(command, process.ExitCode, result);
                } else {
                    return result;
                }
            }
        }
        private static string WriteRead(string inputData, Stream inputStream, Stream outputStream) {
            using(StreamReader streamReader = new StreamReader(outputStream, utf8WithoutBom)) {
                Task<string> readTask = streamReader.ReadToEndAsync();
                using(StreamWriter streamWriter = new StreamWriter(inputStream, utf8WithoutBom)) {
                    streamWriter.Write(inputData);
                }
                return readTask.Result;
            }
        }
    }
}
