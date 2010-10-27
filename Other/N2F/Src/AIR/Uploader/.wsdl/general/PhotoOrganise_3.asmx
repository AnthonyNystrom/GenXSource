<?xml version="1.0" encoding="UTF-8"?><wsdl:definitions xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/" xmlns:http="http://schemas.xmlsoap.org/wsdl/http/" xmlns:mime="http://schemas.xmlsoap.org/wsdl/mime/" xmlns:s="http://www.w3.org/2001/XMLSchema" xmlns:s1="http://tempuri.org/AbstractTypes" xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap/" xmlns:soap12="http://schemas.xmlsoap.org/wsdl/soap12/" xmlns:soapenc="http://schemas.xmlsoap.org/soap/encoding/" xmlns:tm="http://microsoft.com/wsdl/mime/textMatching/" xmlns:tns="http://tempuri.org/" targetNamespace="http://tempuri.org/">
  <wsdl:types>
    <s:schema elementFormDefault="qualified" targetNamespace="http://tempuri.org/">
      <s:element name="Login">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="Email" type="s:string"/>
            <s:element maxOccurs="1" minOccurs="0" name="Password" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="LoginResponse">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="LoginResult" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="CreateNewCollection">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="NewFolderName" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="CreateNewCollectionResponse">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="1" name="CreateNewCollectionResult" type="s:boolean"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="GetCollections">
        <s:complexType/>
      </s:element>
      <s:element name="GetCollectionsResponse">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="GetCollectionsResult" type="tns:ArrayOfPhotoCollectionItem"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfPhotoCollectionItem">
        <s:sequence>
          <s:element maxOccurs="unbounded" minOccurs="0" name="PhotoCollectionItem" nillable="true" type="tns:PhotoCollectionItem"/>
        </s:sequence>
      </s:complexType>
      <s:complexType name="PhotoCollectionItem">
        <s:sequence>
          <s:element maxOccurs="1" minOccurs="0" name="WebPhotoCollectionID" type="s:string"/>
          <s:element maxOccurs="1" minOccurs="0" name="Name" type="s:string"/>
        </s:sequence>
      </s:complexType>
      <s:element name="GetPhotosByCollection">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="WebPhotoCollectionID" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="GetPhotosByCollectionResponse">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="GetPhotosByCollectionResult" type="tns:ArrayOfPhotoItem"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="ArrayOfPhotoItem">
        <s:sequence>
          <s:element maxOccurs="unbounded" minOccurs="0" name="PhotoItem" nillable="true" type="tns:PhotoItem"/>
        </s:sequence>
      </s:complexType>
      <s:complexType name="PhotoItem">
        <s:sequence>
          <s:element maxOccurs="1" minOccurs="0" name="WebPhotoID" type="s:string"/>
          <s:element maxOccurs="1" minOccurs="0" name="MainPhotoURL" type="s:string"/>
          <s:element maxOccurs="1" minOccurs="0" name="ThumbnailURL" type="s:string"/>
        </s:sequence>
      </s:complexType>
      <s:element name="UploadPhoto">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="WebPhotoCollectionID" type="s:string"/>
            <s:element maxOccurs="1" minOccurs="0" name="MainPhotoFilebytes" type="s:base64Binary"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="UploadPhotoResponse">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="1" name="UploadPhotoResult" type="s:boolean"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="RenameCollection">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="WebPhotoCollectionID" type="s:string"/>
            <s:element maxOccurs="1" minOccurs="0" name="NewName" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="RenameCollectionResponse">
        <s:complexType/>
      </s:element>
      <s:element name="DeletePhoto">
        <s:complexType>
          <s:sequence>
            <s:element maxOccurs="1" minOccurs="0" name="WebPhotoID" type="s:string"/>
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="DeletePhotoResponse">
        <s:complexType/>
      </s:element>
      <s:element name="string" nillable="true" type="s:string"/>
      <s:element name="boolean" type="s:boolean"/>
      <s:element name="ArrayOfPhotoCollectionItem" nillable="true" type="tns:ArrayOfPhotoCollectionItem"/>
      <s:element name="ArrayOfPhotoItem" nillable="true" type="tns:ArrayOfPhotoItem"/>
    </s:schema>
    <s:schema targetNamespace="http://tempuri.org/AbstractTypes">
      <s:import namespace="http://schemas.xmlsoap.org/soap/encoding/"/>
      <s:complexType name="StringArray">
        <s:complexContent mixed="false">
          <s:restriction base="soapenc:Array">
            <s:sequence>
              <s:element maxOccurs="unbounded" minOccurs="0" name="String" type="s:string"/>
            </s:sequence>
          </s:restriction>
        </s:complexContent>
      </s:complexType>
    </s:schema>
  </wsdl:types>
  <wsdl:message name="GetPhotosByCollectionHttpGetOut">
    <wsdl:part element="tns:ArrayOfPhotoItem" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="UploadPhotoHttpGetIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
    <wsdl:part name="MainPhotoFilebytes" type="s1:StringArray">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="DeletePhotoHttpGetOut">
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionHttpGetOut">
    <wsdl:part element="tns:boolean" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionSoapIn">
    <wsdl:part element="tns:CreateNewCollection" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetPhotosByCollectionHttpPostOut">
    <wsdl:part element="tns:ArrayOfPhotoItem" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionHttpGetIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
    <wsdl:part name="NewName" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="UploadPhotoHttpGetOut">
    <wsdl:part element="tns:boolean" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginHttpPostIn">
    <wsdl:part name="Email" type="s:string">
    </wsdl:part>
    <wsdl:part name="Password" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetPhotosByCollectionHttpGetIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetCollectionsSoapIn">
    <wsdl:part element="tns:GetCollections" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="DeletePhotoSoapOut">
    <wsdl:part element="tns:DeletePhotoResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionSoapOut">
    <wsdl:part element="tns:RenameCollectionResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetCollectionsHttpGetIn">
  </wsdl:message>
  <wsdl:message name="DeletePhotoHttpGetIn">
    <wsdl:part name="WebPhotoID" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionSoapIn">
    <wsdl:part element="tns:RenameCollection" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginHttpGetOut">
    <wsdl:part element="tns:string" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="DeletePhotoHttpPostIn">
    <wsdl:part name="WebPhotoID" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="UploadPhotoHttpPostOut">
    <wsdl:part element="tns:boolean" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="DeletePhotoSoapIn">
    <wsdl:part element="tns:DeletePhoto" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginSoapIn">
    <wsdl:part element="tns:Login" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionHttpGetOut">
  </wsdl:message>
  <wsdl:message name="GetCollectionsSoapOut">
    <wsdl:part element="tns:GetCollectionsResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetPhotosByCollectionSoapOut">
    <wsdl:part element="tns:GetPhotosByCollectionResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionHttpGetIn">
    <wsdl:part name="NewFolderName" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetCollectionsHttpPostIn">
  </wsdl:message>
  <wsdl:message name="GetCollectionsHttpGetOut">
    <wsdl:part element="tns:ArrayOfPhotoCollectionItem" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionSoapOut">
    <wsdl:part element="tns:CreateNewCollectionResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginHttpPostOut">
    <wsdl:part element="tns:string" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginHttpGetIn">
    <wsdl:part name="Email" type="s:string">
    </wsdl:part>
    <wsdl:part name="Password" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionHttpPostIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
    <wsdl:part name="NewName" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionHttpPostIn">
    <wsdl:part name="NewFolderName" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetPhotosByCollectionSoapIn">
    <wsdl:part element="tns:GetPhotosByCollection" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="RenameCollectionHttpPostOut">
  </wsdl:message>
  <wsdl:message name="UploadPhotoHttpPostIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
    <wsdl:part name="MainPhotoFilebytes" type="s1:StringArray">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetCollectionsHttpPostOut">
    <wsdl:part element="tns:ArrayOfPhotoCollectionItem" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="LoginSoapOut">
    <wsdl:part element="tns:LoginResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="GetPhotosByCollectionHttpPostIn">
    <wsdl:part name="WebPhotoCollectionID" type="s:string">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="DeletePhotoHttpPostOut">
  </wsdl:message>
  <wsdl:message name="CreateNewCollectionHttpPostOut">
    <wsdl:part element="tns:boolean" name="Body">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="UploadPhotoSoapOut">
    <wsdl:part element="tns:UploadPhotoResponse" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:message name="UploadPhotoSoapIn">
    <wsdl:part element="tns:UploadPhoto" name="parameters">
    </wsdl:part>
  </wsdl:message>
  <wsdl:portType name="PhotoOrganiseSoap">
    <wsdl:operation name="Login">
      <wsdl:input message="tns:LoginSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:LoginSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <wsdl:input message="tns:CreateNewCollectionSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:CreateNewCollectionSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <wsdl:input message="tns:GetCollectionsSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:GetCollectionsSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <wsdl:input message="tns:GetPhotosByCollectionSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:GetPhotosByCollectionSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <wsdl:input message="tns:UploadPhotoSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:UploadPhotoSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <wsdl:input message="tns:RenameCollectionSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:RenameCollectionSoapOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <wsdl:input message="tns:DeletePhotoSoapIn">
    </wsdl:input>
      <wsdl:output message="tns:DeletePhotoSoapOut">
    </wsdl:output>
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:portType name="PhotoOrganiseHttpGet">
    <wsdl:operation name="Login">
      <wsdl:input message="tns:LoginHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:LoginHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <wsdl:input message="tns:CreateNewCollectionHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:CreateNewCollectionHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <wsdl:input message="tns:GetCollectionsHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:GetCollectionsHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <wsdl:input message="tns:GetPhotosByCollectionHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:GetPhotosByCollectionHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <wsdl:input message="tns:UploadPhotoHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:UploadPhotoHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <wsdl:input message="tns:RenameCollectionHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:RenameCollectionHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <wsdl:input message="tns:DeletePhotoHttpGetIn">
    </wsdl:input>
      <wsdl:output message="tns:DeletePhotoHttpGetOut">
    </wsdl:output>
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:portType name="PhotoOrganiseHttpPost">
    <wsdl:operation name="Login">
      <wsdl:input message="tns:LoginHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:LoginHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <wsdl:input message="tns:CreateNewCollectionHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:CreateNewCollectionHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <wsdl:input message="tns:GetCollectionsHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:GetCollectionsHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <wsdl:input message="tns:GetPhotosByCollectionHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:GetPhotosByCollectionHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <wsdl:input message="tns:UploadPhotoHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:UploadPhotoHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <wsdl:input message="tns:RenameCollectionHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:RenameCollectionHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <wsdl:input message="tns:DeletePhotoHttpPostIn">
    </wsdl:input>
      <wsdl:output message="tns:DeletePhotoHttpPostOut">
    </wsdl:output>
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:binding name="PhotoOrganiseSoap" type="tns:PhotoOrganiseSoap">
    <soap:binding transport="http://schemas.xmlsoap.org/soap/http"/>
    <wsdl:operation name="Login">
      <soap:operation soapAction="http://tempuri.org/Login" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <soap:operation soapAction="http://tempuri.org/CreateNewCollection" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <soap:operation soapAction="http://tempuri.org/GetCollections" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <soap:operation soapAction="http://tempuri.org/GetPhotosByCollection" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <soap:operation soapAction="http://tempuri.org/UploadPhoto" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <soap:operation soapAction="http://tempuri.org/RenameCollection" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <soap:operation soapAction="http://tempuri.org/DeletePhoto" style="document"/>
      <wsdl:input>
        <soap:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:binding name="PhotoOrganiseSoap12" type="tns:PhotoOrganiseSoap">
    <soap12:binding transport="http://schemas.xmlsoap.org/soap/http"/>
    <wsdl:operation name="Login">
      <soap12:operation soapAction="http://tempuri.org/Login" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <soap12:operation soapAction="http://tempuri.org/CreateNewCollection" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <soap12:operation soapAction="http://tempuri.org/GetCollections" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <soap12:operation soapAction="http://tempuri.org/GetPhotosByCollection" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <soap12:operation soapAction="http://tempuri.org/UploadPhoto" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <soap12:operation soapAction="http://tempuri.org/RenameCollection" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <soap12:operation soapAction="http://tempuri.org/DeletePhoto" style="document"/>
      <wsdl:input>
        <soap12:body use="literal"/>
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal"/>
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:binding name="PhotoOrganiseHttpGet" type="tns:PhotoOrganiseHttpGet">
    <http:binding verb="GET"/>
    <wsdl:operation name="Login">
      <http:operation location="/Login"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <http:operation location="/CreateNewCollection"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <http:operation location="/GetCollections"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <http:operation location="/GetPhotosByCollection"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <http:operation location="/UploadPhoto"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <http:operation location="/RenameCollection"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <http:operation location="/DeletePhoto"/>
      <wsdl:input>
        <http:urlEncoded/>
      </wsdl:input>
      <wsdl:output>
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:binding name="PhotoOrganiseHttpPost" type="tns:PhotoOrganiseHttpPost">
    <http:binding verb="POST"/>
    <wsdl:operation name="Login">
      <http:operation location="/Login"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="CreateNewCollection">
      <http:operation location="/CreateNewCollection"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetCollections">
      <http:operation location="/GetCollections"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetPhotosByCollection">
      <http:operation location="/GetPhotosByCollection"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="UploadPhoto">
      <http:operation location="/UploadPhoto"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
        <mime:mimeXml part="Body"/>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="RenameCollection">
      <http:operation location="/RenameCollection"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="DeletePhoto">
      <http:operation location="/DeletePhoto"/>
      <wsdl:input>
        <mime:content type="application/x-www-form-urlencoded"/>
      </wsdl:input>
      <wsdl:output>
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:service name="PhotoOrganise">
    <wsdl:port binding="tns:PhotoOrganiseSoap" name="PhotoOrganiseSoap">
      <soap:address location="http://12.206.33.16/n2fwebservices/PhotoOrganise.asmx"/>
    </wsdl:port>
    <wsdl:port binding="tns:PhotoOrganiseHttpGet" name="PhotoOrganiseHttpGet">
      <http:address location="http://12.206.33.16/n2fwebservices/PhotoOrganise.asmx"/>
    </wsdl:port>
    <wsdl:port binding="tns:PhotoOrganiseSoap12" name="PhotoOrganiseSoap12">
      <soap12:address location="http://12.206.33.16/n2fwebservices/PhotoOrganise.asmx"/>
    </wsdl:port>
    <wsdl:port binding="tns:PhotoOrganiseHttpPost" name="PhotoOrganiseHttpPost">
      <http:address location="http://12.206.33.16/n2fwebservices/PhotoOrganise.asmx"/>
    </wsdl:port>
  </wsdl:service>
</wsdl:definitions>