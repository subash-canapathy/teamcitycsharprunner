<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<%@ taglib prefix="l" tagdir="/WEB-INF/tags/layout" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="forms" tagdir="/WEB-INF/tags/forms" %>

<jsp:useBean id="propertiesBean" scope="request" type="jetbrains.buildServer.controllers.BasePropertiesBean"/>

<tr>
    <th>
        <label for="script.namespaces">Additional namespaces: </label>
    </th>
    <td>
        <props:multilineProperty name="script.namespaces" rows="3" cols="30" expanded="${not empty propertiesBean.properties['script.namespaces']}" linkTitle="Enter additional namespaces" />
        <span class="error" id="error_script.namespaces"></span>
        <span class="smallNote">Enter additional namespaces, one per line</span>
    </td>
</tr>
<tr>
    <th>
        <label for="script.references">Additional assembly references: </label>
    </th>
    <td>
        <props:multilineProperty name="script.references" rows="3" cols="30" expanded="${not empty propertiesBean.properties['script.references']}" linkTitle="Enter additional assembly references" />
        <span class="error" id="error_script.references"></span>
        <span class="smallNote">Enter additional assembly references, one per line</span>
    </td>
</tr>
<tr>
    <th>
        <label for="script.content">C# code: </label> <l:star/>
    </th>
    <td>
        <c:set var="onkeydown">
        (function(e) {
            if(window.event) { // IE
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

