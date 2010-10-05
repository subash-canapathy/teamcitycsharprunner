package csharpRunner.server;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.serverSide.SBuild;
import jetbrains.buildServer.serverSide.artifacts.ArtifactsInfo;

public final class ReportUtils {
    static boolean isReportTabAvailable(SBuild build) {
        final ArtifactsInfo info = new ArtifactsInfo(build);
        return info.getSize(PluginConstants.OUTPUT_FILE_NAME) >= 0;
    }
}
