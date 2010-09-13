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
import jetbrains.buildServer.agent.AgentRunningBuild;
import jetbrains.buildServer.agent.artifacts.ArtifactsWatcher;
import jetbrains.buildServer.agent.runner.ProcessListener;
import jetbrains.buildServer.util.FileUtil;
import org.jetbrains.annotations.NotNull;

import java.io.File;

public class WriteToFileProcessListener implements ProcessListener {
    private File outputFile;
    private final ArtifactsWatcher artifactsWatcher;
    private StringBuilder builder;

    public WriteToFileProcessListener(AgentRunningBuild build, ArtifactsWatcher artifactsWatcher) {
        this.artifactsWatcher = artifactsWatcher;
        outputFile = new File(build.getBuildTempDirectory(), PluginConstants.OUTPUT_FILE_NAME);
        builder = new StringBuilder();
    }

    public void onStandardOutput(@NotNull String text) {
        builder.append(text);
    }

    public void onErrorOutput(@NotNull String text) {
        builder.append(text);
    }

    public void processStarted(@NotNull String programCommandLine, @NotNull File workingDirectory) {
        FileUtil.createIfDoesntExist(outputFile);
    }

    public void processFinished(int exitCode) {
        FileUtil.writeFile(outputFile, builder.toString());
        artifactsWatcher.addNewArtifactsPath(outputFile.getPath());
    }
}
