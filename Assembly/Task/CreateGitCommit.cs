using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
namespace PetSerAl.Build {
    public class CreateGitCommit : Task {
        public CreateGitCommit() { }
        [Required]
        public string Tree { private get; set; }
        public string[] Parents { private get; set; }
        public string[] Message { private get; set; }
        [Output]
        public string Commit { get; private set; }
        public override bool Execute() {
            try {
                Commit=Exec.CommitTree(Tree, Parents??new string[0], Message??new string[0]);
                return true;
            } catch(ExitCodeException exception) {
                Log.LogErrorFromException(exception);
                return false;
            }
        }
    }
}
