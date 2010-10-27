<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signups.aspx.cs" Inherits="CMS._Default" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Register assembly="dotnetCHARTING" namespace="dotnetCHARTING" tagprefix="dotnetCHARTING" %>
<html xmlns="http://www.w3.org/1999/xhtml">
      <head>
            <title>Signup numbers </title>

      </head>
      <body>
            <div style="text-align:center">
               
            </div>
      </body>
</html></title>
<form id="form1" runat="server">

<table>

<tr>
<td>
<dotnetCHARTING:Chart ID="Chart" runat="server" ShadingEffectMode="Three" 
    Type="ComboSideBySide" Use3D="True">
<DefaultTitleBox>
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont"></Label>
</DefaultTitleBox>

<SmartForecast Start=""></SmartForecast>

<Background ShadingEffectMode="None"></Background>

<DefaultLegendBox Padding="4" CornerBottomRight="Cut">
<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>

<DefaultEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</DefaultEntry>

<HeaderEntry ShapeType="None" Visible="False">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</HeaderEntry>

<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>
</DefaultLegendBox>

<ChartArea CornerTopLeft="Square" StartDateOfYear="">
<DefaultElement ShapeType="None">
<DefaultSubValue Name="">
    <Line Length="4" />
    </DefaultSubValue>

<SmartLabel GlowColor="" Type="UseFont"></SmartLabel>

<LegendEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</LegendEntry>
</DefaultElement>

<Label GlowColor="" Type="UseFont" Font="Tahoma, 8pt"></Label>

<YAxis GaugeNeedleType="One" GaugeLabelMode="Default" SmartScaleBreakLimit="2">
<ScaleBreakLine Color="Gray"></ScaleBreakLine>

<TimeScaleLabels MaximumRangeRows="4"></TimeScaleLabels>

<MinorTimeIntervalAdvanced Start=""></MinorTimeIntervalAdvanced>

<ZeroTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont"></Label>
</ZeroTick>

<DefaultTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont" Text="%Value"></Label>
</DefaultTick>

<TimeIntervalAdvanced Start=""></TimeIntervalAdvanced>

<AlternateGridBackground ShadingEffectMode="None"></AlternateGridBackground>

<Label GlowColor="" Type="UseFont" Alignment="Center" LineAlignment="Center" Font="Arial, 9pt, style=Bold"></Label>
</YAxis>

<XAxis GaugeNeedleType="One" GaugeLabelMode="Default" SmartScaleBreakLimit="2">
<ScaleBreakLine Color="Gray"></ScaleBreakLine>

<TimeScaleLabels MaximumRangeRows="4"></TimeScaleLabels>

<MinorTimeIntervalAdvanced Start=""></MinorTimeIntervalAdvanced>

<ZeroTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont"></Label>
</ZeroTick>

<DefaultTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont" Text="%Value"></Label>
</DefaultTick>

<TimeIntervalAdvanced Start=""></TimeIntervalAdvanced>

<AlternateGridBackground ShadingEffectMode="None"></AlternateGridBackground>

<Label GlowColor="" Type="UseFont" Alignment="Center" LineAlignment="Center" Font="Arial, 9pt, style=Bold"></Label>
</XAxis>

<Background ShadingEffectMode="None"></Background>

<TitleBox Position="Left">
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont" Color="Black"></Label>
</TitleBox>

<LegendBox Padding="4" Position="Top" CornerBottomRight="Cut">
<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>

<DefaultEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</DefaultEntry>

<HeaderEntry ShapeType="None" Name="Name" Value="Value" Visible="False" SortOrder="-1">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</HeaderEntry>

<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>
</LegendBox>
</ChartArea>

<DefaultElement ShapeType="None">
<DefaultSubValue Name=""></DefaultSubValue>

<SmartLabel GlowColor="" Type="UseFont"></SmartLabel>

<LegendEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</LegendEntry>
</DefaultElement>

<NoDataLabel GlowColor="" Type="UseFont"></NoDataLabel>

<TitleBox Position="Left">
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont" Color="Black"></Label>
</TitleBox>
</dotnetCHARTING:Chart>

</td>
<td>

