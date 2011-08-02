package csharpRunner.server;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.controllers.BuildDataExtensionUtil;
import jetbrains.buildServer.serverSide.SBuild;
import jetbrains.buildServer.serverSide.SBuildServer;
import jetbrains.buildServer.web.openapi.PagePlaces;
import jetbrains.buildServer.web.openapi.PlaceId;
import jetbrains.buildServer.web.openapi.SimplePageExtension;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.servlet.http.HttpServletRequest;
import java.util.Map;


public class CSharpReportFragment extends SimplePageExtension {
    private final SBuildServer server;

    public CSharpReportFragment(@NotNull PagePlaces pagePlaces, @NotNull SBuildServer server) {
        super(pagePlaces, PlaceId.BUILD_RESULTS_FRAGMENT, PluginConstants.RUN_TYPE, "csharpReportFragment.jsp");
        this.server = server;
        register();
    }

    @Override
    public void fillModel(@NotNull Map<String, Object> model, @NotNull HttpServletRequest request) {
        model.put("build", getBuild(request));
        model.put("tabCode", PluginConstants.REPORT_TAB_CODE);
    }

    @Nullable
    protected SBuild getBuild(final HttpServletRequest request) {
        return BuildDataExtensionUtil.retrieveBuild(request, server);
    }

    @Override
    public boolean isAvailable(@NotNull HttpServletRequest request) {
        SBuild build = getBuild(request);

        return build != null && ReportUtils.isReportTabAvailable(build);
    }
}
