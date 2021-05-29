declare interface MicrosoftVisualBasicFileIO {
    FileSystem: {
        GetFiles(path: string): System.Collections.ObjectModel.ReadOnlyCollection<string>
    }
}
