package csharpRunner.agent;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.RunBuildException;
import jetbrains.buildServer.agent.AgentRunningBuild;
import jetbrains.buildServer.agent.artifacts.ArtifactsWatcher;
import jetbrains.buildServer.agent.runner.CommandLineBuildService;
import jetbrains.buildServer.agent.runner.ProgramCommandLine;
import jetbrains.buildServer.agent.runner.SimpleProgramCommandLine;
import jetbrains.buildServer.util.StringUtil;
import org.jetbrains.annotations.NotNull;
import sun.misc.BASE64Encoder;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public class CSharpRunnerBuildService extends CommandLineBuildService {
    private final ArtifactsWatcher artifactsWatcher;

    public CSharpRunnerBuildService(ArtifactsWatcher artifactsWatcher) {
        this.artifactsWatcher = artifactsWatcher;
    }

    @NotNull
    @Override
    public ProgramCommandLine makeProgramCommandLine() throws RunBuildException {
        AgentRunningBuild build = getBuild();

        List<String> args = createArgs();

        return new SimpleProgramCommandLine(getRunnerContext(),
                build.getAgentConfiguration().getAgentPluginsDirectory() +  "\\" + PluginConstants.RUN_TYPE + "\\bin\\" + PluginConstants.EXTERNAL_RUNNER_NAME,
                args);
    }

    private List<String> createArgs() {
        Map<String,String> parameters = getBuild().getRunnerParameters();
        List<String> result = new ArrayList<String>();

        BASE64Encoder encoder = new BASE64Encoder();

        String program = null;
        try {
            program = encoder.encode(parameters.get(PluginConstants.PROPERTY_SCRIPT_CONTENT).getBytes("UTF8"));
        } catch (UnsupportedEncodingException e) {
            // are you sure you don't know UTF8?
        }

        result.add(program);

        String namespaces = parameters.get(PluginConstants.PROPERTY_SCRIPT_NAMESPACES);
        result.add(namespaces == null || StringUtil.isEmpty(namespaces) ? ";" : StringUtil.convertLineSeparators(namespaces, ";"));
        String references = parameters.get(PluginConstants.PROPERTY_SCRIPT_REFERENCES);
        result.add(references == null || StringUtil.isEmpty(references) ? ";" : StringUtil.convertLineSeparators(references, ";"));

        return result;
    }
}
