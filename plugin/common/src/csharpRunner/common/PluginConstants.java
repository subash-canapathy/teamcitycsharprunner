package csharpRunner.common;

import org.jetbrains.annotations.NonNls;

import java.io.File;

public interface PluginConstants {
    @NonNls String RUN_TYPE = "csharpRunner";
    @NonNls String RUNNER_DISPLAY_NAME = "C#";
    String RUNNER_DESCRIPTION = "Runner for executing snippets of C# code";
    String OUTPUT_PATH = ".teamcity" + File.separator + "csharpRunner";
    String OUTPUT_FILE_NAME = "CSharpOutput.html";

    String PROPERTY_SCRIPT_CONTENT = "script.content";
    String PROPERTY_SCRIPT_NAMESPACES = "script.namespaces";
    String PROPERTY_SCRIPT_REFERENCES = "script.references";
    String EXTERNAL_RUNNER_NAME = "CSharpCompiler.exe";
    String REPORT_TAB_CODE = "csharpReportTab";
}
