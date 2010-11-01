<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="lf2br.xsl" />
	<xsl:import href="MatchLink.xsl" />
	<xsl:output method="html" />
	
	<xsl:param name="elementUniqueID" />
	<xsl:param name="glossary" />
	<xsl:param name="glossaryItem" />
	<xsl:param name="description" />
		
	<xsl:template match="/">
		<html>
			<head>
				<link rel="stylesheet" type="text/css" href="ucm.css" />
			</head>
			<body leftmargin="0">
				<xsl:apply-templates mode="page" select="//*[@UniqueID = $elementUniqueID]" />
			</body>
		</html>	
	</xsl:template>
	
	<xsl:template mode="page" match="*">
		<h1 class="Title"><xsl:value-of select="$glossary" /></h1>
		<table align="center" class="Table" cellpadding="2" cellspacing="0" width="600px">
			<tr>
				<td class="HeaderTableCell" width="30%">
					<xsl:value-of select="$glossaryItem" />
				</td>
				<td class="HeaderTableCell">
					<xsl:call-template name="lf2br">
						<xsl:with-param name="text" select="$description" />
					</xsl:call-template>
				</td>
			</tr>
			<xsl:apply-templates select="GlossaryItem" />
		</table>
		<br></br>
		<hr width="600px"></hr>		
	</xsl:template>
	
	<xsl:template match="GlossaryItem">
		<tr>
			<td class="TableCell" width="30%">
				<xsl:variable name="uid" select="@UniqueID" />
				<a name="{$uid}"><xsl:value-of select="@Name" /></a>
			</td>
			<td class="TableCell">
				<xsl:call-template name="MatchLink">
					<xsl:with-param name="text" select="Description"/>
				</xsl:call-template>
			</td>			
		</tr>
	</xsl:template>
</xsl:stylesheet>

  