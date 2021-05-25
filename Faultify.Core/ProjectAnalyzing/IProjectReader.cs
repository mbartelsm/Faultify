﻿using System;
using System.Threading.Tasks;

namespace Faultify.Core.ProjectAnalyzing
{
    /// <summary>
    ///     Reading the project in an Asyncronous manner
    /// </summary>
    public interface IProjectReader
    {
        Task<IProjectInfo> ReadProjectAsync(string path, IProgress<string> progress);
    }
}
