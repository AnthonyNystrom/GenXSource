<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:UML="http://www.omg.org/UML">
	<xsl:output method="xml" encoding="UTF-8" />
	<xsl:preserve-space elements="UML" />
	
	<xsl:template match="/">
		<XMI xmi.version="1.1" xmlns:UML="http://www.omg.org/UML">
			<XMI.header>
				<XMI.documentation>
					<XMI.exporter>Use Case Maker</XMI.exporter><!-- Unisys.JCR.2 -->
					<XMI.exporterVersion>1.0.0</XMI.exporterVersion><!-- 1.3.4 -->
				</XMI.documentation>
				<XMI.metamodel xmi.name="UML" xmi.version="1.3"/>
			</XMI.header>
			<XMI.content>
				<xsl:apply-templates select="//Model" />
				<xsl:for-each select="//Model | //Package | //Actor | //UseCase">
					<xsl:if test="Attributes/Description/text()">
						<xsl:apply-templates mode="taggedvalue" select="Attributes/Description">
							<xsl:with-param name="uid" select="@UniqueID" />
							<xsl:with-param name="value" select="Attributes/Description/text()" />
						</xsl:apply-templates>
					</xsl:if>
				</xsl:for-each>
			</XMI.content>
		</XMI>
	</xsl:template>
	
	<xsl:template match="Model">
		<xsl:element name="UML:Model">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="@UniqueID" />
			</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name" />
			</xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<UML:Namespace.ownedElement>
				<!-- Stereotype definition -->
				<xsl:element name="UML:Stereotype">
					<xsl:attribute name="xmi.id">
						<xsl:value-of select="concat('st_',@UniqueID)" />
					</xsl:attribute>
					<xsl:attribute name="name">Use Case Model</xsl:attribute>
					<xsl:attribute name="isSpecification">false</xsl:attribute>
					<xsl:attribute name="visibility">public</xsl:attribute>
					<xsl:attribute name="isAbstract">false</xsl:attribute>
					<xsl:attribute name="isLeaf">false</xsl:attribute>
					<xsl:attribute name="isRoot">false</xsl:attribute>
					<xsl:attribute name="baseClass">Model</xsl:attribute>
					<xsl:attribute name="extendedElement">
						<xsl:value-of select="@UniqueID" />
					</xsl:attribute>
				</xsl:element>
				<!-- Subpackages -->
				<xsl:if test="Packages/Package">
					<xsl:apply-templates select="Packages/Package" />
				</xsl:if>
				<!-- Actors -->
				<xsl:if test="Actors/Actor">
					<xsl:apply-templates select="Actors" />
				</xsl:if>
				<!-- UseCases -->
				<xsl:if test="UseCases/UseCase">
					<xsl:apply-templates select="UseCases" />
				</xsl:if>
			</UML:Namespace.ownedElement>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="Packages">
		<xsl:apply-templates select="Package" />
	</xsl:template>
	
	<xsl:template match="Actors">
		<xsl:apply-templates select="Actor" />
	</xsl:template>
	
	<xsl:template match="UseCases">
		<xsl:apply-templates select="UseCase" />
	</xsl:template>

	<xsl:template match="Package">
		<xsl:element name="UML:Package">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="@UniqueID" />
			</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name" />
			</xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<UML:Namespace.ownedElement>
				<xsl:element name="UML:Stereotype">
					<xsl:attribute name="xmi.id">
						<xsl:value-of select="concat('st_',@UniqueID)" />
					</xsl:attribute>
					<xsl:attribute name="name">Use Case Package</xsl:attribute>
					<xsl:attribute name="isSpecification">false</xsl:attribute>
					<xsl:attribute name="visibility">public</xsl:attribute>
					<xsl:attribute name="isAbstract">false</xsl:attribute>
					<xsl:attribute name="isLeaf">false</xsl:attribute>
					<xsl:attribute name="isRoot">false</xsl:attribute>
					<xsl:attribute name="baseClass">Package</xsl:attribute>
					<xsl:attribute name="extendedElement">
						<xsl:value-of select="@UniqueID" />
					</xsl:attribute>
				</xsl:element>
				<xsl:if test="Packages/Package">
					<xsl:apply-templates select="Packages" />
				</xsl:if>
				<xsl:if test="Actors/Actor">
					<xsl:apply-templates select="Actors" />
				</xsl:if>
				<xsl:if test="UseCases/UseCase">
					<xsl:apply-templates select="UseCases" />
				</xsl:if>
			</UML:Namespace.ownedElement>	
		</xsl:element>
	</xsl:template>

	<xsl:template match="Actor">
		<xsl:element name="UML:Actor">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="@UniqueID" />
			</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name" />
			</xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<xsl:attribute name="isAbstract">false</xsl:attribute>
			<xsl:attribute name="isLeaf">false</xsl:attribute>
			<xsl:attribute name="isRoot">false</xsl:attribute>
			
			<xsl:variable name="aid" select="@UniqueID" />
			<xsl:for-each select="//UseCase">
				<xsl:if test="ActiveActors/ActiveActor/ActorUniqueID/text() = $aid">
					<UML:Namespace.ownedElement>
						<xsl:call-template name="association">
							<xsl:with-param name="ucid" select="@UniqueID" />
							<xsl:with-param name="aid" select="$aid" />
						</xsl:call-template>
					</UML:Namespace.ownedElement>
				</xsl:if>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>

	<xsl:template match="UseCase">
		<xsl:variable name="ucid" select="@UniqueID" />
	
		<xsl:element name="UML:UseCase">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="@UniqueID" />
			</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name" />
			</xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<xsl:attribute name="isAbstract">false</xsl:attribute>
			<xsl:attribute name="isLeaf">false</xsl:attribute>
			<xsl:attribute name="isRoot">false</xsl:attribute>
		</xsl:element>
		
		<xsl:for-each select="Steps/Step/Dependency">
			<xsl:variable name="ref_ucid" select="PartnerUniqueID/text()" />
			<xsl:if test="$ref_ucid != ''">
				<xsl:if test="Type = 'Client'">
					<xsl:call-template name="dependency">
						<xsl:with-param name="client_ucid" select="$ucid" />
						<xsl:with-param name="supplier_ucid" select="$ref_ucid" />
						<xsl:with-param name="stereotype" select="Stereotype/text()" />
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="Type = 'Supplier'">
					<xsl:call-template name="dependency">
						<xsl:with-param name="client_ucid" select="$ref_ucid" />
						<xsl:with-param name="supplier_ucid" select="$ucid" />
						<xsl:with-param name="stereotype" select="Stereotype/text()" />
					</xsl:call-template>				
				</xsl:if>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="dependency">
		<xsl:param name="client_ucid" />
		<xsl:param name="supplier_ucid" />
		<xsl:param name="stereotype" />
		
		<xsl:element name="UML:Dependency">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="concat('dep_',$client_ucid,'_',$supplier_ucid)" />
			</xsl:attribute>
			<xsl:attribute name="name"></xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<xsl:attribute name="isAbstract">false</xsl:attribute>
			<xsl:attribute name="isLeaf">false</xsl:attribute>
			<xsl:attribute name="isRoot">false</xsl:attribute>
			<xsl:attribute name="client">
				<xsl:value-of select="$client_ucid" />
			</xsl:attribute>
			<xsl:attribute name="supplier">
				<xsl:value-of select="$supplier_ucid" />
			</xsl:attribute>
		</xsl:element>
		<xsl:if test="$stereotype != ''">
			<UML:Stereotype>
				<xsl:attribute name="xmi.id">
					<xsl:value-of select="concat('st_',$client_ucid,'_',$supplier_ucid)" />
				</xsl:attribute>
				<xsl:attribute name="name">
					<xsl:value-of select="$stereotype" />
				</xsl:attribute>
				<xsl:attribute name="extendedElement">
					<xsl:value-of select="concat('dep_',$client_ucid,'_',$supplier_ucid)" />
				</xsl:attribute>
			</UML:Stereotype>
		</xsl:if>		
	</xsl:template>
	
	<xsl:template name="association">
		<xsl:param name="ucid" />
		<xsl:param name="aid" />
		
		<xsl:element name="UML:Association">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="concat('ass_',$aid,'_',$ucid)" />
			</xsl:attribute>
			<xsl:attribute name="name"></xsl:attribute>
			<xsl:attribute name="isSpecification">false</xsl:attribute>
			<xsl:attribute name="visibility">public</xsl:attribute>
			<xsl:attribute name="isAbstract">false</xsl:attribute>
			<xsl:attribute name="isLeaf">false</xsl:attribute>
			<xsl:attribute name="isRoot">false</xsl:attribute>
			<UML:Association.connection>
				<xsl:element name="UML:AssociationEnd">
					<xsl:attribute name="xmi.id">
						<xsl:value-of select="concat($ucid,'_',$aid)" />
					</xsl:attribute>
					<xsl:attribute name="aggregation">none</xsl:attribute>
					<xsl:attribute name="isNavigable">true</xsl:attribute>
					<xsl:attribute name="changeability">changeable</xsl:attribute>
					<xsl:attribute name="targetScope">instance</xsl:attribute>
					<xsl:attribute name="visibility">private</xsl:attribute>
					<xsl:attribute name="isSpecification">false</xsl:attribute>
					<xsl:attribute name="type">
						<xsl:value-of select="$ucid" />
					</xsl:attribute>
				</xsl:element>
				<xsl:element name="UML:AssociationEnd">
					<xsl:attribute name="xmi.id">
						<xsl:value-of select="concat($aid,'_',$ucid)" />
					</xsl:attribute>	
					<xsl:attribute name="aggregation">none</xsl:attribute>
					<xsl:attribute name="isNavigable">false</xsl:attribute>
					<xsl:attribute name="changeability">changeable</xsl:attribute>
					<xsl:attribute name="targetScope">instance</xsl:attribute>
					<xsl:attribute name="visibility">private</xsl:attribute>
					<xsl:attribute name="isSpecification">false</xsl:attribute>
					<xsl:attribute name="type">
						<xsl:value-of select="$aid" />
					</xsl:attribute>
				</xsl:element>
			</UML:Association.connection>
		</xsl:element>
	</xsl:template>

	<xsl:template mode="taggedvalue" match="*">
		<xsl:param name="uid" />
		<xsl:param name="value" />

		<xsl:element name="UML:TaggedValue">
			<xsl:attribute name="xmi.id">
				<xsl:value-of select="concat('doc_',$uid)" />
			</xsl:attribute>
			<xsl:attribute name="tag">documentation</xsl:attribute>
			<xsl:attribute name="value">
				<xsl:value-of select="$value" />
			</xsl:attribute>
			<UML:TaggedValue.modelElement>
			<xsl:element name="UML:ModelElement">
				<xsl:attribute name="xmi.idref">
					<xsl:value-of select="$uid" />
				</xsl:attribute>
			</xsl:element>
			</UML:TaggedValue.modelElement>
		</xsl:element>
	</xsl:template>

</xsl:stylesheet>
