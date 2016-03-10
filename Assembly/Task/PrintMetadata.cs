using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
namespace PetSerAl.Build {
    public class PrintMetadata : Task {
        public PrintMetadata() { }
        [Required]
        public ITaskItem[] Items { private get; set; }
        public override bool Execute() {
            foreach(ITaskItem item in Items) {
                Log.LogMessage("--- '{0}' ---", item.ItemSpec);
                foreach(string name in item.MetadataNames) {
                    Log.LogMessage("{0}: '{1}'", name, item.GetMetadata(name));
                }
            }
            return true;
        }
    }
}
