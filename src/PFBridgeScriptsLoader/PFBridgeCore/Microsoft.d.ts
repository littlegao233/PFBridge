declare interface Microsoft {
    VisualBasic: {
        FileIO: {
            FileSystem: {
                GetFiles(path: string):   System.Collections.ObjectModel.ReadOnlyCollection<string>
            }
        }
    }
}
