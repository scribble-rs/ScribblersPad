using System.Diagnostics;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace ScribblersPadEditor
{
    public class GitCommitHashPreprocessBuildWithReport : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            using Process git_describe_process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "describe --tags --first-parent --abbrev=40 --long --dirty --always",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            if (git_describe_process.Start() && git_describe_process.WaitForExit(3000) && (git_describe_process.ExitCode == 0))
            {
                using StreamWriter git_description_stream_writer = File.CreateText("./Assets/ScribblersPad/Resources/GitDescription.txt");
                string git_description_string = git_describe_process.StandardOutput.ReadToEnd().Trim();
                git_description_stream_writer.Write(git_description_string);
                UnityEngine.Debug.Log($"Git description: { git_description_string }");
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to execute command \"git describe --tags --first-parent --abbrev=40 --long --dirty --always\".");
            }
        }
    }
}
