using System;
namespace PetSerAl.Build {
    internal class ExitCodeException : Exception {
        internal ExitCodeException(string command, int exitCode, string result) : base($"'{command}' terminated with non-zero exit code: {exitCode}.") {
            Command=command;
            ExitCode=exitCode;
            Result=result;
        }
        public string Command { get; }
        public int ExitCode { get; }
        public string Result { get; }
    }
}
