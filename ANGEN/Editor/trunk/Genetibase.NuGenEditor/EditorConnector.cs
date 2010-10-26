/* -----------------------------------------------
 * EditorConnector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.Adornment;
using Genetibase.Windows.Controls.Editor.AdornmentLibrary.Breakpoint;
using Genetibase.Windows.Controls.Editor.AdornmentLibrary.TextMarker;
using Genetibase.Windows.Controls.Editor.AdornmentLibrary.ToolTip;
using Genetibase.Windows.Controls.Editor.AdornmentSurface;
using Genetibase.Windows.Controls.Editor.AdornmentSurfaceManager;
using Genetibase.Windows.Controls.Editor.Classification;
using Genetibase.Windows.Controls.Editor.Find;
using Genetibase.Windows.Controls.Editor.InputBinding;
using Genetibase.Windows.Controls.Editor.Navigation;
using Genetibase.Windows.Controls.Editor.Undo;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
	internal static class EditorConnector
	{
        public static IMouseBinding CreateMouseBinding(IEditorView avalonTextViewHost)
        {
            return new ClickMouseBinding(avalonTextViewHost.TextView);
        }

        private static IFindLogic _findLogic;

        public static IFindLogic GetFindLogic()
        {
            if (_findLogic == null)
            {
                _findLogic = new FindLogic(GetTextStructureNavigatorFactory());
            }

            return _findLogic;
        }

        private static readonly Dictionary<Object, IUndoManager> _undoManagerLookup = new Dictionary<Object, IUndoManager>();
        private static UndoManager _undoManager;

        public static IUndoManager GetUndoManager(Object key)
        {
            IUndoManager manager;

            if (key == null)
            {
                if (_undoManager == null)
                {
                    _undoManager = new UndoManager();
                }
            
                return _undoManager;
            }

            if (!_undoManagerLookup.TryGetValue(key, out manager))
            {
                manager = new UndoManager();
                _undoManagerLookup[key] = manager;
            }
            return manager;
        }

        private static ITextStructureNavigatorFactory _textStructureNavigatorFactory;
    
        public static ITextStructureNavigatorFactory GetTextStructureNavigatorFactory()
        {
            if (_textStructureNavigatorFactory == null)
            {
                _textStructureNavigatorFactory = new TextStructureNavigationFactory();
            }

            return _textStructureNavigatorFactory;
        }

        public static IAdornmentSurfaceSpaceManager GetAdornmentSurfaceSpaceManager(IEditorArea editorArea)
        {
            IAdornmentSurfaceSpaceManager manager;
            if (editorArea == null)
            {
                throw new ArgumentNullException("editorArea");
            }
            
            IPropertyOwner owner = editorArea;

            if (!owner.TryGetProperty<IAdornmentSurfaceSpaceManager>("IAdornmentSurfaceSpaceManagerMap", out manager))
            {
                throw new ArgumentException("The specified editorArea doesn't have a space manager.");
            }
            return manager;
        }

        public static ITextStructureNavigator GetTextStructureNavigator(TextBuffer textBuffer)
        {
            if (textBuffer == null)
            {
                throw new ArgumentNullException("textBuffer");
            }

            IPropertyOwner owner = textBuffer;
            ITextStructureNavigator property = null;

            if (!owner.TryGetProperty<ITextStructureNavigator>("ITextStructureNavigatorCache", out property))
            {
                property = new NaturalLanguageNavigator(textBuffer);
                owner.AddProperty("ITextStructureNavigatorCache", property);
            }

            return property;
        }

        private static IAdornmentProvider[] _adornmentProviders;

        public static IAdornmentProvider[] GetAdornmentProviders(ITextArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            if (_adornmentProviders == null)
            {
                _adornmentProviders = new IAdornmentProvider[] {
                    CreateBreakpointProviderInternal(textView.TextBuffer)
                    , CreateTextMarkerProvider(textView)
                    , GetToolTipProviderInternal(textView)
                };
            }

            return _adornmentProviders;
        }

        internal static BreakpointProvider CreateBreakpointProviderInternal(TextBuffer textBuffer)
        {
            if (textBuffer == null)
            {
                throw new ArgumentNullException("textBuffer");
            }
            BreakpointProvider property = null;
            IPropertyOwner owner = textBuffer;
            if (!owner.TryGetProperty<BreakpointProvider>("BreakpointProvider", out property))
            {
                property = new BreakpointProvider(textBuffer);
                owner.AddProperty("BreakpointProvider", property);
            }
            return property;
        }

        private static TextMarkerProvider CreateTextMarkerProvider(ITextArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            TextMarkerProvider property = null;
            IPropertyOwner owner = textView;
            if (!owner.TryGetProperty<TextMarkerProvider>("TextMarkerProvider", out property))
            {
                property = new TextMarkerProvider(textView);
                owner.AddProperty("TextMarkerProvider", property);
            }
            return property;
        }

        internal static ToolTipProvider GetToolTipProviderInternal(ITextArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }
            
            ToolTipProvider property = null;
            IPropertyOwner owner = textView;
            
            if (!owner.TryGetProperty<ToolTipProvider>("ToolTipProvider", out property))
            {
                property = new ToolTipProvider(textView);
                owner.AddProperty("ToolTipProvider", property);
            }
            return property;
        }

        public static IAdornmentSurface CreateAdornmentSurface(IEditorArea textView, Type adornmentType)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            if (adornmentType == null)
            {
                throw new ArgumentNullException("adornmentType");
            }

            var factories = GetSurfaceFactories(textView);

            if (factories.ContainsKey(adornmentType))
            {
                return factories[adornmentType];
            }

            return null;
        }

        private static Dictionary<Type, IAdornmentSurface> _surfaceFactories;

        public static Dictionary<Type, IAdornmentSurface> GetSurfaceFactories(IEditorArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            if (_surfaceFactories == null)
            {
                _surfaceFactories = new Dictionary<Type, IAdornmentSurface>();
                _surfaceFactories.Add(typeof(BreakpointAdornment), new BreakpointAdornmentSurface(textView));
                _surfaceFactories.Add(typeof(TextMarkerAdornment), new TextMarkerAdornmentSurface(textView));
                _surfaceFactories.Add(typeof(ToolTipAdornment), new ToolTipAdornmentSurface(textView));
            }

            return _surfaceFactories;
        }

        public static IEditorCommands CreateEditorCommands(ITextArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            IPropertyOwner owner = textView;
            IEditorCommands property = null;

            if (!owner.TryGetProperty<IEditorCommands>("IEditorCommandsProvider", out property))
            {
                property = new EditorCommands(textView);
                owner.AddProperty("IEditorCommandsProvider", property);
            }

            return property;
        }

        public static IAdornmentProvider GetAdornmentAggregator(ITextArea textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            IPropertyOwner owner = textView;
            IAdornmentProvider property = null;

            if (!owner.TryGetProperty<IAdornmentProvider>("IAdornmentAggregatorCache", out property))
            {
                property = new AdornmentAggregator(textView);
                owner.AddProperty("IAdornmentAggregatorCache", property);
            }

            return property;
        }

        public static IAdornmentSurfaceManager GetAdornmentSurfaceManager(IAdornmentSurfaceHost surfaceHost)
        {
            AdornmentSurfaceManager manager;

            if (surfaceHost == null)
            {
                throw new ArgumentNullException("surfaceHost");
            }

            if (surfaceHost.TextView == null)
            {
                throw new ArgumentException("The specified surfaceHost isn't valid.");
            }

            IPropertyOwner textView = surfaceHost.TextView;

            if (!textView.TryGetProperty<AdornmentSurfaceManager>("IAdornmentSurfaceManagerFactory", out manager))
            {
                manager = new AdornmentSurfaceManager(surfaceHost);
                textView.AddProperty("IAdornmentSurfaceManagerFactory", manager);
            }

            return manager;
        }

        public static IClassifier GetClassifierAggregator(TextBuffer textBuffer)
        {
            ClassifierAggregator aggregator;

            if (textBuffer == null)
            {
                throw new ArgumentNullException("textBuffer");
            }

            IPropertyOwner owner = textBuffer;

            if (!owner.TryGetProperty<ClassifierAggregator>("IClassifierAggregatorProvider", out aggregator))
            {
                aggregator = new ClassifierAggregator(textBuffer);
                owner.AddProperty("IClassifierAggregatorProvider", aggregator);
            }

            return aggregator;
        }

        private static ITextBufferFactory _linkBufferFactory;

        public static ITextBufferFactory GetLinkBufferFactory()
        {
            if (_linkBufferFactory == null)
            {
                _linkBufferFactory = new LinkBufferFactory();
            }

            return _linkBufferFactory;
        }

        private static Dictionary<String, IClassificationFormatMap> _cachedFormatMaps;

        public static IClassificationFormatMap SelectClassificationFormatMap(String documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException("documentType");
            }
            
            if (_cachedFormatMaps == null)
            {
                _cachedFormatMaps = new Dictionary<String, IClassificationFormatMap>();
            }

            documentType = DocumentTypeHelper.GetNormalizedDocumentType(documentType);
            IClassificationFormatMap formatMapWithFallback = null;
            
            if (!_cachedFormatMaps.TryGetValue(documentType, out formatMapWithFallback))
            {
                formatMapWithFallback = GetFormatMapWithFallback(documentType);
                _cachedFormatMaps[documentType] = formatMapWithFallback;
            }

            return formatMapWithFallback;
        }

        private static IClassificationFormatMap GetFormatMapWithFallback(String documentType)
        {
            while (documentType.Length != 0)
            {
                IClassificationFormatMap formatMapWithoutFallback = GetFormatMapWithoutFallback(documentType);
                
                if (formatMapWithoutFallback != null)
                {
                    return formatMapWithoutFallback;
                }
                
                documentType = DocumentTypeHelper.GetParentDocumentType(documentType);
            }

            return new DefaultClassificationFormatMap();
        }

        private static IClassificationFormatMap GetFormatMapWithoutFallback(String documentType)
        {
            foreach (var item in ClassificationFormatMap)
            {
                if (DocumentTypeHelper.AreDocumentTypesSame(item.Key, documentType))
                {
                    return item.Value;
                }
            }

            return null;
        }

        private static Dictionary<String, IClassificationFormatMap> _classificationFormatMaps;

        private static Dictionary<String, IClassificationFormatMap> ClassificationFormatMap
        {
            get
            {
                if (_classificationFormatMaps == null)
                {
                    _classificationFormatMaps = new Dictionary<String, IClassificationFormatMap>();
                    var classificationFormatMap = new ClassificationFormatMap();
                    var xmlWordClassificationLookup = new XmlWordClassificationLookup();
                    _classificationFormatMaps.Add("text.cs", classificationFormatMap);
                    _classificationFormatMaps.Add("text.vb", classificationFormatMap);
                    _classificationFormatMaps.Add("text", classificationFormatMap);
                    _classificationFormatMaps.Add("text.xml", xmlWordClassificationLookup);
                }

                return _classificationFormatMaps;
            }
        }
	}
}
