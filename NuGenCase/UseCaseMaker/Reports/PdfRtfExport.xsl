<?xml version="1.0" encoding="utf-8" ?> 
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:output method="xml" version="1.0" encoding="utf-8" standalone="yes"/>

  <!-- Application parameters -->
  <xsl:param name="version"/>
  <xsl:param name="outputType"/>
  <!-- General parameters -->
  <xsl:param name="description"/>
  <xsl:param name="notes"/>
  <xsl:param name="relatedDocs"/>
  <xsl:param name="model"/>
  <xsl:param name="actor"/>
  <xsl:param name="useCase"/>
  <xsl:param name="package"/>
  <xsl:param name="actors"/>
  <xsl:param name="useCases"/>
  <xsl:param name="packages"/>
  <xsl:param name="summary"/>
  <xsl:param name="glossary"/>
  <xsl:param name="glossaryItem"/>
  <!-- 'Actor' specific parameters -->
  <xsl:param name="goals"/>
  <!-- 'Use case' specific paramters -->
  <xsl:param name="preconditions"/>
  <xsl:param name="postconditions"/>
  <xsl:param name="activeActors"/>
  <xsl:param name="primary"/>
  <xsl:param name="details"/>
  <xsl:param name="level"/>
  <xsl:param name="status"/>
  <xsl:param name="complexity"/>
  <xsl:param name="priority"/>
  <xsl:param name="implementation"/>
  <xsl:param name="release"/>
  <xsl:param name="assignedTo"/>
  <xsl:param name="openIssues"/>
  <xsl:param name="flowOfEvents"/>
  <xsl:param name="prose"/>
  <xsl:param name="requirements"/>
  <xsl:param name="history"/>
  <xsl:param name="implementationNodeSet"/>
  <xsl:param name="statusNodeSet"/>
  <xsl:param name="complexityNodeSet"/>
  <xsl:param name="levelNodeSet"/>
  <xsl:param name="historyTypeNodeSet"/>
  
  <!-- Start of Style Attributes -->
  <xsl:attribute-set name="ElementTitleCell">
    <xsl:attribute name="bgred">144</xsl:attribute>
	<xsl:attribute name="bggreen">144</xsl:attribute>
	<xsl:attribute name="bgblue">204</xsl:attribute>  	
    <xsl:attribute name="borderwidth">0.2</xsl:attribute>
    <xsl:attribute name="left">true</xsl:attribute>
    <xsl:attribute name="right">true</xsl:attribute>
    <xsl:attribute name="top">true</xsl:attribute>
    <xsl:attribute name="bottom">true</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="ElementTitle">
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">12</xsl:attribute>
    <xsl:attribute name="fontstyle">bold</xsl:attribute>
	<xsl:attribute name="red">255</xsl:attribute>
	<xsl:attribute name="green">255</xsl:attribute>	
	<xsl:attribute name="blue">255</xsl:attribute>    
  </xsl:attribute-set>
  <xsl:attribute-set name="ElementNameCell">
    <xsl:attribute name="bgred">255</xsl:attribute>
	<xsl:attribute name="bggreen">255</xsl:attribute>
	<xsl:attribute name="bgblue">255</xsl:attribute>
    <xsl:attribute name="borderwidth">0.2</xsl:attribute>
    <xsl:attribute name="left">true</xsl:attribute>
    <xsl:attribute name="right">true</xsl:attribute>
    <xsl:attribute name="top">true</xsl:attribute>
    <xsl:attribute name="bottom">true</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="ElementName">
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">15.0</xsl:attribute>
    <xsl:attribute name="fontstyle">bold</xsl:attribute>
  </xsl:attribute-set>  
  <xsl:attribute-set name="ElementTextCell">
    <xsl:attribute name="borderwidth">0.2</xsl:attribute>
    <xsl:attribute name="left">true</xsl:attribute>
    <xsl:attribute name="right">true</xsl:attribute>
    <xsl:attribute name="top">false</xsl:attribute>
    <xsl:attribute name="bottom">true</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>    
  </xsl:attribute-set>
  <xsl:attribute-set name="ElementText">
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">9.0</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>    
  </xsl:attribute-set>  
  <xsl:attribute-set name="ElementLinkCell">
    <xsl:attribute name="borderwidth">0.2</xsl:attribute>
    <xsl:attribute name="left">true</xsl:attribute>
    <xsl:attribute name="right">true</xsl:attribute>
    <xsl:attribute name="top">false</xsl:attribute>
    <xsl:attribute name="bottom">true</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>    
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">9.0</xsl:attribute>    
  </xsl:attribute-set>
  <xsl:attribute-set name="ElementLink">
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">9.0</xsl:attribute>
    <xsl:attribute name="fontstyle">italic,underline</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>    
  </xsl:attribute-set>
  <xsl:attribute-set name="ParaSep">
  <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="spacingafter">12.0</xsl:attribute>
    <xsl:attribute name="spacingbefore">12.0</xsl:attribute>
    <xsl:attribute name="borderwidth">0.3</xsl:attribute>
    <xsl:attribute name="left">false</xsl:attribute>
    <xsl:attribute name="right">false</xsl:attribute>
    <xsl:attribute name="top">false</xsl:attribute>
    <xsl:attribute name="bottom">true</xsl:attribute>
    <xsl:attribute name="red">128</xsl:attribute>
    <xsl:attribute name="green">128</xsl:attribute>
    <xsl:attribute name="blue">128</xsl:attribute>    
    <xsl:attribute name="indentationleft">5.0</xsl:attribute>
    <xsl:attribute name="indentationright">5.0</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="ParaTitle">
    <xsl:attribute name="align">left</xsl:attribute>
    <xsl:attribute name="font">Helvetica</xsl:attribute>
    <xsl:attribute name="size">15.0</xsl:attribute>
    <xsl:attribute name="fontstyle">bold</xsl:attribute>
    <xsl:attribute name="red">0</xsl:attribute>
    <xsl:attribute name="green">0</xsl:attribute>
    <xsl:attribute name="blue">0</xsl:attribute>    
  </xsl:attribute-set>
  <!-- End of Style Attributes -->
  
  <!-- Start of template -->
  <xsl:template match="/">
    <itext pagesize="A4" orientation="portrait" left="50" top="50" right="50" bottom="50">
      <xsl:apply-templates/>
    </itext>
  </xsl:template>
  
  <!-- Start of Summary section -->
  <!-- Summary section removed in version 0.9.3 -->
  <!--
  <xsl:template name="MakeSummary">
    <paragraph xsl:use-attribute-sets="ParaTitle">
	  <xsl:value-of select="$summary"/>
    </paragraph>
    <newline/>
    <xsl:call-template name="MakeSummaryItem">
      <xsl:with-param name="node" select="//Model"/>
      <xsl:with-param name="indent" select="0"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="MakeSummaryItem">
    <xsl:param name="node"/>
    <xsl:param name="indent"/>
    <xsl:call-template name="FormatSummaryItem">
      <xsl:with-param name="node" select="$node"/>
      <xsl:with-param name="indent" select="$indent"/>
    </xsl:call-template>
    <xsl:for-each select="$node/Actors/Actor">
      <xsl:call-template name="MakeSummaryItem">
        <xsl:with-param name="node" select="."/>
        <xsl:with-param name="indent" select="$indent + 20.0"/>
      </xsl:call-template>
    </xsl:for-each>
    <xsl:for-each select="$node/UseCases/UseCase">
      <xsl:call-template name="MakeSummaryItem">
        <xsl:with-param name="node" select="."/>
        <xsl:with-param name="indent" select="$indent + 20.0"/>
      </xsl:call-template>
    </xsl:for-each>
    <xsl:for-each select="$node/Packages/Package">
      <xsl:call-template name="MakeSummaryItem">
        <xsl:with-param name="node" select="."/>
        <xsl:with-param name="indent" select="$indent + 20.0"/>
      </xsl:call-template>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="FormatSummaryItem">
    <xsl:param name="node"/>
    <xsl:param name="indent"/>
	<xsl:element name="paragraph">
      <xsl:attribute name="indentationleft">
        <xsl:value-of select="$indent" />
      </xsl:attribute>
      <xsl:attribute name="spacingafter">10.0</xsl:attribute>
        <xsl:element name="anchor" use-attribute-sets="ElementLink">
          <xsl:attribute name="reference">
            <xsl:value-of select="concat('#',$node/@UniqueID)"/>
          </xsl:attribute>
          <xsl:value-of select="concat($node/@Name,' (',$node/@Prefix,$node/@ID,') ')"/>
      </xsl:element>
	</xsl:element>
  </xsl:template>
  -->
  <!-- End of Summary section -->   
   
    <!-- Start of Glossary section -->
  <xsl:template match="Glossary">
    <paragraph xsl:use-attribute-sets="ParaTitle">
      <xsl:value-of select="$glossary"/>
    </paragraph>
    <table columns="2" width="100%" widths="33.33333;66.66666" cellpadding="2.0">
      <row>
        <cell xsl:use-attribute-sets="ElementTitleCell">
          <paragraph xsl:use-attribute-sets="ElementTitle">
             <xsl:value-of select="$glossaryItem"/>
          </paragraph>
        </cell>
        <cell xsl:use-attribute-sets="ElementTitleCell">
          <paragraph xsl:use-attribute-sets="ElementTitle">
            <xsl:value-of select="$description"/>
          </paragraph>
        </cell>
      </row>
      <xsl:apply-templates select="GlossaryItem"/>
    </table>
  </xsl:template>
  <xsl:template match="GlossaryItem">
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <xsl:choose>
          <xsl:when test="$outputType = 'withLink'">
            <xsl:element name="anchor" use-attribute-sets="ElementText">
              <xsl:attribute name="name">
                <xsl:value-of select="@UniqueID"/>
              </xsl:attribute>
              <xsl:value-of select="@Name"/>
           </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:value-of select="@Name"/>
            </paragraph>
          </xsl:otherwise>
        </xsl:choose>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:call-template name="MatchLink">
            <xsl:with-param name="text" select="Description"/>
          </xsl:call-template>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- End of Glossary section -->

  <!-- Model -->
  <xsl:template match="Model">
	<!-- Common attributes -->
    <xsl:call-template name="CommonAttributes">
      <xsl:with-param name="node" select="."/>
      <xsl:with-param name="elementType" select="$model"/>
    </xsl:call-template>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
    <!-- Actors list -->
    <xsl:if test="Actors/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$actors"/>
        </xsl:call-template>
        <xsl:for-each select="Actors/Actor">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
          </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Use cases list -->
    <xsl:if test="UseCases/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$useCases"/>
        </xsl:call-template>
        <xsl:for-each select="UseCases/UseCase">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
          </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Packages list -->
    <xsl:if test="Packages/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$packages"/>
        </xsl:call-template>
        <xsl:for-each select="Packages/Package">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
          </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Requirements details -->
    <xsl:if test="Requirements/*">
      <xsl:apply-templates select="Requirements"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>     
    <!-- Actors details -->
    <xsl:apply-templates select="Actors/Actor"/>
    <!-- Use cases details -->
    <xsl:apply-templates select="UseCases/UseCase"/>
    <newpage/>
    <!-- Recurse package items -->
    <xsl:apply-templates select="Packages/Package"/>
    <!-- Glossary -->
    <xsl:apply-templates select="Glossary"/>
    <!-- Summary -->
    <!-- Summary removed in version 0.9.3 -->
    <!--
    <newpage/>
    <xsl:call-template name="MakeSummary"/>
    -->
  </xsl:template>
  
  <xsl:template match="Package">
	<!-- Common attributes -->
    <xsl:call-template name="CommonAttributes">
      <xsl:with-param name="node" select="."/>
      <xsl:with-param name="elementType" select="$package"/>
    </xsl:call-template>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
    <!-- Actors list -->
    <xsl:if test="Actors/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$actors"/>
        </xsl:call-template>
        <xsl:for-each select="Actors/Actor">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
          </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Use cases list -->
    <xsl:if test="UseCases/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$useCases"/>
        </xsl:call-template>
        <xsl:for-each select="UseCases/UseCase">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
        </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Packages list -->
    <xsl:if test="Packages/*">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$packages"/>
        </xsl:call-template>
        <xsl:for-each select="Packages/Package">
          <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(@Name,' (',@Prefix,@ID,')')"/>
            <xsl:with-param name="link" select="@UniqueID"/>
        </xsl:call-template>
        </xsl:for-each>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <!-- Requirements details -->
    <xsl:if test="Requirements/*">
      <xsl:apply-templates select="Requirements"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>    
    <!-- Actors details -->
    <xsl:apply-templates select="Actors/Actor"/>
    <!-- Use cases details -->
    <xsl:apply-templates select="UseCases/UseCase"/>
    <newpage/>
    <xsl:apply-templates select="Packages/Package"/>
  </xsl:template>
  <!-- Actor details -->
  <xsl:template match="Actor">
		<!-- Common attributes -->
    <xsl:call-template name="CommonAttributes">
      <xsl:with-param name="node" select="."/>
      <xsl:with-param name="elementType" select="$actor"/>
    </xsl:call-template>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
    <!-- Actor specific -->
    <xsl:if test="Goals/*">
      <xsl:apply-templates select="Goals"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>    
  </xsl:template>
  <!-- 'Actor' goals -->
  <xsl:template match="Goals">
    <table columns="2" width="100%" cellpadding="2.0" widths="20.0;80.0">
      <xsl:call-template name="HeaderRow">
        <xsl:with-param name="colspan" select="2"/>
        <xsl:with-param name="text" select="$goals"/>
      </xsl:call-template>
      <xsl:for-each select="Goal">
        <row>
        <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:value-of select="Name"/>
            </paragraph>
        </cell>
        <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
            <xsl:call-template name="MatchLink">
                <xsl:with-param name="text" select="Description"/>
            </xsl:call-template>
            </paragraph>
        </cell>
        </row>
      </xsl:for-each>
    </table>
  </xsl:template>  
  <!-- Use case details -->
  <xsl:template match="UseCase">
		<!-- Common attributes -->
    <xsl:call-template name="CommonAttributes">
      <xsl:with-param name="node" select="."/>
      <xsl:with-param name="elementType" select="$useCase"/>
    </xsl:call-template>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
    <!-- Use case specific -->
    <xsl:call-template name="General"/>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
    <xsl:call-template name="Details"/>
    <xsl:if test="OpenIssues/*">
      <xsl:apply-templates select="OpenIssues"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <xsl:if test="Steps/*">
      <xsl:apply-templates select="Steps"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <xsl:if test="Prose/text() != ''">
      <xsl:apply-templates select="Prose"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <xsl:if test="HistoryItems/*">
      <xsl:apply-templates select="HistoryItems"/>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
  </xsl:template>
  <!-- 'Use case' general -->
  <xsl:template name="General">
    <xsl:if test="Preconditions/text()">
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$preconditions"/>
        </xsl:call-template>
        <row>
          <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:call-template name="MatchLink">
                <xsl:with-param name="text" select="Preconditions/text()"/>
              </xsl:call-template>
            </paragraph>
          </cell>
        </row>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <xsl:if test="Postconditions/text()">
      <paragraph spacingafter="12.0"/>
      <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$postconditions"/>
        </xsl:call-template>
        <row>
          <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:call-template name="MatchLink">
                <xsl:with-param name="text" select="Postconditions/text()"/>
              </xsl:call-template>
            </paragraph>
          </cell>
        </row>
      </table>
      <paragraph xsl:use-attribute-sets="ParaSep"/>
    </xsl:if>
    <xsl:if test="ActiveActors/*">
      <xsl:apply-templates select="ActiveActors"/>
    </xsl:if>
  </xsl:template>
  <!-- 'Use case' details -->
  <xsl:template name="Details">
    <table columns="2" width="100%" cellpadding="2.0" widths="33.33333;66.66666">
      <xsl:call-template name="HeaderRow">
        <xsl:with-param name="colspan" select="2"/>
        <xsl:with-param name="text" select="$details"/>
      </xsl:call-template>
      <xsl:apply-templates select="Priority"/>
      <xsl:apply-templates select="Level"/>
      <xsl:apply-templates select="Complexity"/>
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="Implementation"/>
      <xsl:if test="AssignedTo/text() != ''">
        <xsl:apply-templates select="AssignedTo"/>
      </xsl:if>
      <xsl:if test="Release/text() != ''">
        <xsl:apply-templates select="Release"/>
      </xsl:if>
    </table>
    <paragraph xsl:use-attribute-sets="ParaSep"/>
  </xsl:template>
  <!-- 'Use case' priority -->
  <xsl:template match="Priority">
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$priority"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="text()"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' level -->
  <xsl:template match="Level">
    <xsl:variable name="target" select="text()"/>
    <xsl:variable name="value" select="$levelNodeSet[@EnumName = $target]"/>
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$level"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$value"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' complexity -->
  <xsl:template match="Complexity">
    <xsl:variable name="target" select="text()"/>
    <xsl:variable name="value" select="$complexityNodeSet[@EnumName = $target]"/>
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$complexity"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$value"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' status -->
  <xsl:template match="Status">
    <xsl:variable name="target" select="text()"/>
    <xsl:variable name="value" select="$statusNodeSet[@EnumName = $target]"/>
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$status"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$value"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' implementation -->
  <xsl:template match="Implementation">
    <xsl:variable name="target" select="text()"/>
    <xsl:variable name="value" select="$implementationNodeSet[@EnumName = $target]"/>
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$implementation"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$value"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' assigned to -->
  <xsl:template match="AssignedTo">
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$assignedTo"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="text()"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' release -->
  <xsl:template match="Release">
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$release"/>
        </paragraph>
      </cell>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="text()"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <!-- 'Use case' active actors -->
  <xsl:template match="ActiveActors">
      <table columns="1" width="100%" cellpadding="2.0">
      <xsl:call-template name="HeaderRow">
        <xsl:with-param name="text" select="$activeActors"/>
      </xsl:call-template>
      <xsl:for-each select="ActiveActor">
        <xsl:variable name="target" select="ActorUniqueID/text()"/>
        <xsl:choose>
        <xsl:when test="IsPrimary/text() = 'True'">
            <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="concat(//Actor[@UniqueID = $target]/@Name,' (',$primary,')')"/>
            <xsl:with-param name="link" select="$target"/>
            </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
            <xsl:call-template name="IntLinkRow">
            <xsl:with-param name="text" select="//Actor[@UniqueID = $target]/@Name"/>
            <xsl:with-param name="link" select="$target"/>
            </xsl:call-template>
        </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </table>
  </xsl:template>
  <!-- 'Use case' open issues -->
  <xsl:template match="OpenIssues">
    <table columns="2" width="100%" cellpadding="2.0" widths="20.0;80.0">
      <xsl:call-template name="HeaderRow">
        <xsl:with-param name="colspan" select="2"/>
        <xsl:with-param name="text" select="$openIssues"/>
      </xsl:call-template>
      <xsl:for-each select="OpenIssue">
        <row>
        <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:value-of select="Name"/>
            </paragraph>
        </cell>
        <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
            <xsl:call-template name="MatchLink">
                <xsl:with-param name="text" select="Description"/>
            </xsl:call-template>
            </paragraph>
        </cell>
        </row>
      </xsl:for-each>
    </table>
  </xsl:template>
  <!-- 'Use case' flow of events -->
  <xsl:template match="Steps">
    <table columns="2" width="100%" cellpadding="2.0" widths="20.0;80.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="colspan" select="2"/>
          <xsl:with-param name="text" select="$flowOfEvents"/>
        </xsl:call-template>
        <xsl:for-each select="Step">
          <row>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:value-of select="Name"/>
              </paragraph>
            </cell>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:call-template name="MatchLink">
                  <xsl:with-param name="text" select="Description"/>
                </xsl:call-template>
              </paragraph>
            </cell>
          </row>
        </xsl:for-each>
    </table>
  </xsl:template>
  <!-- 'Use case' prose -->
  <xsl:template match="Prose">
    <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$prose"/>
        </xsl:call-template>
        <row>
          <cell xsl:use-attribute-sets="ElementTextCell">
            <paragraph xsl:use-attribute-sets="ElementText">
              <xsl:call-template name="MatchLink">
                <xsl:with-param name="text" select="text()"/>
              </xsl:call-template>
            </paragraph>
          </cell>
        </row>
    </table>
  </xsl:template>
  <!-- 'Use case' requirements -->
  <xsl:template match="Requirements">
    <table columns="2" width="100%" cellpadding="2.0" widths="20.0;80.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="colspan" select="2"/>
          <xsl:with-param name="text" select="$requirements"/>
        </xsl:call-template>
        <xsl:for-each select="Requirement">
          <row>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:value-of select="Name"/>
              </paragraph>
            </cell>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:call-template name="MatchLink">
                  <xsl:with-param name="text" select="Description"/>
                </xsl:call-template>
              </paragraph>
            </cell>
          </row>
        </xsl:for-each>
    </table>
  </xsl:template>
  <!-- 'Use case' history -->
  <xsl:template match="HistoryItems">
    <table columns="4" width="100%" cellpadding="2.0" widths="20.0;20.0;20.0;40.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="colspan" select="4"/>
          <xsl:with-param name="text" select="$history"/>
        </xsl:call-template>
        <xsl:for-each select="HistoryItem">
          <row>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:value-of select="substring-before(DateValue,' ')"/>
              </paragraph>
            </cell>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <xsl:variable name="typeTarget" select="Type/text()"/>
              <xsl:variable name="typeValue" select="$historyTypeNodeSet[@EnumName = $typeTarget]"/>
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:value-of select="$typeValue"/>
              </paragraph>
            </cell>
            <xsl:variable name="actionTarget" select="Action/text()"/>
            <xsl:choose>
              <xsl:when test="Type/text() = 'Status'">
                <cell xsl:use-attribute-sets="ElementTextCell">
                  <xsl:variable name="statusValue" select="$statusNodeSet[@ListIndex = $actionTarget]"/>
                  <paragraph xsl:use-attribute-sets="ElementText">
                    <xsl:value-of select="$statusValue"/>
                  </paragraph>
                </cell>
              </xsl:when>
              <xsl:otherwise>
                <cell xsl:use-attribute-sets="ElementTextCell">
                  <xsl:variable name="implValue" select="$implementationNodeSet[@ListIndex = $actionTarget]"/>
                  <paragraph xsl:use-attribute-sets="ElementText">
                    <xsl:value-of select="$implValue"/>
                  </paragraph>
                </cell>
              </xsl:otherwise>
            </xsl:choose>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:value-of select="Notes"/>
              </paragraph>
            </cell>
          </row>
        </xsl:for-each>
    </table>
  </xsl:template>
  <!-- General common attributes -->
  <xsl:template name="CommonAttributes">
    <xsl:param name="node"/>
    <xsl:param name="elementType"/>
    <table columns="1" width="100%" cellpadding="2.0">
        <xsl:call-template name="HeaderRow">
          <xsl:with-param name="text" select="$elementType"/>
        </xsl:call-template>
        <xsl:call-template name="NameRow">
          <xsl:with-param name="text" select="concat($node/@Name,' (',$node/@Prefix,$node/@ID,')')"/>
          <xsl:with-param name="uniqueID" select="$node/@UniqueID"/>
        </xsl:call-template>
    </table>
    <paragraph spacingafter="12.0"/>
    <xsl:if test="$node/Attributes/Description/text() != ''">
      <table columns="1" width="100%" cellpadding="2.0">
          <xsl:call-template name="HeaderRow">
            <xsl:with-param name="text" select="$description"/>
          </xsl:call-template>
          <row>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:call-template name="MatchLink">
                  <xsl:with-param name="text" select="$node/Attributes/Description/text()"/>
                </xsl:call-template>
              </paragraph>
            </cell>
          </row>                 
      </table>
      <paragraph spacingafter="12.0"/>
    </xsl:if>
    <xsl:if test="$node/Attributes/Notes/text() != ''">
      <table columns="1" width="100%" cellpadding="2.0">
          <xsl:call-template name="HeaderRow">
            <xsl:with-param name="text" select="$notes"/>
          </xsl:call-template>
          <row>
            <cell xsl:use-attribute-sets="ElementTextCell">
              <paragraph xsl:use-attribute-sets="ElementText">
                <xsl:call-template name="MatchLink">
                  <xsl:with-param name="text" select="$node/Attributes/Notes/text()"/>
                </xsl:call-template>
              </paragraph>
            </cell>
          </row>
      </table>
      <paragraph spacingafter="12.0"/>
    </xsl:if>
    <xsl:if test="$node/Attributes/RelatedDocuments/*">
      <table columns="1" width="100%" cellpadding="2.0">
          <xsl:call-template name="HeaderRow">
            <xsl:with-param name="text" select="$relatedDocs"/>
          </xsl:call-template>
          <xsl:for-each select="$node/Attributes/RelatedDocuments/RelatedDocument">
            <xsl:call-template name="ExtLinkRow">
              <xsl:with-param name="text" select="FileName/text()"/>
              <xsl:with-param name="link" select="concat('file:///',translate(FileName/text(),'\\','/'))"/>
            </xsl:call-template>
          </xsl:for-each>
      </table>
    </xsl:if>
  </xsl:template>
  <xsl:template name="HeaderRow">
    <xsl:param name="colspan" select="1"/>
    <xsl:param name="text"/>
    <row>
      <xsl:element name="cell" use-attribute-sets="ElementTitleCell">
        <xsl:attribute name="colspan">
          <xsl:value-of select="$colspan"/>
        </xsl:attribute>
        <paragraph xsl:use-attribute-sets="ElementTitle">
          <xsl:value-of select="$text"/>
        </paragraph>
      </xsl:element>
    </row>
  </xsl:template>
  <xsl:template name="NameRow">
    <xsl:param name="text"/>
    <xsl:param name="uniqueID"/>
    <row>
      <cell xsl:use-attribute-sets="ElementNameCell">
        <xsl:choose>
          <xsl:when test="$outputType = 'withLink'">
		    <xsl:element name="anchor" use-attribute-sets="ElementName">
		      <xsl:attribute name="name">
			    <xsl:value-of select="$uniqueID"/>
			  </xsl:attribute>
			  <paragraph>
    		    <xsl:element name="chunk">
	   	          <xsl:attribute name="generictag">
		            <xsl:value-of select="$text"/>
		          </xsl:attribute>
		          <xsl:value-of select="$text"/>
		        </xsl:element>
		      </paragraph>
            </xsl:element>  
          </xsl:when>  
          <xsl:otherwise>
            <paragraph xsl:use-attribute-sets="ElementName">
              <xsl:value-of select="$text"/>
		    </paragraph>
          </xsl:otherwise>
        </xsl:choose>
      </cell>
    </row>
  </xsl:template>
  <xsl:template name="TextRow">
    <xsl:param name="text"/>
    <row>
      <cell xsl:use-attribute-sets="ElementTextCell">
        <paragraph xsl:use-attribute-sets="ElementText">
          <xsl:value-of select="$text"/>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <xsl:template name="ExtLinkRow">
    <xsl:param name="text"/>
    <xsl:param name="link"/>
    <row>
      <cell xsl:use-attribute-sets="ElementLinkCell">
        <paragraph>
          <xsl:element name="anchor" use-attribute-sets="ElementLink">
            <xsl:attribute name="reference">
              <xsl:value-of select="$link"/>
            </xsl:attribute>
            <xsl:value-of select="$text"/>
          </xsl:element>
        </paragraph>
      </cell>
    </row>
  </xsl:template>
  <xsl:template name="IntLinkRow">
    <xsl:param name="text"/>
    <xsl:param name="link"/>
    <row>
      <cell xsl:use-attribute-sets="ElementLinkCell">
        <xsl:choose>
          <xsl:when test="$outputType = 'withLink'">
            <paragraph>
              <xsl:element name="anchor" use-attribute-sets="ElementLink">
                <xsl:attribute name="reference">
                  <xsl:value-of select="concat('#',$link)"/>
                </xsl:attribute>
                <xsl:value-of select="$text"/>
              </xsl:element>
            </paragraph>
          </xsl:when>
          <xsl:otherwise>
            <paragraph xsl:use-attribute-sets="ElementLink">
              <xsl:value-of select="$text"/>
            </paragraph>
          </xsl:otherwise>
        </xsl:choose>
      </cell>
    </row>
  </xsl:template>
  <xsl:template name="MatchLink">
    <xsl:param name="text"/>
    <xsl:choose>
      <xsl:when test="contains($text,'&#34;') and string-length($text) > 1">
        <xsl:variable name="start" select="substring-after($text, '&#34;')" />
        <xsl:variable name="end" select="substring-before($start, '&#34;')" />
        <xsl:choose>
          <xsl:when test="$end">
            <xsl:if test="$start">
			  <xsl:call-template name="lf2nl"> 
                <xsl:with-param name="text" select="substring-before($text,$end)"/>
              </xsl:call-template>
            </xsl:if>
            <xsl:choose>
              <xsl:when test="//Glossary/*[@Name = $end]">
                <xsl:call-template name="MakeAnchorLink">
                  <xsl:with-param name="glossary_uid" select="//Glossary/@UniqueID"/>
                  <xsl:with-param name="text" select="$end"/>
                  <xsl:with-param name="node" select="//Glossary/*[@Name = $end]"/>
                </xsl:call-template>
                <xsl:call-template name="MatchLink">
                  <xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="//*[@Path = $end]">
                    <xsl:call-template name="MakePathLink">
                      <xsl:with-param name="text" select="$end"/>
                      <xsl:with-param name="node" select="//*[@Path = $end]"/>
                    </xsl:call-template>
                    <xsl:call-template name="MatchLink">
                      <xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:when test="//*[@Name = $end]">
                    <xsl:call-template name="MakeNameLink">
                      <xsl:with-param name="text" select="$end"/>
                      <xsl:with-param name="node" select="//*[@Name = $end]"/>
                    </xsl:call-template>
                    <xsl:call-template name="MatchLink">
                      <xsl:with-param name="text" select="substring-after($text,concat($end,'&#34;'))" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="lf2nl">
                      <xsl:with-param name="text" select="$end"/>
                    </xsl:call-template>
                    <xsl:call-template name="MatchLink">
                      <xsl:with-param name="text" select="substring-after($text,$end)"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="MatchLink">
              <xsl:with-param name="text" select="substring-after($text,$start)"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="lf2nl">
          <xsl:with-param name="text" select="$text"/>
        </xsl:call-template>  
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="MakeNameLink">
    <xsl:param name="text"/>
    <xsl:param name="node"/>
    <xsl:choose>
      <xsl:when test="$outputType = 'withLink'">
        <xsl:element name="anchor" use-attribute-sets="ElementLink">
          <xsl:attribute name="reference">
            <xsl:value-of select="concat('#',$node/@UniqueID)"/>
          </xsl:attribute>
          <xsl:value-of select="$text"/>
        </xsl:element>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:when>
      <xsl:otherwise>
        <chunk xsl:use-attribute-sets="ElementLink">
          <xsl:value-of select="$text"/>
        </chunk>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="MakePathLink">
    <xsl:param name="text"/>
    <xsl:param name="node"/>
    <xsl:choose>
      <xsl:when test="$outputType = 'withLink'">
        <xsl:element name="anchor" use-attribute-sets="ElementLink">
          <xsl:attribute name="reference">
            <xsl:value-of select="concat('#',$node/@UniqueID)"/>
          </xsl:attribute>
          <xsl:value-of select="$text"/>
        </xsl:element>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:when>
      <xsl:otherwise>
        <chunk xsl:use-attribute-sets="ElementLink">
          <xsl:value-of select="$text"/>
        </chunk>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="MakeAnchorLink">
    <xsl:param name="glossary_uid"/>
    <xsl:param name="text"/>
    <xsl:param name="node"/>
    <xsl:choose>
      <xsl:when test="$outputType = 'withLink'">
        <xsl:element name="anchor" use-attribute-sets="ElementLink">
          <xsl:attribute name="reference">
            <xsl:value-of select="concat('#',$node/@UniqueID)"/>
          </xsl:attribute>
          <xsl:value-of select="$text"/>
        </xsl:element>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:when>
      <xsl:otherwise>
        <chunk xsl:use-attribute-sets="ElementLink">
          <xsl:value-of select="$text"/>
        </chunk>
        <chunk>
          <xsl:value-of select="'&#34;'" />
        </chunk>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="lf2nl">
    <xsl:param name="text" />
    <xsl:choose>
		<xsl:when test="contains($text, '&#10;')">
			<xsl:variable name="head" select="substring-before($text, '&#10;')" />
			<xsl:variable name="tail" select="substring-after($text, '&#10;')" />
			<xsl:value-of select="$head" />
			<newline/>
			<xsl:if test="$tail">
				<xsl:call-template name="lf2nl">
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
