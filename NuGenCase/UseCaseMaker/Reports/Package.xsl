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
	<xsl:param name="actors" />
	<xsl:param name="useCases" />
	<xsl:param name="packages" />
	<xsl:param name="requirements" />
	
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
		
		<xsl:if test="Actors/Actor">
			<xsl:apply-templates select="Actors" />
		</xsl:if>

		<xsl:if test="UseCases/UseCase">
			<xsl:apply-templates select="UseCases" />
		</xsl:if>	

		<xsl:if test="Packages/Package">
			<xsl:apply-templates select="Packages" />
		</xsl:if>
		
		<xsl:if test="Requirements/*">
			<xsl:apply-templates select="Requirements" />
		</xsl:if>		
	</xsl:template>
	
	<xsl:template match="Actors">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$actors" />
				</td>
			</tr>
			<xsl:for-each select="Actor">
			<tr>
				<td class="TableCell">
					<xsl:variable name="uid" select="@UniqueID" />
					<a class="href_default" href="{$uid}.htm" target="mainPage">
						<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
					</a>
				</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
	<xsl:template match="UseCases">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$useCases" />
				</td>
			</tr>
			<xsl:for-each select="UseCase">
			<tr>
				<td class="TableCell">
					<xsl:variable name="uid" select="@UniqueID" />
					<a class="href_default" href="{$uid}.htm" target="mainPage">
						<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
					</a>
				</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
	
	<xsl:template match="Requirements">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" colspan="2">
					<xsl:value-of select="$requirements" />
				</td>
			</tr>
			<xsl:for-each select="Requirement">
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
	
	<xsl:template match="Packages">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$packages" />
				</td>
			</tr>
			<xsl:for-each select="Package">
			<tr>
				<td class="TableCell">
					<xsl:variable name="uid" select="@UniqueID" />
					<a class="href_default" href="{$uid}.htm" target="mainPage">
						<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
					</a>
				</td>
			</tr>
			</xsl:for-each>
		</table>
		<br></br>
		<hr width="600px"></hr>
	</xsl:template>
</xsl:stylesheet>