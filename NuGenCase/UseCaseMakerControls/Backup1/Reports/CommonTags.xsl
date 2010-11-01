<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="MatchLink.xsl" />
	<xsl:output method="html" />

	<xsl:template name="CommonTags">
		<xsl:param name="elementType" />
		<xsl:param name="description" />
		<xsl:param name="notes" />
		<xsl:param name="relatedDocs" />
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$elementType" />
				</td>
			</tr>
			<tr>
				<td class="TableCell" align="center">
					<xsl:variable name="uid" select="@UniqueID" />
					<a class="Title" name="{$uid}">
						<xsl:value-of select="@Name" />
						(<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
					</a>
				</td>
			</tr>
		</table>
		<br></br>
		<xsl:if test="Attributes/Description/text() != ''">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$description" />
				</td>
			</tr>
			<tr>
				<td class="TableCell">
					<xsl:call-template name="MatchLink">
						<xsl:with-param name="text" select="Attributes/Description/text()"/>
					</xsl:call-template>
				</td>
			</tr>
		</table>
		<br></br>
		</xsl:if>
		<xsl:if test="Attributes/Notes/text() != ''">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$notes" />
				</td>
			</tr>
			<tr>
				<td class="TableCell">
					<xsl:call-template name="MatchLink">
						<xsl:with-param name="text" select="Attributes/Notes/text()"/>
					</xsl:call-template>
				</td>
			</tr>
		</table>
		<br></br>
		</xsl:if>
		<xsl:if test="Attributes/RelatedDocuments/RelatedDocument">
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell">
					<xsl:value-of select="$relatedDocs" />
				</td>
			</tr>
			<xsl:call-template name="RelatedDocument" />
		</table>
		<br></br>
		</xsl:if>
		<hr width="600px"></hr>
	</xsl:template>

	<xsl:template name="RelatedDocument">
		<xsl:for-each select="Attributes/RelatedDocuments/RelatedDocument">
		<tr>
			<td class="TableCell">
				<xsl:variable name="filelink" select="FileName/text()" />
				<a class="href_default" href="file:///{$filelink}" target="_new">
					<xsl:value-of select="$filelink" />
				</a>
			</td>
		</tr>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>