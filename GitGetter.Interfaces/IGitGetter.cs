using System;
using System.Collections.Generic;

namespace GitGetter.Interfaces
{
    /// <summary>
    /// Interface for Git Getter that can provide info on remote git branches + their interrelationships + their last commit dates.
    /// </summary>
    public interface IGitGetter
    {
        /// <summary>
        /// Get last commit dates + names of remote branches for the specified git project, sorted by Commit Date (from newest to oldest). Report any errors using the specified reporter.
        /// </summary>
        /// <param name="projectPath">The checkout path of the git project.</param>
        /// <param name="reporter">The IErrorReporter instance to report errors to.</param>
        /// <returns>Array of tuples (DateTime date, string branch) sorted by date in descending order (newest to oldest).</returns>
        (DateTime date, string branch)[] BranchesByLastCommitDate(string projectPath, IErrorReporter reporter);

        /// <summary>
        /// Determine which of the presented 'branches' have been fully merged into 'branch' for the specified git project. Report any errors using the specified reporter.
        /// </summary>
        /// <param name="branch">The remote branch to report on (value should start with "/origin/").</param>
        /// <param name="branches">Remote branches that will be compared with 'branch' (values should all start with "/origin/").</param>
        /// <param name="projectPath">The checkout path of the git project.</param>
        /// <param name="reporter">The IErrorReporter instance to report errors to.</param>
        /// <returns>The subset of 'branches' for which "has been fully merged into 'branch'" is true (in the same order as 'branches').</returns>
        string[] MergedBranches(string branch, IEnumerable<string> branches, string projectPath, IErrorReporter reporter);

        /// <summary>
        /// Determine which of the presented 'branches' have not been fully merged into 'branch' for the specified git project. Report any errors using the specified reporter.
        /// </summary>
        /// <param name="branch">The remote branch to report on (value should start with "/origin/").</param>
        /// <param name="branches">Remote branches that will be compared with 'branch' (values should all start with "/origin/").</param>
        /// <param name="projectPath">The checkout path of the git project.</param>
        /// <param name="reporter">The IErrorReporter instance to report errors to.</param>
        /// <returns>The subset of 'branches' for which "has not been fully merged into 'branch'" is true (in the same order as 'branches').</returns>
        string[] NotMergedBranches(string branch, IEnumerable<string> branches, string projectPath, IErrorReporter reporter);

        /// <summary>
        /// Update cached data for the specified git project by connecting with the remote server, as configured in the git config. Report any errors using the specified reporter.
        /// </summary>
        /// <param name="projectPath">The checkout path of the git project.</param>
        /// <param name="reporter">The IErrorReporter instance to report errors to.</param>
        void Refresh(string projectPath, IErrorReporter reporter);

        /// <summary>
        /// Get list of remote branches for the specified git project. Report any errors using the specified reporter.
        /// </summary>
        /// <param name="projectPath">The checkout path of the git project.</param>
        /// <param name="reporter">The IErrorReporter instance to report errors to.</param>
        /// <returns>Array of remote branch names (all starting with "/origin/").</returns>
        string[] RemoteBranches(string projectPath, IErrorReporter reporter);
    }
}