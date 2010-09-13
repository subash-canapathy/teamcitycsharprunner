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
import jetbrains.buildServer.serverSide.PropertiesProcessor;
import jetbrains.buildServer.serverSide.RunType;
import jetbrains.buildServer.serverSide.RunTypeRegistry;
import org.jetbrains.annotations.NotNull;

import java.util.Map;

/**
 * Created by IntelliJ IDEA.
 * User: Simone
 * Date: 11-set-2010
 * Time: 15.52.21
 * To change this template use File | Settings | File Templates.
 */
public class CSharpRunnerRunType extends RunType {
    public CSharpRunnerRunType(final RunTypeRegistry registry) {
        registry.registerRunType(this);
    }

    @Override
    public String getType() {
        return PluginConstants.RUN_TYPE;
    }

    @Override
    @NotNull
    public String getDisplayName() {
        return PluginConstants.RUNNER_DISPLAY_NAME;
    }

    @Override
    public String getDescription() {
        return PluginConstants.RUNNER_DESCRIPTION;
    }

    @Override
    public PropertiesProcessor getRunnerPropertiesProcessor() {
        return null;
    }

    @Override
    public String getEditRunnerParamsJspFilePath() {
        return "editRunnerRunParameters.jsp";
    }

    @Override
    public String getViewRunnerParamsJspFilePath() {
        return "viewRunnerRunParameters.jsp";
    }

    @Override
    public Map<String, String> getDefaultRunnerProperties() {
        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }
}
