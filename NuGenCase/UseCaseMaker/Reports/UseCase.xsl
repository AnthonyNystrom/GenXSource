<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="CommonTags.xsl" />
	<xsl:import href="MatchLink.xsl" />
 	<xsl:output method="html" />
	
	<xsl:param name="elementUniqueID" />	
	<xsl:param name="elementType" />
	<xsl:param name="description" />
	<xsl:param name="notes" />
	<xsl:param name="relatedDocs" />
	<xsl:param name="flowOfEvents" />
	<xsl:param name="openIssues" />	
	<xsl:param name="prose" />
	<xsl:param name="details" />
	<xsl:param name="priority" />
	<xsl:param name="statusNodeSet" />
	<xsl:param name="status" />
	<xsl:param name="levelNodeSet" />
	<xsl:param name="level" />
	<xsl:param name="complexityNodeSet" />
	<xsl:param name="complexity" />
	<xsl:param name="implementationNodeSet" />
	<xsl:param name="implementation" />
	<xsl:param name="assignedTo" />	
	<xsl:param name="release" />
	<xsl:param name="preconditions" />
	<xsl:param name="postconditions" />	
	<xsl:param name="activeActors" />
	<xsl:param name="primary" />
	<xsl:param name="history" />	
	<xsl:param name="historyTypeNodeSet" />
	
	<xsl:template match="/">
		<html>
			<head>
				<link rel="stylesheet" type="text/css" href="ucm.css" />
			</head>
			<body>
				<xsl:apply-templates mode="page" select="//*[@UniqueID = $elementUniqueID]" />
			</body>
		</html>
	</xsl:template>

	<xsl:template mode="page" match="*">
		<xsl:call-template name="CommonTags">
				<xsl:with-param name="elementType">$elementType</xsl:with-param>
				<xsl:with-param name="description">$description</xsl:with-param>
				<xsl:with-param name="notes">$notes</xsl:with-param>
				<xsl:with-param name="relatedDocs">$relatedDocs</xsl:with-param>
		</xsl:call-template>

		<xsl:call-template name="General" />
		
		<xsl:call-template name="Details" />

		<xsl:if test="OpenIssues/*">
			<xsl:apply-templates select="OpenIssues" />
		</xsl:if>

		<xsl:if test="Steps/*">
			<xsl:apply-templates select="Steps" />
		</xsl:if>
		
		<xsl:if test="Prose/text() != ''">
			<xsl:apply-templates select="Prose" />
		</xsl:if>
		
		<xsl:if test="HistoryItems/*">
			<xsl:apply-templates select="HistoryItems" />
		</xsl:if>
		
	</xsl:template>
	
	<xsl:template name="General">
		<xsl:if test="Preconditions/text() != ''">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
		<tr>
			<td class="HeaderTableCell" colspan="2">
				<xsl:value-of select="$preconditions" />
			</td>
			<xsl:apply-templates select="Preconditions" />
		</tr>
		</table>
		<br></br>
		<hr width="600px"></hr>
		</xsl:if>
		<xsl:if test="Postconditions/text() != ''">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
		<tr>
			<td class="HeaderTableCell" colspan="2">
				<xsl:value-of select="$postconditions" />
			</td>
			<xsl:apply-templates select="Postconditions" />
		</tr>
		</table>
		<br></br>
		<hr width="600px"></hr>
		</xsl:if>
		<xsl:if test="ActiveActors/*">
			<xsl:apply-templates select="ActiveActors" />
		</xsl:if>
	</xsl:template>

	<xsl:template match="ActiveActors">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$activeActors" />
				</td>
			</tr>
			<xsl:for-each select="ActiveActor">
			<tr>
				<td class="TableCell" colspan="2">
					<xsl:variable name="target" select="ActorUniqueID/text()" />
					<xsl:value-of select="//Actor[@UniqueID = $target]/@Name" />
					<xsl:if test="IsPrimary/text() = 'True'">
						(<xsl:value-of select="$primary"/>)
					</xsl:if>
				</td>						
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
	<xsl:template name="Details">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
		<tr>
			<td class="HeaderTableCell" colspan="2">
				<xsl:value-of select="$details" />
			</td>
		</tr>
		<xsl:apply-templates select="Priority" />
		<xsl:apply-templates select="Level" />
		<xsl:apply-templates select="Complexity" />
		<xsl:apply-templates select="Status" />
		<xsl:apply-templates select="Implementation" />
		<xsl:if test="AssignedTo/text() != ''">
			<xsl:apply-templates select="AssignedTo" />
		</xsl:if>
		<xsl:if test="Release/text() != ''">
			<xsl:apply-templates select="Release" />
		</xsl:if>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
	<xsl:template match="Preconditions">
		<tr>
			<td class="TableCell">
				<xsl:call-template name="MatchLink">
					<xsl:with-param name="text" select="text()"/>
				</xsl:call-template>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="Postconditions">
		<tr>
			<td class="TableCell">
				<xsl:call-template name="MatchLink">
					<xsl:with-param name="text" select="text()"/>
				</xsl:call-template>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="Priority">
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$priority" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="text()" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="Level">
		<xsl:variable name="target" select="text()" />
		<xsl:variable name="value" select="$levelNodeSet[@EnumName = $target]" />
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$level" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="$value" />
			</td>
		</tr>
	</xsl:template>
	
	<xsl:template match="Complexity">
		<xsl:variable name="target" select="text()" />
		<xsl:variable name="value" select="$complexityNodeSet[@EnumName = $target]" />
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$complexity" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="$value" />
			</td>
		</tr>
	</xsl:template>
	
	<xsl:template match="Status">
		<xsl:variable name="target" select="text()" />
		<xsl:variable name="value" select="$statusNodeSet[@EnumName = $target]" />
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$status" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="$value" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="Implementation">
		<xsl:variable name="target" select="text()" />
		<xsl:variable name="value" select="$implementationNodeSet[@EnumName = $target]" />
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$implementation" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="$value" />
			</td>
		</tr>
	</xsl:template>
	
	<xsl:template match="AssignedTo">
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$assignedTo" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="text()" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="Release">
		<tr>
			<td class="TableCell" width="30%">
				<xsl:value-of select="$release" />
			</td>
			<td class="TableCell">
				<xsl:value-of select="text()" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="OpenIssues">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" colspan="2">
					<xsl:value-of select="$openIssues" />
				</td>
			</tr>
			<xsl:for-each select="OpenIssue">
			<tr>
			<td class="TableCell" width="100px" valign="top">
				<xsl:value-of select="Name"/>
			</td>
			<td class="TableCell">
				<xsl:call-template name="MatchLink">
					<xsl:with-param name="text" select="Description"/>
				</xsl:call-template>
			</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>

	<xsl:template match="Steps">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" colspan="2">
					<xsl:value-of select="$flowOfEvents" />
				</td>
			</tr>
			<xsl:for-each select="Step">
			<tr>
			<td class="TableCell" width="100px" valign="top">
				<xsl:value-of select="Name"/>
			</td>
			<td class="TableCell">
				<xsl:call-template name="MatchLink">
					<xsl:with-param name="text" select="Description"/>
				</xsl:call-template>
			</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>

	<xsl:template match="Prose">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" colspan="2">
					<xsl:value-of select="$prose" />
				</td>
			</tr>
			<tr>
				<td class="TableCell">
					<xsl:call-template name="MatchLink">
						<xsl:with-param name="text" select="text()"/>
					</xsl:call-template>
				</td>
			</tr>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
	<xsl:template match="HistoryItems">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" colspan="4">
					<xsl:value-of select="$history" />
				</td>
			</tr>
			<xsl:for-each select="HistoryItem">
			<tr>
				<td class="TableCell" width="100px" valign="top">
					<xsl:value-of select="substring-before(DateValue,' ')" />
				</td>
				<td class="TableCell">
					<xsl:variable name="typeTarget" select="Type/text()" />
					<xsl:variable name="typeValue" select="$historyTypeNodeSet[@EnumName = $typeTarget]" />
					<xsl:value-of select="$typeValue" />
				</td>
				<xsl:variable name="actionTarget" select="Action/text()" />
				<xsl:choose>
					<xsl:when test="Type/text() = 'Status'">
						<td class="TableCell">
							<xsl:variable name="statusValue" select="$statusNodeSet[@ListIndex = $actionTarget]" />
							<xsl:value-of select="$statusValue" />
						</td>
					</xsl:when>
					<xsl:otherwise>
						<td class="TableCell">
							<xsl:variable name="implValue" select="$implementationNodeSet[@ListIndex = $actionTarget]" />
							<xsl:value-of select="$implValue" />
						</td>
					</xsl:otherwise>
				</xsl:choose>
				<td class="TableCell">
					<xsl:value-of select="Notes" />
				</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
</xsl:stylesheet>