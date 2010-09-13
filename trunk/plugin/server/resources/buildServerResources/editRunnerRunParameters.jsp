<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<%@ taglib prefix="l" tagdir="/WEB-INF/tags/layout" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="forms" tagdir="/WEB-INF/tags/forms" %>

<jsp:useBean id="propertiesBean" scope="request" type="jetbrains.buildServer.controllers.BasePropertiesBean"/>

<forms:workingDirectory />
<tr>
    <th>
        <label for="script.type">Type: </label>
    </th>
    <td>
        <props:selectProperty name="script.type">
            <props:option value="auto">&lt;Auto&gt;</props:option>
            <props:option value="expression">Expression</props:option>
            <props:option value="statement">Statement</props:option>
            <props:option value="program">Program</props:option>
        </props:selectProperty>
    </td>
</tr>
<tr>
    <th>
        <label for="script.content">C# code: </label> <l:star/>
    </th>
    <td>
        <c:set var="onkeydown">
        (function(e) {
            if(window.event) // IE {
                keynum = e.keyCode
            }
            else if(e.which) {
                keynum = e.which
            }

            if(keynum == 9 /* TAB */) {
                $('script.content').value = $('script.content').value + '    ';
                return false;
            }

            return true;
        })
        </c:set>
        <props:multilineProperty name="script.content" onkeydown="return ${onkeydown}(event)" rows="10" cols="58" expanded="true" linkTitle="Type the C# code" />
        <span class="error" id="error_script.content"></span>
        <span class="smallNote">Enter C# code</span>
    </td>
</tr>

