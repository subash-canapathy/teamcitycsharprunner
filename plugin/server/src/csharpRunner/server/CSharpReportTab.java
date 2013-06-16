/*
 * Copyright 2000-2010 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package csharpRunner.server;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.serverSide.SBuild;
import jetbrains.buildServer.serverSide.SBuildServer;
import jetbrains.buildServer.web.openapi.PagePlaces;
import jetbrains.buildServer.web.openapi.ViewLogTab;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.servlet.http.HttpServletRequest;
import java.io.File;
import java.util.Map;

public class CSharpReportTab extends ViewLogTab {
    private static final String TAB_STARTPAGE = PluginConstants.OUTPUT_PATH.replace(File.separatorChar, '/') + "/" + PluginConstants.OUTPUT_FILE_NAME;

    public CSharpReportTab(PagePlaces pagePlaces, SBuildServer server) {
        super("C#", PluginConstants.REPORT_TAB_CODE, pagePlaces, server);
        setIncludeUrl("/artifactsViewer.jsp");
    }

    @Override
    protected void fillModel(Map model, HttpServletRequest request, @Nullable SBuild build) {
        model.put("basePath", "");
        model.put("startPage", TAB_STARTPAGE);
    }

    @Override
    public boolean isAvailable(@NotNull HttpServletRequest request) {
        final SBuild build = getBuild(request);
        if (build == null) return false;

        final String projectId = build.getProjectId();
        if (projectId == null) return false;

        return ReportUtils.isReportTabAvailable(build);
    }
}
