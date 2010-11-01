<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:import href="lf2br.xsl" />
	<xsl:output method="html" />

<xsl:template name="MatchLink">
    <xsl:param name="text" />
     	<xsl:choose>
    		<xsl:when test="contains($text,'&#34;') and string-length($text) > 1">
				<xsl:variable name="start" select="substring-after($text, '&#34;')" />
				<xsl:variable name="end" select="substring-before($start, '&#34;')" />
				<xsl:choose>
					<xsl:when test="$end">
						<xsl:if test="$start">
							<xsl:call-template name="lf2br">
								<xsl:with-param name="text" select="substring-before($text,$end)" />
							</xsl:call-template>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="//Glossary/*[@Name = $end]">
								<xsl:call-template name="MakeAnchorLink">
									<xsl:with-param name="glossary_uid" select="//Glossary/@UniqueID" />
									<xsl:with-param name="text" select="$end" />
									<xsl:with-param name="node" select="//Glossary/*[@Name = $end]" />
								</xsl:call-template>
								<xsl:call-template name="MatchLink">
									<xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
								</xsl:call-template>							
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="//*[@Path = $end]">
										<xsl:call-template name="MakePathLink">
											<xsl:with-param name="text" select="$end" />
											<xsl:with-param name="node" select="//*[@Path = $end]" />
										</xsl:call-template>
										<xsl:call-template name="MatchLink">
											<xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
										</xsl:call-template>
									</xsl:when>
									<xsl:when test="//*[@Name = $end]">
										<xsl:call-template name="MakeNameLink">
											<xsl:with-param name="text" select="$end" />
											<xsl:with-param name="node" select="//*[@Name = $end]" />
										</xsl:call-template>
										<xsl:call-template name="MatchLink">
											<xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="lf2br">
											<xsl:with-param name="text" select="$end" />
										</xsl:call-template>
										<xsl:call-template name="MatchLink">
											<xsl:with-param name="text" select="substring-after($text,$end)" />
										</xsl:call-template>
									</xsl:otherwise>
								</xsl:choose>							
							</xsl:otherwise>
						</xsl:choose>						
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="MatchLink">
							<xsl:with-param name="text" select="substring-after($text,$start)" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="lf2br">
					<xsl:with-param name="text" select="$text" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
</xsl:template>

<xsl:template name="MakeNameLink">
	<xsl:param name="text" />
	<xsl:param name="node" />
	
	<xsl:variable name="target" select="concat($node/@UniqueID,'.htm')" />
	<a class="href_elementname" href="{$target}">
	<xsl:value-of select="$text" />
	</a>
	<xsl:value-of select="'&#34;'" />
</xsl:template>

<xsl:template name="MakePathLink">
	<xsl:param name="text" />
	<xsl:param name="node" />
	
	<xsl:variable name="target" select="concat($node/@UniqueID,'.htm')" />
	<a class="href_elementpath" href="{$target}">
	<xsl:value-of select="$text" />
	</a>
	<xsl:value-of select="'&#34;'" />
</xsl:template>

<xsl:template name="MakeAnchorLink">
	<xsl:param name="glossary_uid" />
	<xsl:param name="text" />
	<xsl:param name="node" />
	
	<xsl:variable name="target" select="concat($glossary_uid,'.htm','#',$node/@UniqueID)" />
	<a class="href_glossaryitem" href="{$target}">
	<xsl:value-of select="$text" />
	</a>
	<xsl:value-of select="'&#34;'" />
</xsl:template>

</xsl:stylesheet>