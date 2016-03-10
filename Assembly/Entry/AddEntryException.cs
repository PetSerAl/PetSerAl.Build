using System;
namespace PetSerAl.Build {
    internal class AddEntryException : Exception {
        internal AddEntryException(TreeEntry tree, string treePath, Entry entry) : base($"Exception when adding '{treePath}' to tree.") {
            Tree=tree;
            TreePath=treePath;
            Entry=entry;
        }
        public TreeEntry Tree { get; }
        public string TreePath { get; }
        public Entry Entry { get; }
    }
}
