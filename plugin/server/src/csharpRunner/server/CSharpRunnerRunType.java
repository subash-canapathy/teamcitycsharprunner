package csharpRunner.server;

import csharpRunner.common.PluginConstants;
import jetbrains.buildServer.serverSide.InvalidProperty;
import jetbrains.buildServer.serverSide.PropertiesProcessor;
import jetbrains.buildServer.serverSide.RunType;
import jetbrains.buildServer.serverSide.RunTypeRegistry;
import org.jetbrains.annotations.NotNull;

import java.util.Collection;
import java.util.Collections;
import java.util.Map;
import java.util.Set;

public class CSharpRunnerRunType extends RunType {
    public CSharpRunnerRunType(final RunTypeRegistry registry) {
        registry.registerRunType(this);
    }

    @NotNull
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
        return new PropertiesProcessor() {
            public Collection<InvalidProperty> process(Map<String, String> properties) {
                if(noScriptContent(properties))
                    return invalidScriptContentProperty();

                return Collections.emptySet();
            }

            private Set<InvalidProperty> invalidScriptContentProperty() {
                return Collections.singleton(new InvalidProperty(PluginConstants.PROPERTY_SCRIPT_CONTENT,
                        "Please insert some C# code to run"));
            }

            private boolean noScriptContent(Map<String, String> properties) {
                return !properties.containsKey(PluginConstants.PROPERTY_SCRIPT_CONTENT) ||
                    properties.get(PluginConstants.PROPERTY_SCRIPT_CONTENT).isEmpty();
            }
        };
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
        return null;
    }
}
