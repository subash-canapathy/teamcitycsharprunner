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

package csharpRunner.agent;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.agent.AgentBuildRunnerInfo;
import jetbrains.buildServer.agent.BuildAgentConfiguration;
import jetbrains.buildServer.agent.artifacts.ArtifactsWatcher;
import jetbrains.buildServer.agent.runner.CommandLineBuildService;
import jetbrains.buildServer.agent.runner.CommandLineBuildServiceFactory;
import org.jetbrains.annotations.NotNull;

/**
 * Created by IntelliJ IDEA.
 * User: Simone
 * Date: 11-set-2010
 * Time: 16.12.16
 * To change this template use File | Settings | File Templates.
 */
public class CSharpRunnerBuildServiceFactory implements CommandLineBuildServiceFactory {
    private final ArtifactsWatcher artifactsWatcher;

    public CSharpRunnerBuildServiceFactory(ArtifactsWatcher artifactsWatcher) {
        this.artifactsWatcher = artifactsWatcher;
    }

    @NotNull
    public CommandLineBuildService createService() {
        return new CSharpRunnerBuildService(artifactsWatcher);
    }

    @NotNull
    public AgentBuildRunnerInfo getBuildRunnerInfo() {
        return new AgentBuildRunnerInfo() {
            @NotNull
            public String getType() {
                return PluginConstants.RUN_TYPE;
            }

            public boolean canRun(@NotNull BuildAgentConfiguration agentConfiguration) {
                return agentConfiguration.getSystemInfo().isWindows();
            }
        };
    }
}
