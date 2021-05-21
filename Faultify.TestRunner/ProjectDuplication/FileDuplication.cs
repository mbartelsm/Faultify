﻿using System;
using System.IO;

namespace Faultify.TestRunner.ProjectDuplication
{
    /// <summary>
    ///     Wrapper over duplicated testproject files.
    ///     TODO: Implement memory mapped files.
    /// </summary>
    public class FileDuplication : IDisposable
    {
        private FileStream? _fileStream;

        public FileDuplication(string directory, string name)
        {
            Directory = directory;
            Name = name;
        }

        /// <summary>
        ///     Name of the file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Directory in which the file is located.
        /// </summary>
        public string Directory { get; set; }

        public void Dispose()
        {
            _fileStream?.Close();
            _fileStream = null;
        }

        /// <summary>
        ///     Retrieves the full file path.
        /// </summary>
        public string FullFilePath => Path.Combine(Directory, Name);

        /// <summary>
        ///     Returns whether write mode for the file stream is enabled.
        /// </summary>
        private bool IsWriteModeEnabled => _fileStream?.CanWrite ?? false;

        /// <summary>
        ///     Returns whether read mode for the file stream is enabled.
        /// </summary>
        private bool IsReadModeEnabled => _fileStream?.CanRead ?? false;

        /// <summary>
        ///     Opens up a write access to the file and returns the stream.
        /// </summary>
        /// <returns></returns>
        public Stream OpenReadWriteStream()
        {
            if (_fileStream == null || IsReadModeEnabled) EnableReadWriteOnly();
            return _fileStream!;
        }


        /// <summary>
        ///     Opens up a read access to the file and returns the stream.
        /// </summary>
        /// <returns></returns>
        public Stream OpenReadStream()
        {
            if (_fileStream == null || IsWriteModeEnabled) EnableReadOnly();
            return _fileStream!;
        }

        /// <summary>
        ///     Enables write modes and closes any earlier initialized streams.
        /// </summary>
        private void EnableReadWriteOnly()
        {
            Dispose();

            _fileStream = new FileStream(
                path: FullFilePath,
                mode: FileMode.Open,
                access: FileAccess.ReadWrite,
                share: FileShare.ReadWrite);
        }

        /// <summary>
        ///     Enables read modes and closes any earlier initialized streams.
        /// </summary>
        private void EnableReadOnly()
        {
            Dispose();

            _fileStream = new FileStream(
                path: FullFilePath,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.ReadWrite);
        }
    }
}
