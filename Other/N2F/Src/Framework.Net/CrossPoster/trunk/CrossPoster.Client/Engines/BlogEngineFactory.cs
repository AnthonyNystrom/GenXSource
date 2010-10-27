/* ------------------------------------------------
 * BlogEngineFactory.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client.Engines
{
    static class BlogEngineFactory
    {
        private static IBlogEngine _bloggerEngine;
        private static IBlogEngine _wordPressEngine;
        private static IBlogEngine _livejournalEngine;

        public static IBlogEngine GetEngine(BlogType blogType)
        {
            switch (blogType)
            {
                case BlogType.WordPress:
                    if (_wordPressEngine == null)
                        _wordPressEngine = new WordPressEngine();
                    return _wordPressEngine;
                case BlogType.LiveJournal:
                    if (_livejournalEngine == null)
                        _livejournalEngine = new LiveJournalEngine();
                    return _livejournalEngine;
                default:
                    if (_bloggerEngine == null)
                        _bloggerEngine = new BloggerEngine();
                    return _bloggerEngine;
            }
        }
    }
}
