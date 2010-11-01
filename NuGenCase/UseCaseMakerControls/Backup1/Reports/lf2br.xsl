<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" />

<xsl:template name="lf2br">
    <xsl:param name="text" />
    <xsl:choose>
		<xsl:when test="contains($text, '&#10;')">
			<xsl:variable name="head" select="substring-before($text, '&#10;')" />
			<xsl:variable name="tail" select="substring-after($text, '&#10;')" />
			<xsl:value-of select="$head" />
			<br />
			<xsl:if test="$tail">
				<xsl:call-template name="lf2br">
					<xsl:with-param name="text" select="$tail" />
				</xsl:call-template>
			</xsl:if>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$text" />
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

</xsl:stylesheet>