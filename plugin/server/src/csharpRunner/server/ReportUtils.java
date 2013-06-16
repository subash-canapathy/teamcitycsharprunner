package csharpRunner.server;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.serverSide.SBuild;
import jetbrains.buildServer.serverSide.artifacts.ArtifactsInfo;

import java.io.File;

public final class ReportUtils {
    static boolean isReportTabAvailable(SBuild build) {
        final ArtifactsInfo info = new ArtifactsInfo(build);
        return info.getSize(new File(PluginConstants.OUTPUT_PATH, PluginConstants.OUTPUT_FILE_NAME).getPath()) >= 0;
    }
}
