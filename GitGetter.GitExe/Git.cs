using GitGetter.GitExe.Helpers;
using GitGetter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitGetter.GitExe
{
    /// <summary>
    /// Implementation of IGitGetter that uses your globally installed "git for Windows" executable to get branch data for the git project.
    /// </summary>
    public class Git : IGitGetter
    {
        /// <summary>
        /// Get list of remote branches. Result is output from running "git branch -r", slightly sanitized (trimmed) and without origin/HEAD because that is not an actual branch.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="reporter"></param>
        /// <returns></returns>
        public string[] RemoteBranches(string projectPath, IErrorReporter reporter)
        {
            return OS.RunProgram("git.exe", "branch -r", projectPath, reporter)
                .Select(line => line.Trim())
                .Where(line => line.HasValue())
                .Where(line => !line.StartsWith("origin/HEAD "))
                .ToArray();
        }

        /// <summary>
        /// Refresh the local branch-list that git maintains as a cache for itself, by running 'git remote update origin --prune'.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="reporter"></param>
        public void Refresh(string projectPath, IErrorReporter reporter)
        {
            OS.RunProgram("git.exe", "remote update origin --prune", projectPath, reporter);
        }

        /// <summary>
        /// Get a list of all remote branches that have been merged into the remote 'branch'. Result is output from running 'git branch --no-color --remotes --merged origin/the_remote_branch', slightly sanitized (trimmed) and without origin/HEAD (because that is not an actual branch), also leaving out all of 'branches' (you can fill this with branches that are not of interest).
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branches"></param>
        /// <param name="projectPath"></param>
        /// <param name="reporter"></param>
        /// <returns></returns>
        public string[] MergedBranches(string branch, IEnumerable<string> branches, string projectPath, IErrorReporter reporter)
        {
            return OS.RunProgram("git.exe", "branch --no-color --remotes    --merged " + branch, projectPath, reporter)
                .Select(line => line.Trim())
                .Where(line => line.HasValue())
                .Where(line => !line.StartsWith("origin/HEAD "))
                .Where(line => !branches.Contains(line))
                .ToArray();
        }

        /// <summary>
        /// Get a list of all remote branches that have NOT been merged into the remote 'branch'. Result is output from running 'git branch --no-color --remotes --no-merged origin/the_remote_branch', slightly sanitized (trimmed) and without origin/HEAD (because that is not an actual branch), also leaving out all of 'branches' (you can fill this with branches that are not of interest).
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branches"></param>
        /// <param name="projectPath"></param>
        /// <param name="reporter"></param>
        /// <returns></returns>
        public string[] NotMergedBranches(string branch, IEnumerable<string> branches, string projectPath, IErrorReporter reporter)
        {
            return OS.RunProgram("git.exe", "branch --no-color --remotes --no-merged " + branch, projectPath, reporter)
                .Select(line => line.Trim())
                .Where(line => line.HasValue())
                .Where(line => !line.StartsWith("origin/HEAD "))
                .Where(line => !branches.Contains(line))
                .ToArray();
        }

        /// <summary>
        /// Get a list of all remote branches in order of their Last Commit Date (newest to oldest), which are also returned. Result is output from running 'git for-each-ref --sort=-committerdate refs/remotes/ --format="%(committerdate:iso)|%(refname:short)"', slightly sanitized (trimmed) and without origin/HEAD (because that is not an actual branch).
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="reporter"></param>
        /// <returns></returns>
        public (DateTime date, string branch)[] BranchesByLastCommitDate(string projectPath, IErrorReporter reporter)
        {
            // Tell git.exe to give results in the format "yyyy-MM-dd HH:mm:ss|short branchname".
            // Each line is then split and parsed, and the results are delivered as a ValueTuple array.
            return OS.RunProgram("git.exe", "for-each-ref --sort=-committerdate refs/remotes/ --format=\"%(committerdate:iso)|%(refname:short)\"", projectPath, reporter)
                .Select(line => line.Trim())
                .Where(line => line.HasValue())
                .Where(line => !line.EndsWith("|origin/HEAD"))
                .Select(line => DateAndBranch(line))
                .ToArray();
        }

        /// <summary>
        /// (private method) Split line by "|". Parse item[0] as a DateTime. Return a ValueTuple containing the parsed DateTime + item[1] (which is assumed to represent a Branch name).
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private (DateTime date, string branch) DateAndBranch(string line)
        {
            var parts = line.Split('|');
            var date = DateTime.Parse(parts[0], null, System.Globalization.DateTimeStyles.RoundtripKind);
            return (date, parts[1]);
        }
    }
}