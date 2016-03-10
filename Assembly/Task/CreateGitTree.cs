using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
namespace PetSerAl.Build {
    public class CreateGitTree : Task {
        public CreateGitTree() { }
        [Required]
        public ITaskItem[] Files { private get; set; }
        [Output]
        public string Tree { get; private set; }
        public override bool Execute() {
            try {
                TreeEntry root = new TreeEntry();
                foreach(ITaskItem file in Files) {
                    string treePath = file.GetMetadata("TargetPath");
                    bool isGitObject;
                    bool.TryParse(file.GetMetadata("IsGitObject"), out isGitObject);
                    root.AddEntry(treePath, isGitObject ? (Entry)new ObjectEntry(file.ItemSpec) : new BlobEntry(treePath, file.ItemSpec));
                }
                Tree=root.Hash;
                return true;
            } catch(AddEntryException exception) {
                Log.LogErrorFromException(exception);
                return false;
            } catch(ExitCodeException exception) {
                Log.LogErrorFromException(exception);
                return false;
            }
        }
    }
}
