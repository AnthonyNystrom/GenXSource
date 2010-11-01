<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:param name="modelBrowser" />
	<xsl:param name="glossary" />
	<xsl:output method="html" />
	
	<xsl:template match="/">
		<html>
			<head>
				<link rel="stylesheet" type="text/css" href="ucm.css" />
			</head>
			<body leftmargin="0">
				<xsl:apply-templates select="//Model" />
			</body>
		</html>
	</xsl:template>
	<xsl:template match="Model">
		<table class="Table">
			<tr>
				<td class="TransparentTableCell" align="center">
					<p class="SubTitle">
					<xsl:value-of select="$modelBrowser" />
					</p>
				</td>
			</tr>
			<tr>
				<td class="TransparentTableCell" align="center">
					<xsl:variable name="glossary_uid" select="Glossary/@UniqueID" />
					<a class="href_default" href="{$glossary_uid}.htm" target="mainPage">
						<xsl:value-of select="$glossary" />
					</a>
					<hr />
				</td>
			</tr>
			<tr>
				<td class="TransparentTableCell" nowrap="1">
					<ul>
						<li class="LIPackage">
							<xsl:variable name="uid" select="@UniqueID" />
							<a class="href_default" href="{$uid}.htm" target="mainPage">
								<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
							</a>
							<xsl:apply-templates select="Actors" />
							<xsl:apply-templates select="UseCases" />
							<xsl:apply-templates select="Packages" />
						</li>
					</ul>
			</td>
			</tr>
		</table>
	</xsl:template>
	
	<xsl:template match="Packages">
		<xsl:apply-templates select="Package" />
	</xsl:template>
	
	<xsl:template match="Actors">
		<xsl:apply-templates select="Actor" />
	</xsl:template>
	
	<xsl:template match="Package">
		<ul>
			<li class="LIPackage">
				<xsl:variable name="uid" select="@UniqueID" />
				<a class="href_default" href="{$uid}.htm" target="mainPage">
					<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
				</a>
				<xsl:apply-templates select="Actors" />
				<xsl:apply-templates select="UseCases" />
				<xsl:apply-templates select="Packages" />				
			</li>
		</ul>
	</xsl:template>

	<xsl:template match="Actor">
		<ul>
			<li class="LIActor">
				<xsl:variable name="uid" select="@UniqueID" />
				<a class="href_default" href="{$uid}.htm" target="mainPage">
					<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
				</a>
			</li>
		</ul>
	</xsl:template>

	<xsl:template match="UseCase">
		<ul>
			<li class="LIUseCase">
				<xsl:variable name="uid" select="@UniqueID" />
				<a class="href_default" href="{$uid}.htm" target="mainPage">
					<xsl:value-of select="@Name" /> (<xsl:value-of select="@Prefix" /><xsl:value-of select="@ID" />)
				</a>
			</li>
		</ul>
	</xsl:template>
</xsl:stylesheet>