<dotnetCHARTING:Chart ID="Chart2" runat="server" ShadingEffectMode="Three" 
    Type="Scatter" Use3D="True">
<DefaultTitleBox>
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont"></Label>
</DefaultTitleBox>

<SmartForecast Start=""></SmartForecast>

<Background ShadingEffectMode="None"></Background>

<DefaultLegendBox Padding="4" CornerBottomRight="Cut">
<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>

<DefaultEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</DefaultEntry>

<HeaderEntry ShapeType="None" Visible="False">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</HeaderEntry>

<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>
</DefaultLegendBox>

<ChartArea CornerTopLeft="Square" StartDateOfYear="">
<DefaultElement ShapeType="None">
<DefaultSubValue Name="">
    <Line Length="4" />
    </DefaultSubValue>

<SmartLabel GlowColor="" Type="UseFont"></SmartLabel>

<LegendEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</LegendEntry>
</DefaultElement>

<Label GlowColor="" Type="UseFont" Font="Tahoma, 8pt"></Label>

<YAxis GaugeNeedleType="One" GaugeLabelMode="Default" SmartScaleBreakLimit="2">
<ScaleBreakLine Color="Gray"></ScaleBreakLine>

<TimeScaleLabels MaximumRangeRows="4"></TimeScaleLabels>

<MinorTimeIntervalAdvanced Start=""></MinorTimeIntervalAdvanced>

<ZeroTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont"></Label>
</ZeroTick>

<DefaultTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont" Text="%Value"></Label>
</DefaultTick>

<TimeIntervalAdvanced Start=""></TimeIntervalAdvanced>

<AlternateGridBackground ShadingEffectMode="None"></AlternateGridBackground>

<Label GlowColor="" Type="UseFont" Alignment="Center" LineAlignment="Center" Font="Arial, 9pt, style=Bold"></Label>
</YAxis>

<XAxis GaugeNeedleType="One" GaugeLabelMode="Default" SmartScaleBreakLimit="2">
<ScaleBreakLine Color="Gray"></ScaleBreakLine>

<TimeScaleLabels MaximumRangeRows="4"></TimeScaleLabels>

<MinorTimeIntervalAdvanced Start=""></MinorTimeIntervalAdvanced>

<ZeroTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont"></Label>
</ZeroTick>

<DefaultTick>
<Line Length="3"></Line>

<Label GlowColor="" Type="UseFont" Text="%Value"></Label>
</DefaultTick>

<TimeIntervalAdvanced Start=""></TimeIntervalAdvanced>

<AlternateGridBackground ShadingEffectMode="None"></AlternateGridBackground>

<Label GlowColor="" Type="UseFont" Alignment="Center" LineAlignment="Center" Font="Arial, 9pt, style=Bold"></Label>
</XAxis>

<Background ShadingEffectMode="None"></Background>

<TitleBox Position="Left">
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont" Color="Black"></Label>
</TitleBox>

<LegendBox Padding="4" Position="Top" CornerBottomRight="Cut">
<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>

<DefaultEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</DefaultEntry>

<HeaderEntry ShapeType="None" Name="Name" Value="Value" Visible="False" SortOrder="-1">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</HeaderEntry>

<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>
</LegendBox>
</ChartArea>

<DefaultElement ShapeType="None">
<DefaultSubValue Name=""></DefaultSubValue>

<SmartLabel GlowColor="" Type="UseFont"></SmartLabel>

<LegendEntry ShapeType="None">
<Background ShadingEffectMode="None"></Background>

<LabelStyle GlowColor="" Type="UseFont"></LabelStyle>
</LegendEntry>
</DefaultElement>

<NoDataLabel GlowColor="" Type="UseFont"></NoDataLabel>

<TitleBox Position="Left">
<HeaderLabel GlowColor="" Type="UseFont"></HeaderLabel>

<HeaderBackground ShadingEffectMode="None"></HeaderBackground>

<Background ShadingEffectMode="None"></Background>

<Label GlowColor="" Type="UseFont" Color="Black"></Label>
</TitleBox>
</dotnetCHARTING:Chart>
</td>
</tr>
</table>


</form>
