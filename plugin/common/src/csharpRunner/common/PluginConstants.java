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

package csharpRunner.common;

import org.jetbrains.annotations.NonNls;

/**
 * Created by IntelliJ IDEA.
 * User: Simone
 * Date: 11-set-2010
 * Time: 15.54.45
 * To change this template use File | Settings | File Templates.
 */
public interface PluginConstants {
    @NonNls String RUN_TYPE = "csharpRunner";
    @NonNls String RUNNER_DISPLAY_NAME = "C#";
    String RUNNER_DESCRIPTION = "Runner for executing snippets of C# code";
    String OUTPUT_FILE_NAME = "CSharpOutput.html";

    String PROPERTY_SCRIPT_CONTENT = "script.content";
    String PROPERTY_SCRIPT_NAMESPACES = "script.namespaces";
    String PROPERTY_SCRIPT_REFERENCES = "script.references";
    String EXTERNAL_RUNNER_NAME = "CSharpCompiler.exe";
}
